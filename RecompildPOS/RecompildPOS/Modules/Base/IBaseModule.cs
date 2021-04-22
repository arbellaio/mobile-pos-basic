using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Sync;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.Base
{
    public interface IBaseModule
    {
        Task<SyncLog> AddSyncLogBeforeRequest(string tableName, string url, bool isPost, string request);

        Task UpdateSyncLogAfterRequest(SyncLog syncLog, string terminalLogId, int errorCode, bool synced,
            int resultCount);

        Task<SyncLog> InitializeSyncLog(string requestUrl, string tableName, bool isPost = false);
    }

    public class BaseModule : IBaseModule
    {
        public async Task<SyncLog> AddSyncLogBeforeRequest(string tableName, string url, bool isPost, string request)
        {
            SyncLog syncLog = await InitializeSyncLog(url, tableName, isPost);
            var date = ModulesConfig.SyncDate;
            string serialNo = ModulesConfig.SerialNo;
            //Add Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.Request = request;
            syncLog.IsPost = isPost;
            syncLog.IsPending = true;
            await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            return syncLog;
        }

        public async Task UpdateSyncLogAfterRequest(SyncLog syncLog, string terminalLogId, int errorCode, bool synced, int resultCount)
        {
            syncLog.TerminalLogId = terminalLogId;
            syncLog.ErrorCode = errorCode;
            syncLog.IsPending = false;
            syncLog.Synced = synced;
            syncLog.ResponseTime = DateTime.UtcNow;

            if (errorCode == 200)
            {
                syncLog.ResultCount = resultCount;
                syncLog.ErrorCode = 0;
            }

            await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
        }

        public async Task<SyncLog> InitializeSyncLog(string requestUrl, string tableName, bool isPost = false)
        {

            SyncLog syncLog = await App.Database.SyncLog.GetSyncLogByTableName(tableName); ;
            if (isPost || syncLog == null)
            {

                syncLog = new SyncLog();
                syncLog.TableName = tableName;
                syncLog.RequestUrl = requestUrl;
                syncLog.RequestedTime = DateTime.UtcNow;
                await App.Database.SyncLog.AddSyncLogItem(syncLog);
            }

            return syncLog;
        }
    }
}