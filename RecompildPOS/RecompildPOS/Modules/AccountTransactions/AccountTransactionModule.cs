using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Plugin.Connectivity;
using RecompildPOS.Database;
using RecompildPOS.Extensions;
using RecompildPOS.Helpers.Connection;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.Sync;
using RecompildPOS.Models.Transactions;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.AccountTransactions
{
    public class AccountTransactionModule : IAccountTransactionModule
    {
        private bool _isSyncingAccountTransactions;

        public async Task SyncAccountTransactions()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingAccountTransactions)
            {
                //                "Already Syncing Users".ToToast();
                return;
            }

            _isSyncingAccountTransactions = true;

            await CheckAndPostAccountTransactions();

            DateTime date;
            var syncLog =
                await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.TransactionSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.AccountTransactionUrl, DatabaseConfig.Tables.TransactionSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            AccountTransactionSyncCollection accountTransactionSyncCollection =
                await App.RecompildPosService.AccountTransaction.GetAccountTransactions(ModulesConfig.SerialNo,
                    App.Business.Business.BusinessId,
                    date);
            if (accountTransactionSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingAccountTransactions = false;
                return;
            }

            await App.Database.AccountTransactions.AddUpdateAccountTransactionsSync(accountTransactionSyncCollection.AccountTransactions);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                accountTransactionSyncCollection.TerminalLogId.ToString(), accountTransactionSyncCollection.Count,
                ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int) ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = accountTransactionSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                accountTransactionSyncCollection.Count);

            _isSyncingAccountTransactions = false;
        }

        private async Task<bool> CheckAndPostAccountTransactions()
        {
            var unSyncedAccountTransactions = await App.Database.AccountTransactions.GetAllUnSyncedAccountTransactions();
            if (unSyncedAccountTransactions != null && unSyncedAccountTransactions.Any())
            {
                foreach (var unSyncAccountTransaction in unSyncedAccountTransactions)
                {
                    unSyncAccountTransaction.IsPending = true;
                    await App.Database.AccountTransactions.UpdateAccountTransactions(unSyncAccountTransaction);
                    bool synced = false;
                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            unSyncAccountTransaction.RequestedTime = DateTime.UtcNow;
                            var newAccountTransactionSync = new AccountTransactionSync
                            {
                                Notes = unSyncAccountTransaction.Notes,
                                Type = unSyncAccountTransaction.Type,
                                AccountId = unSyncAccountTransaction.AccountId,
                                BusinessId = unSyncAccountTransaction.BusinessId,
                                CreatedBy = unSyncAccountTransaction.CreatedBy,
                                CreatedDate = unSyncAccountTransaction.CreatedDate,
                                InvoiceNo = unSyncAccountTransaction.InvoiceNo,
                                OrderAmount = unSyncAccountTransaction.OrderAmount,
                                OrderCost = unSyncAccountTransaction.OrderCost,
                                OrderId = unSyncAccountTransaction.OrderId,
                                OrderToken = unSyncAccountTransaction.OrderToken,
                                PaidAmount = unSyncAccountTransaction.PaidAmount,
                                TotalDiscount = unSyncAccountTransaction.TotalDiscount,
                                TotalTax = unSyncAccountTransaction.TotalTax,
                                TypeName = unSyncAccountTransaction.TypeName,
                                UserId = unSyncAccountTransaction.UserId,
                                ClosingAccountBalance = unSyncAccountTransaction.ClosingAccountBalance,
                                OpeningAccountBalance = unSyncAccountTransaction.OpeningAccountBalance,
                                LastModifiedBy = unSyncAccountTransaction.LastModifiedBy,
                                LastModifiedDate = unSyncAccountTransaction.LastModifiedDate,
                                OrderProcessId = unSyncAccountTransaction.OrderProcessId,
                                AccountPaymentModeId = unSyncAccountTransaction.AccountPaymentModeId,
                                AccountTransactionId = unSyncAccountTransaction.AccountTransactionId
                            };
                            var accountTransactionSyncCollection = new AccountTransactionSyncCollection()
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                TerminalLogId = unSyncAccountTransaction.TerminalLogId,
                                BusinessId = unSyncAccountTransaction.BusinessId,
                                AccountTransactions = new List<AccountTransactionSync> {newAccountTransactionSync},
                                Count = 1
                            };
                            var isPostTransaction =
                                await App.RecompildPosService.AccountTransaction.PostAccountTransaction(
                                    accountTransactionSyncCollection);
                            unSyncAccountTransaction.ResponseTime = DateTime.UtcNow;
                            if (isPostTransaction)
                            {
                                synced = true;
                                unSyncAccountTransaction.ErrorCode = (int) HttpStatusCode.OK;
                                AppResources.ACCOUNT_TRANSACTION_MODULE_ACCOUNT_TRANSACTIONS_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncAccountTransaction.ErrorCode = (int) HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncAccountTransaction.IsPending = false;
                    unSyncAccountTransaction.IsSynced = synced;
                    await App.Database.AccountTransactions.UpdateAccountTransactions(unSyncAccountTransaction);
                }

                return true;
            }

            return false;
        }

    }
}