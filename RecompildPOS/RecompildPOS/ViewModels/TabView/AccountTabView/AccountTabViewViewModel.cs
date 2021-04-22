using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using RecompildPOS.Helpers.Alert;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Helpers.DummyData;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Resources.Language;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.MakeSale.MakeSalePopup;
using RecompildPOS.Views.MakeSale.MakeSaleScan;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.TabView.AccountTabView
{
    public class AccountTabViewViewModel : BaseViewModel
    {
        public AccountTabViewViewModel()
        {
            IsBusy = true;
            Accounts = DummyDataGenerator.GetAllAccount();
            IsBusy = false;
        }
        public ICommand CallAccountCommand => new Command<string>(CallAccountCommandLocker.Execute);
        protected CommandLockerHelper CallAccountCommandLocker => new CommandLockerHelper(CallAccount);

        public ICommand MakeSaleCommand => new Command<string>(MakeSaleCommandLocker.Execute);
        protected CommandLockerHelper MakeSaleCommandLocker => new CommandLockerHelper(OpenMakeSalePopup);

      
        public ICommand SearchCommand => new Command<string>(SearchAccount);

        private ObservableCollection<AccountSync> _accounts;
        public ObservableCollection<AccountSync> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                SearchAccount(_searchText);
            }
        }

        private AccountSync _selectedAccount;
        public AccountSync SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }


        private async void OpenMakeSalePopup()
        {
//            await App.NavigationService.PushAsync(new MakeSaleScanPage());
            await App.NavigationService.PushPopupAsync(new MakeSalePopupPage());
        }

        private async void CallAccount(object number)
        {
            var phoneNumber = (string)number;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                try
                {
                    PhoneDialer.Open(phoneNumber);
                }
                catch (ArgumentNullException ex)
                {
                    // Number was null or white space
                    await Alert.ShowAlert(AppResources.ALERT_HEADING_WARNING, "Phone Number incorrect.");
                    Debug.WriteLine(ex.Message);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Phone Dialer is not supported on this device.
                    await Alert.ShowAlert(AppResources.ALERT_HEADING_WARNING, "Calling not supported on device.");
                    Debug.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    // Other error has occurred.
                }
            }
        }

        private void SearchAccount(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                var accounts = new List<AccountSync>(Accounts);
                Accounts.Clear();
                var filterAccounts = accounts.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList();
                if (filterAccounts.Any())
                {
                    foreach (var account in filterAccounts)
                    {
                        Accounts.Add(account);
                    }
                }
                else
                    Accounts.Clear();
            }
            else
            {
                Accounts = DummyDataGenerator.GetAllAccount();
            }
        }
   
    }
}
