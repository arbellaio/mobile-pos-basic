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
using RecompildPOS.Helpers.MappingHelper;
using RecompildPOS.Models.Expense;
using RecompildPOS.Models.Finances;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.Sync;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.BusinessFinances
{
    public class BusinessFinanceModule : IBusinessFinanceModule
    {
        private bool _isSyncingBusinessFinances;

        #region Business Finances Sync Methods

        public async Task SyncBusinessFinances()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingBusinessFinances)
            {
                //                "Already Syncing Users".ToToast();
                return;
            }

            _isSyncingBusinessFinances = true;

            await CheckAndPostBusinessFinances();

            DateTime date;
            var syncLog = await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.BusinessFinanceSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.BusinessFinancesUrl, DatabaseConfig.Tables.BusinessFinanceSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestUrl = WebServiceConfig.BusinessFinancesUrl;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            BusinessFinanceSyncCollection businessFinanceSyncCollection =
                await App.RecompildPosService.BusinessFinance.GetBusinessFinances(ModulesConfig.SerialNo, App.Business.Business.BusinessId,
                    date);
            if (businessFinanceSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingBusinessFinances = false;
                return;
            }

            await App.Database.BusinessFinances.AddUpdateBusinessFinances(businessFinanceSyncCollection.BusinessFinances);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                businessFinanceSyncCollection.TerminalLogId.ToString(), businessFinanceSyncCollection.Count, ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int)ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = businessFinanceSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                businessFinanceSyncCollection.Count);

            _isSyncingBusinessFinances = false;
        }

        #endregion


        #region Business Finance Database SyncLog Methods

        private async Task<bool> CheckAndPostBusinessFinances()
        {
            var allUnSyncedBusinessFinances = await App.Database.BusinessFinances.GetAllUnSyncedBusinessFinances();
            if (allUnSyncedBusinessFinances != null && allUnSyncedBusinessFinances.Any())
            {
                foreach (var unSyncedBusinessFinance in allUnSyncedBusinessFinances)
                {
                    unSyncedBusinessFinance.IsPending = true;
                    await App.Database.BusinessFinances.UpdateBusinessFinance(unSyncedBusinessFinance);
                    bool synced = false;

                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            unSyncedBusinessFinance.RequestedTime = DateTime.UtcNow;
                            
                            var dataMapper = new DataMappingHelper<BusinessFinanceSync, BusinessFinance>();
                            var newBusinessFinanceSync = dataMapper.MapModel(unSyncedBusinessFinance);
                           
                            var businessFinanceSyncCollection = new BusinessFinanceSyncCollection()
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                TerminalLogId = unSyncedBusinessFinance.TerminalLogId,
                                BusinessId = unSyncedBusinessFinance.BusinessId,
                                BusinessFinances = new List<BusinessFinanceSync> { newBusinessFinanceSync },
                                Count = 1
                            };
                            var isPostProducts = await App.RecompildPosService.BusinessFinance.PostBusinessFinances(businessFinanceSyncCollection);
                            unSyncedBusinessFinance.ResponseTime = DateTime.UtcNow;
                            if (isPostProducts)
                            {
                                synced = true;
                                unSyncedBusinessFinance.ErrorCode = (int)HttpStatusCode.OK;
                                unSyncedBusinessFinance.LastSynced = DateTime.UtcNow;
                                AppResources.BUSINESS_FINANCES_MODULE_BUSINESS_FINANCES_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncedBusinessFinance.ErrorCode = (int)HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncedBusinessFinance.IsPending = false;
                    unSyncedBusinessFinance.IsSynced = synced;
                    await App.Database.BusinessFinances.UpdateBusinessFinance(unSyncedBusinessFinance);
                }

                return true;
            }

            return false;
        }

       


#endregion

    }
}
