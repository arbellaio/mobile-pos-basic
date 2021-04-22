using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Plugin.Connectivity;
using RecompildPOS.Database;
using RecompildPOS.Extensions;
using RecompildPOS.Helpers;
using RecompildPOS.Helpers.Connection;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.ServicesModels.Register;
using RecompildPOS.Models.Sync;
using RecompildPOS.Models.Users;
using RecompildPOS.Resources.Keys;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;
using Xamarin.Essentials;

namespace RecompildPOS.Modules.Businesses
{
    public class BusinessModule : IBusinessModule
    {
        private bool _isSyncingBusinesses;
        public BusinessSync Business { get; set; }

        #region Sync Businesses

        public async Task SyncBusinessesModule()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingBusinesses)
            {
//                "Already Syncing Businesses".ToToast();
                Analytics.TrackEvent("Already Syncing Businesses : " + this.GetType().Name +
                                     App.Business.Business.BusinessId);
                return;
            }

            _isSyncingBusinesses = true;
            // if (Preferences.ContainsKey(AppKeys.IsNewRegister) && Preferences.Get(AppKeys.IsNewRegister, false))
            // {
            //     await CheckToPostRegister();
            //     Preferences.Set(AppKeys.IsNewRegister, false);
            // }

            await CheckToPostBusiness();

            DateTime date;
            var syncLog = await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.BusinessSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.BusinessesUrl,
                    DatabaseConfig.Tables.BusinessSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            BusinessSyncCollection businessesSyncCollection =
                await App.RecompildPosService.Business.GetBusinesses(ModulesConfig.SerialNo,App.Business.Business.BusinessId, date);
            if (businessesSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingBusinesses = false;
                return;
            }

            await App.Database.Businesses.AddUpdateBusinesses(businessesSyncCollection.Businesses);
            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                businessesSyncCollection.TerminalLogId.ToString(), businessesSyncCollection.Count,
                ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int) ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = businessesSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                businessesSyncCollection.Count);

            _isSyncingBusinesses = false;
        }

        #endregion

        #region Database SyncLog Methods

        private async Task CheckToPostBusiness()
        {
            var unSyncedBusinessInformation = await App.Database.Businesses.GetAllUnSyncedBusinessInformation();
            if (unSyncedBusinessInformation != null)
            {
                foreach (var unSyncedBusiness in unSyncedBusinessInformation)
                {
                    bool synced = false;
                    unSyncedBusiness.IsPending = true;

                    await App.Database.Businesses.UpdateBusiness(unSyncedBusiness);
                    
                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            unSyncedBusiness.RequestedTime = DateTime.UtcNow;
                            var newBusiness = new BusinessSync
                            {
                                Category =  unSyncedBusiness.Category,
                                Name = unSyncedBusiness.Category,
                                Owner = unSyncedBusiness.Owner,
                                Type = unSyncedBusiness.Type,
                                CategoryId = unSyncedBusiness.CategoryId,
                                CreatedBy = unSyncedBusiness.CreatedBy,
                                CreatedDate = unSyncedBusiness.CreatedDate,
                                LicenseNumber = unSyncedBusiness.LicenseNumber,
                                TypeId = unSyncedBusiness.TypeId,
                                LastModifiedBy = unSyncedBusiness.LastModifiedBy,
                                LastModifiedDate = unSyncedBusiness.LastModifiedDate,
                                BusinessOwnerUserId = unSyncedBusiness.BusinessOwnerUserId,
                                BusinessId = unSyncedBusiness.BusinessId,
                            };
                            var businessSyncCollection = new BusinessSyncCollection
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                TerminalLogId = unSyncedBusiness.TerminalLogId,
                                BusinessId = unSyncedBusiness.BusinessId,
                                Businesses = new List<BusinessSync> {newBusiness},
                                Count = 1
                            };
                            var isBusinessesPosted =
                                await App.RecompildPosService.Business.PostBusinesses(businessSyncCollection);

                            unSyncedBusiness.ResponseTime = DateTime.UtcNow;

                            if (isBusinessesPosted)
                            {
                                synced = true;
                                unSyncedBusiness.ErrorCode = (int) HttpStatusCode.OK;
                                unSyncedBusiness.LastSynced = DateTime.UtcNow;
                                AppResources.BUSINESS_MODULE_BUSINESSES_UPDATED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncedBusiness.ErrorCode = (int) HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncedBusiness.IsPending = false;
                    unSyncedBusiness.IsSynced = synced;
                    await App.Database.Businesses.UpdateBusiness(unSyncedBusiness);
                }
            }
        }
        #endregion
        
        
        // public async Task CheckToPostRegister()
        // {
        //     var syncLogBusiness =
        //         await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.Business.ToString(), true,
        //             false);
        //     var syncLogUser =
        //         await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.Users.ToString(), true, false);
        //     if (syncLogUser != null && syncLogBusiness != null)
        //     {
        //         bool synced = false;
        //
        //         syncLogUser.IsPending = true;
        //         syncLogBusiness.IsPending = true;
        //         await App.Database.SyncLog.UpdateSyncLogItem(syncLogUser);
        //         await App.Database.SyncLog.UpdateSyncLogItem(syncLogBusiness);
        //         try
        //         {
        //             var businessToSync = JsonConvert.DeserializeObject<BusinessSync>(syncLogBusiness.Request);
        //             var userToSync = JsonConvert.DeserializeObject<UserSync>(syncLogUser.Request);
        //             var registerBusinessSync = new RegisterBusinessSync
        //             {
        //                 Business = businessToSync,
        //                 User = userToSync,
        //                 SerialNo = ModulesConfig.SerialNo,
        //                 RequestDate = DateTime.UtcNow,
        //             };
        //
        //             if (CrossConnectivity.Current.IsConnected)
        //             {
        //                 var registerBusiness =
        //                     await App.RecompildPosService.Auth.RegisterUser(registerBusinessSync);
        //                 if (registerBusiness != null && registerBusiness.Business != null &&
        //                     registerBusiness.User != null)
        //                 {
        //                     await App.Database.Users.AddUpdateUsers(registerBusiness.User);
        //                     await AddUpdateBusiness(registerBusiness.Business);
        //                     synced = true;
        //                     AppResources.BUSINESS_MODULE_BUSINESS_REGISTERED_TEXT.ToToast();
        //                 }
        //                 else
        //                 {
        //                     synced = false;
        //                     AppResources.BUSINESS_MODULE_BUSINESS_REGISTRATION_ISSUE_TEXT.ToToast();
        //                 }
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
        //         }
        //
        //         syncLogUser.IsPending = false;
        //         syncLogBusiness.IsPending = false;
        //         syncLogUser.Synced = synced;
        //         syncLogBusiness.Synced = synced;
        //         await App.Database.SyncLog.UpdateSyncLogItem(syncLogUser);
        //         await App.Database.SyncLog.UpdateSyncLogItem(syncLogBusiness);
        //     }
    }

    
}

