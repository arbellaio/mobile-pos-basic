using System;
using System.Threading.Tasks;
using Plugin.Connectivity;
using RecompildPOS.Extensions;
using RecompildPOS.Helpers.Connection;
using RecompildPOS.Helpers.NotifyProperty;
using RecompildPOS.Resources.Keys;
using RecompildPOS.Resources.Language;
using RecompildPOS.Views;
using Xamarin.Essentials;

namespace RecompildPOS.Modules.Sync
{
    public class SyncModule : NotifyPropertyChangedHelper, ISyncModule
    {
        public SyncModule()
        {
        }

        private bool isSyncing;

        public bool IsSyncing
        {
            get { return isSyncing; }
            set
            {
                isSyncing = value;
                OnPropertyChanged();
            }
        }

        public async Task SyncAllModules()
        {
            bool isSync = Preferences.Get(AppKeys.IsSyncingModules, false);
            if (CrossConnectivity.Current.IsConnected)
            {
                if (await ConnectionHelper.IsConnected())
                {
                    if (!isSync)
                    {
                        Preferences.Set(AppKeys.IsSyncingModules, true);
                        AppResources.ALERT_SYNC_STARTED.ToToast();
                        try
                        {
                            await App.Business.SyncBusinessesModule();
                            await App.Users.SyncUsersModule();
                            AppResources.ALERT_SYNC_COMPLETED.ToToast();
                            SyncDone?.Invoke();
                            Preferences.Set(AppKeys.NotFirstTime, true);
                            Preferences.Set(AppKeys.IsSyncingModules, false);
                            Preferences.Set(AppKeys.LastSyncDateTime, DateTime.UtcNow);
                        }
                        finally
                        {
                        }
                    }
                    else
                    {
                        AppResources.ALERT_SYNC_IN_PROGRESS.ToToast();
                    }
                }
                else
                {
                   AppResources.ALERT_NO_INTERNET.ToToast();
                }
            }
        }

        public Action SyncDone { get; set; }
    }
}