using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using RecompildPOS.Models.Sync;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Sync
{
    public class SyncViewModel : BaseViewModel
    {
        private ObservableCollection<SyncPageItems> syncList;
        public ObservableCollection<SyncPageItems> SyncList
        {
            get { return syncList; }
            set
            {
                syncList = value;
                OnPropertyChanged();
            }
        }

        private bool autoSync;
        public bool AutoSync
        {
            get { return autoSync; }
            set
            {
                autoSync = value;
                OnPropertyChanged();
            }
        }

        public ICommand ItemTappedCommand { get; private set; }

        public SyncViewModel()
        {
            ItemTappedCommand = new Command(ItemTapped);
            SyncList = new ObservableCollection<SyncPageItems>();
            var tables = Enum.GetNames(typeof(Database.DatabaseConfig.Tables));
            foreach (var item in tables)
            {
                SyncList.Add(new SyncPageItems() { Name = item });
            }
            AutoSync = App.AutoSync;
        }

        public void AutoSyncToggled()
        { 
            App.ToggleAutoSync(AutoSync);
        }

        private async void ItemTapped(object obj)
        {
            var item = (SyncPageItems) ((ItemTappedEventArgs) obj).Item;
            if (item.Name == Database.DatabaseConfig.Tables.UserSync.ToString())
            {
                item.IsSyncing = true;
                await App.Users.SyncUsersModule();
                item.IsSyncing = false;
            }
            else if (item.Name == Database.DatabaseConfig.Tables.AccountSync.ToString())
            {
                item.IsSyncing = true;
                await App.Accounts.SyncAccounts();
                item.IsSyncing = false;
            }
            else if (item.Name == Database.DatabaseConfig.Tables.ProductSync.ToString())
            {
                item.IsSyncing = true;
                await App.Products.SyncProducts();
                item.IsSyncing = false;
            }
            else if (item.Name == Database.DatabaseConfig.Tables.BusinessSync.ToString())
            {
                item.IsSyncing = true;
                await App.Business.SyncBusinessesModule();
                item.IsSyncing = false;
            }
        }
    }
}
