using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Plugin.Connectivity;
using RecompildPOS.Database;
using RecompildPOS.Extensions;
using RecompildPOS.Helpers;
using RecompildPOS.Helpers.Connection;
using RecompildPOS.Helpers.MappingHelper;
using RecompildPOS.Models.Products;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.Sync;
using RecompildPOS.Models.Users;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.Users
{
    public interface IUserModule
    {
        int LoggedInUserId { get; set; }
        UserSync User { get; set; }
        Task<bool> Login(string username, string password);
        Task SyncUsersModule();
    }

    public class UserModule : IUserModule
    {
        public int LoggedInUserId { get; set; }
        public UserSync User { get; set; }

        private bool _isSyncingUsers;

        #region Sync Users

        public async Task SyncUsersModule()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingUsers)
            {
                //                "Already Syncing Users".ToToast();
                return;
            }

            _isSyncingUsers = true;

            await CheckAndPostUsers();

            DateTime date;
            var syncLog = await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.UserSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.UsersUrl, DatabaseConfig.Tables.UserSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestUrl = WebServiceConfig.UsersUrl;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            UserSyncCollection usersSyncCollection =
                await App.RecompildPosService.User.GetUsers(ModulesConfig.SerialNo, App.Business.Business.BusinessId,
                    date);
            if (usersSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingUsers = false;
                return;
            }

            await App.Database.Users.AddUpdateUsers(usersSyncCollection.Users);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                usersSyncCollection.TerminalLogId.ToString(), usersSyncCollection.Count, ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int) ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)

            {
                terminalLogId = usersSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                usersSyncCollection.Count);

            _isSyncingUsers = false;
        }

        #endregion


        #region Database SyncLog Methods

        public async Task<bool> CheckAndPostUsers()
        {
            var allUnSyncedUsers = await App.Database.Users.GetAllUnSyncedUsers();
               

            if (allUnSyncedUsers != null && allUnSyncedUsers.Any())
            {
                foreach (var unSyncUser in allUnSyncedUsers)
                {
                    unSyncUser.IsPending = true;
                    await App.Database.Users.UpdateUser(unSyncUser);
                    bool synced = false;

                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            unSyncUser.RequestedTime = DateTime.UtcNow;
                            var dataMapper = new DataMappingHelper<UserSync,User>();
                            var newUser = dataMapper.MapModel(unSyncUser);
                            var userSyncCollection = new UserSyncCollection
                            {
                                TerminalLogId = unSyncUser.TerminalLogId,
                                BusinessId = unSyncUser.BusinessId,
                                SerialNo = ModulesConfig.SerialNo,
                                Users = new List<UserSync> { newUser },
                                Count = 1
                            };

                            var isUsersPosted = await App.RecompildPosService.User.PostUsers(userSyncCollection);
                            unSyncUser.ResponseTime = DateTime.UtcNow;
                            if (isUsersPosted)
                            {
                                synced = true;
                                unSyncUser.ErrorCode = (int)HttpStatusCode.OK;
                                unSyncUser.LastSynced = DateTime.UtcNow;
                                AppResources.USERS_MODULE_USER_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncUser.ErrorCode = (int)HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncUser.IsPending = false;
                    unSyncUser.IsSynced = synced;
                    await App.Database.Users.UpdateUser(unSyncUser);
                }

                return true;
            }

            return false;
        }

        #endregion


        #region Authentication

        public async Task<bool> Login(string username, string password)
        {
            var user = await App.Database.Users.LoginUser(username, password);
            if (user != null)
            {
                LoggedInUserId = user.UserId;
                User = user;
                var business = await App.Database.Businesses.GetBusinessByBusinessId(user.BusinessId);
                if (business != null)
                    App.Business.Business = business;

                return true;
            }
            else
                return false;
        }

        #endregion
       
    }
}