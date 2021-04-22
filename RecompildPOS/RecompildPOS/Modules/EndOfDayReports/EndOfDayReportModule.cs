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
using RecompildPOS.Models.EndOfDayReports;
using RecompildPOS.Models.Finances;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.Sync;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.EndOfDayReports
{
    public class EndOfDayReportModule : IEndOfDayReportModule
    {
        private bool _isSyncingEndOfDayReports;

        #region Sync End Of Day Report Methods

        public async Task SyncEndOfDayReports()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingEndOfDayReports)
            {
                //                "Already Syncing Users".ToToast();
                return;
            }

            _isSyncingEndOfDayReports = true;

            await CheckAndPostEndOfDayReports();

            DateTime date;
            var syncLog =
                await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.EndOfDayReportSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.EndOfDayReportUrl, DatabaseConfig.Tables.EndOfDayReportSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestUrl = WebServiceConfig.EndOfDayReportUrl;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            EndOfDayReportSyncCollection endOfDayReportSyncCollection =
                await App.RecompildPosService.EndOfDayDayReport.GetEndOfDayReports(ModulesConfig.SerialNo,
                    App.Business.Business.BusinessId,
                    date);
            if (endOfDayReportSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingEndOfDayReports = false;
                return;
            }

            await App.Database.EndOfDayReports.AddUpdateEndOfDayReports(endOfDayReportSyncCollection.EndOfDayReports);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                endOfDayReportSyncCollection.TerminalLogId.ToString(), endOfDayReportSyncCollection.Count,
                ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int) ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = endOfDayReportSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                endOfDayReportSyncCollection.Count);

            _isSyncingEndOfDayReports = false;
        }

       
        #endregion


        #region End Of Day Reports Database SyncLog Methods

        private async Task<bool> CheckAndPostEndOfDayReports()
        {
            var allUnSyncedEndOfDayReports =
                await App.Database.EndOfDayReports.GetAllUnSyncedEndOfDayReports();

            if (allUnSyncedEndOfDayReports != null && allUnSyncedEndOfDayReports.Any())
            {
                foreach (var report in allUnSyncedEndOfDayReports)
                {
                    report.IsPending = true;
                    await App.Database.EndOfDayReports.UpdateEndOfDayReport(report);
                    bool synced = false;

                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            report.RequestedTime = DateTime.UtcNow;
                            var dataMapper = new DataMappingHelper<EndOfDayReportSync, EndOfDayReport>();
                            var newReportSync = dataMapper.MapModel(report);
                            var endOfDayReportSyncCollection = new EndOfDayReportSyncCollection()
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                TerminalLogId = report.TerminalLogId,
                                BusinessId = report.BusinessId,
                                EndOfDayReports = new List<EndOfDayReportSync> {newReportSync},
                                Count = 1
                            };
                            var isPostEndOfDayReports =
                                await App.RecompildPosService.EndOfDayDayReport.PostEndOfDayReports(
                                    endOfDayReportSyncCollection);
                            report.ResponseTime = DateTime.UtcNow;
                            if (isPostEndOfDayReports)
                            {
                                synced = true;
                                report.ErrorCode = (int) HttpStatusCode.OK;
                                report.LastSynced = DateTime.UtcNow;
                                AppResources.END_OF_DAY_REPORT_MODULE_END_OF_DAY_REPORT_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                report.ErrorCode = (int) HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    report.IsPending = false;
                    report.IsSynced = synced;
                    await App.Database.EndOfDayReports.UpdateEndOfDayReport(report);
                }

                return true;
            }

            return false;
        }

      

      

        #endregion

    }
}