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
using RecompildPOS.Models.Products;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.Sync;
using RecompildPOS.Resources.Language;
using RecompildPOS.Services;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.Products
{
    public class ProductsModule : IProductsModule
    {
        private bool _isSyncingProducts;
        #region Sync Products

        public async Task SyncProducts()
        {
            if (!CrossConnectivity.Current.IsConnected || !await ConnectionHelper.IsConnected())
            {
                if (!CrossConnectivity.Current.IsConnected)
                    AppResources.ALERT_NO_INTERNET.ToToast();
                return;
            }

            if (_isSyncingProducts)
            {
                //                "Already Syncing Users".ToToast();
                return;
            }

            _isSyncingProducts = true;

            await CheckAndPostProducts();

            DateTime date;
            var syncLog = await App.Database.SyncLog.GetSyncLogByTableName(DatabaseConfig.Tables.ProductSync.ToString());

            if (syncLog != null && syncLog.RequestedTime != DateTime.MinValue)
                date = syncLog.RequestedTime;
            else
                date = ModulesConfig.SyncDate;

            string serialNo = ModulesConfig.SerialNo;

            if (syncLog == null)
                syncLog = await App.Base.InitializeSyncLog(WebServiceConfig.ProductsUrl, DatabaseConfig.Tables.ProductSync.ToString());

            //Update Sync Log before sending request
            syncLog.SerialNo = serialNo;
            syncLog.RequestUrl = WebServiceConfig.ProductsUrl;
            syncLog.RequestedTime = DateTime.UtcNow;

            //Service Call
            ProductsSyncCollection productsSyncCollection =
                await App.RecompildPosService.Products.GetProducts(ModulesConfig.SerialNo, App.Business.Business.BusinessId,
                    date);
            if (productsSyncCollection == null)
            {
                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                _isSyncingProducts = false;
                return;
            }

            await App.Database.Products.AddUpdateProducts(productsSyncCollection.Products);

            HttpResponseMessage ackResponse = await App.RecompildPosService.Acknowledgement.VerifyAckAsync(
                productsSyncCollection.TerminalLogId.ToString(), productsSyncCollection.Count, ModulesConfig.SerialNo);

            string terminalLogId = "";
            int errorCode = (int)ackResponse.StatusCode;
            bool isSynced = false;
            if (errorCode == 200)
            {
                terminalLogId = productsSyncCollection.TerminalLogId.ToString();
                isSynced = true;
                await App.Database.SyncLog.UpdateSyncLogItem(syncLog);
            }

            await App.Base.UpdateSyncLogAfterRequest(syncLog, terminalLogId, errorCode, isSynced,
                productsSyncCollection.Count);

            _isSyncingProducts = false;
        }

       

        #endregion

        #region Database Product SyncLog Methods

        private async Task<bool> CheckAndPostProducts()
        {
            var allUnSyncedProducts =
                await App.Database.Products.GetAllUnSyncedProducts();

            if (allUnSyncedProducts != null && allUnSyncedProducts.Any())
            {
                foreach (var unSyncProduct in allUnSyncedProducts)
                {
                    unSyncProduct.IsPending = true;
                    await App.Database.Products.UpdateProduct(unSyncProduct);
                    bool synced = false;

                    try
                    {
                        if (CrossConnectivity.Current.IsConnected && await ConnectionHelper.IsConnected())
                        {
                            unSyncProduct.RequestedTime = DateTime.UtcNow;
                            var dataMapper = new DataMappingHelper<ProductSync,Product>();
                            var newProduct = dataMapper.MapModel(unSyncProduct);
                            var productsSyncCollection = new ProductsSyncCollection
                            {
                                SerialNo = ModulesConfig.SerialNo,
                                TerminalLogId = unSyncProduct.TerminalLogId,
                                BusinessId = unSyncProduct.BusinessId,
                                Products = new List<ProductSync> { newProduct },
                                Count = 1
                            };
                            var isPostProducts = await App.RecompildPosService.Products.PostProducts(productsSyncCollection);
                            unSyncProduct.ResponseTime = DateTime.UtcNow;
                            if (isPostProducts)
                            {
                                synced = true;
                                unSyncProduct.ErrorCode = (int)HttpStatusCode.OK;
                                unSyncProduct.LastSynced = DateTime.UtcNow;
                                AppResources.PRODUCTS_MODULE_PRODUCTS_POSTED.ToToast();
                            }
                            else
                            {
                                synced = false;
                                unSyncProduct.ErrorCode = (int)HttpStatusCode.BadRequest;
                                AppResources.ALERT_SOMETHING_WENT_WRONG.ToToast();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Analytics.TrackEvent(this.GetType().Name + " Exception: " + e.Message);
                    }

                    unSyncProduct.IsPending = false;
                    unSyncProduct.IsSynced = synced;
                    await App.Database.Products.UpdateProduct(unSyncProduct);
                }

                return true;
            }

            return false;
        }

        
        
        #endregion
      
    }
}