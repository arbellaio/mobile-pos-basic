﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
 using Plugin.Connectivity;
using RecompildPOS.Database;
using RecompildPOS.Extensions;
using RecompildPOS.Helpers.Connection;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.ServicesModels;
 using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

 namespace RecompildPOS.Modules.Accounts
{
    public class AccountModule : IAccountModule
    {
        private bool _isSyncingAccounts;

        public async Task SyncAccounts()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingAccounts)
            {
                //                "Already Syncing Accounts".ToToast();
                return;
            }

            _isSyncingAccounts = true;

            await CheckAndPostAccounts();

            DateTime date;
            var syncLog = await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.AccountSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.AccountsUrl, DatabaseConfig.Tables.AccountSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestUrl = WebServiceConfig.AccountsUrl;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            AccountSyncCollection accountSyncCollection =
                await App.RecompildPosService.Accounts.GetAccounts(ModulesConfig.SerialNo,
                    App.Business.Business.BusinessId,
                    date);
            if (accountSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingAccounts = false;
                return;
            }

            await App.Database.Accounts.AddOrUpdateAccountsSync(accountSyncCollection.Accounts);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                accountSyncCollection.TerminalLogId.ToString(), accountSyncCollection.Count, ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int) ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = accountSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                accountSyncCollection.Count);

            _isSyncingAccounts = false;
        }

        private async Task<bool> CheckAndPostAccounts()
        {
            var unSyncedAccounts = await App.Database.Accounts.GetAllUnSyncedAccounts();

            if (unSyncedAccounts != null && unSyncedAccounts.Any())
            {
                foreach (var unSyncedAccount in unSyncedAccounts)
                {
                    unSyncedAccount.IsPending = true;
                    await App.Database.Accounts.UpdateAccount(unSyncedAccount);
                    bool synced = false;
                    try
                    {
                        unSyncedAccount.RequestedTime = DateTime.UtcNow;
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                             var newAccountSync = new AccountSync
                             {
                                 Name = unSyncedAccount.Name,
                                 BusinessId = unSyncedAccount.BusinessId,
                                 AccountCode = unSyncedAccount.AccountCode,
                                 Address = unSyncedAccount.Address,
                                 Balance = unSyncedAccount.Balance,
                                 CreatedBy = unSyncedAccount.CreatedBy,
                                 CreatedDate = unSyncedAccount.CreatedDate,
                                 CreditLimit = unSyncedAccount.CreditLimit,
                                 Email = unSyncedAccount.Email,
                                 PhoneNumber = unSyncedAccount.PhoneNumber,
                                 LastModifiedBy = unSyncedAccount.LastModifiedBy,
                                 LastModifiedDate = unSyncedAccount.LastModifiedDate,
                                 AccountId = unSyncedAccount.AccountId
                             };
                            var accountSyncCollection = new AccountSyncCollection
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                Accounts = new List<AccountSync> { newAccountSync },
                                TerminalLogId = unSyncedAccount.TerminalLogId,
                                BusinessId = unSyncedAccount.BusinessId,
                                Count = 1
                            };

                            var isAccountsPosted =
                                await App.RecompildPosService.Accounts.PostAccounts(accountSyncCollection);
                            unSyncedAccount.ResponseTime = DateTime.UtcNow;

                            if (isAccountsPosted)
                            {
                                synced = true;
                                unSyncedAccount.ErrorCode = (int) HttpStatusCode.OK;
                                AppResources.ACCOUNTS_MODULE_ACCOUNTS_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncedAccount.ErrorCode = (int) HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncedAccount.IsPending = false;
                    unSyncedAccount.IsSynced = synced;
                    await App.Database.Accounts.UpdateAccount(unSyncedAccount);
                }

                return true;
            }

            return false;
        }


    }
}