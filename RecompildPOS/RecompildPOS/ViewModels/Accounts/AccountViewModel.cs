using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.Alert;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Helpers.DummyData;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Resources.Language;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.Account.AddAccount;
using RecompildPOS.Views.Account.PhoneBook;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Accounts
{
    public class AccountViewModel : BaseViewModel
    {

        public AccountViewModel()
        {
            IsBusy = true;
            Accounts = DummyDataGenerator.GetAllAccount();
            IsBusy = false;
        }
        public ICommand AddFromPhoneBookCommand => new Command(AddFromPhoneBookCommandLocker.Execute);
        protected CommandLockerHelper AddFromPhoneBookCommandLocker => new CommandLockerHelper(GoToPhoneBookContactPage);

        public ICommand EditAccountCommand => new Command<AccountSync>(EditAccountCommandLocker.Execute);
        protected CommandLockerHelper EditAccountCommandLocker => new CommandLockerHelper(async () => { await GoToAddAccountPage();});

        public ICommand NewAccountCommand => new Command<AccountSync>(NewAccountCommandLocker.Execute);
        protected CommandLockerHelper NewAccountCommandLocker => new CommandLockerHelper(async () => { await GoToAddNewAccountPage(); });

        public ICommand CallAccountCommand => new Command<string>(CallAccountCommandLocker.Execute);
        protected CommandLockerHelper CallAccountCommandLocker => new CommandLockerHelper(CallAccount);

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



        private async void CallAccount(object number)
        {
            var phoneNumber = (string) number;
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


        private async void GoToPhoneBookContactPage()
        {
           await App.NavigationService.PushAsync(new PhoneBookContactsPage());
        }

        private async Task GoToAddAccountPage()
        {
            if (SelectedAccount != null)
            {
               await App.NavigationService.PushAsync(new AddAccountPage(SelectedAccount));
            }
        }

        private async Task GoToAddNewAccountPage()
        {
            await App.NavigationService.PushAsync(new AddAccountPage());
        }
    }
}