using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Plugin.Connectivity;
using RecompildPOS.Database;
using RecompildPOS.Extensions;
using RecompildPOS.Helpers.Connection;
using RecompildPOS.Helpers.MappingHelper;
using RecompildPOS.Models.Expense;
using RecompildPOS.Models.Finances;
using RecompildPOS.Models.Products;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.Sync;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.BusinessFinances
{
    public interface IBusinessFinanceExpenseModule
    {
        Task SyncBusinessExpenses();
    }

    public class BusinessFinanceExpenseModule : IBusinessFinanceExpenseModule
    {

        private bool _isSyncingBusinessExpenses;
        
            
        #region Sync Business Expenses Methods


        public async Task SyncBusinessExpenses()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingBusinessExpenses)
            {
                //                "Already Syncing Users".ToToast();
                return;
            }

            _isSyncingBusinessExpenses = true;

            await CheckAndPostBusinessExpenses();

            DateTime date;
            var syncLog = await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.BusinessExpenseSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.BusinessExpensesUrl,DatabaseConfig.Tables.BusinessExpenseSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestUrl = WebServiceConfig.BusinessExpensesUrl;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            BusinessExpenseSyncCollection businessExpenseSyncCollection =
                await App.RecompildPosService.BusinessFinance.GetBusinessExpenses(ModulesConfig.SerialNo, App.Business.Business.BusinessId,
                    date);
            if (businessExpenseSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingBusinessExpenses = false;
                return;
            }

            await App.Database.BusinessExpenses.AddUpdateBusinessExpenses(businessExpenseSyncCollection.BusinessExpenses);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                businessExpenseSyncCollection.TerminalLogId.ToString(), businessExpenseSyncCollection.Count, ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int)ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = businessExpenseSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                businessExpenseSyncCollection.Count);

            _isSyncingBusinessExpenses = false;
        }

        #endregion


        #region Database Business Expenses SyncLog Methods

        private async Task<bool> CheckAndPostBusinessExpenses()
        {
            var allUnSyncedBusinessExpenses =
                await App.Database.BusinessExpenses.GetAllUnSyncedBusinessExpenses();

            if (allUnSyncedBusinessExpenses != null && allUnSyncedBusinessExpenses.Any())
            {
                foreach (var unSyncedBusinessExpense in allUnSyncedBusinessExpenses)
                {
                    unSyncedBusinessExpense.IsPending = true;
                    await App.Database.BusinessExpenses.UpdateBusinessExpense(unSyncedBusinessExpense);
                    bool synced = false;

                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            unSyncedBusinessExpense.RequestedTime = DateTime.UtcNow;
                            var dataMapper = new DataMappingHelper<BusinessExpenseSync,BusinessExpense>();
                            var newBusinessExpense = dataMapper.MapModel(unSyncedBusinessExpense);
                            var businessExpenseSyncCollection = new BusinessExpenseSyncCollection()
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                TerminalLogId = unSyncedBusinessExpense.TerminalLogId,
                                BusinessId = unSyncedBusinessExpense.BusinessId,
                                BusinessExpenses = new List<BusinessExpenseSync> { newBusinessExpense },
                                Count = 1
                            };
                            var isPostProducts = await App.RecompildPosService.BusinessFinance.PostBusinessExpenses(businessExpenseSyncCollection);
                            unSyncedBusinessExpense.ResponseTime = DateTime.UtcNow;
                            if (isPostProducts)
                            {
                                synced = true;
                                unSyncedBusinessExpense.ErrorCode = (int)HttpStatusCode.OK;
                                unSyncedBusinessExpense.LastSynced = DateTime.UtcNow;
                                AppResources.BUSINESS_EXPENSES_MODULE_BUSINESS_EXPENSES_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncedBusinessExpense.ErrorCode = (int)HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncedBusinessExpense.IsPending = false;
                    unSyncedBusinessExpense.IsSynced = synced;
                    await App.Database.BusinessExpenses.UpdateBusinessExpense(unSyncedBusinessExpense);
                }

                return true;
            }

            return false;
        }

        #endregion

    }
}
