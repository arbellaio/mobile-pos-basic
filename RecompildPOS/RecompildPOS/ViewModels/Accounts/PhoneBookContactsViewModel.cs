using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.Alert;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Helpers.ContactsHelper;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Selectable;
using RecompildPOS.Resources.Language;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using PermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace RecompildPOS.ViewModels.Accounts
{
    public class PhoneBookContactsViewModel : BaseViewModel
    {

        #region Commands
        public ICommand SaveContactsCommand => new Command(SaveContactsCommandLocker.Execute);
        protected CommandLockerHelper SaveContactsCommandLocker => new CommandLockerHelper(async () => await SaveContacts());

        public ICommand SearchCommand => new Command<string>(SearchContacts);

        public ICommand SelectionChangedCommand => new Command<SelectableItem<Account>>((selectedAccountObj) =>
        {
            var selectableItem = selectedAccountObj;
            if (selectableItem != null && !SelectedContacts.Contains(selectableItem.Item))
            {
                selectedAccountObj.IsSelected = true;
                SelectedContacts.Add(selectableItem.Item);
            }
            else
            {
                if (selectedAccountObj != null)
                {
                    selectedAccountObj.IsSelected = false;
                    SelectedContacts.Remove(selectableItem?.Item);
                }
            }
        });


        #endregion


        #region Properties
        private string _searchContact;
        public string SearchContact
        {
            get { return _searchContact; }
            set
            {
                _searchContact = value;
                OnPropertyChanged(nameof(SearchContact));
                SearchContacts(_searchContact);
            }
        }

        private ObservableCollection<SelectableItem<Account>> _contacts;
        public ObservableCollection<SelectableItem<Account>> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }

        private ObservableCollection<Account> _selectedContacts = new ObservableCollection<Account>();
        public ObservableCollection<Account> SelectedContacts
        {
            get { return _selectedContacts; }
            set
            {
                _selectedContacts = value;
                OnPropertyChanged(nameof(SelectedContacts));
            }
        }


        #endregion





        /// <summary>
        /// Gets Contacts from Phone_Book with permission
        /// </summary>
        /// <returns></returns>
        public async Task GetPhoneBookContacts()
        {
           
                var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.ContactsRead>();
                }

                if (status == PermissionStatus.Granted)
                {
                    Contacts = new ObservableCollection<SelectableItem<Account>>();
                    //Query permission
                    var contacts = await DependencyService.Get<IContactHelper>().GetDeviceContactsAsync();
                    foreach (var contact in contacts)
                    {
                        var selectedAccount = new SelectableItem<Account>(contact);
                        _contacts.Add(selectedAccount);
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await Alert.ShowAlert(AppResources.ALERT_HEADING_WARNING,"Without permission can't access contacts.");
                }
        }


        /// <summary>
        /// Searches Contacts Retrieved from Phone_Book
        /// </summary>
        /// <param name="searchText"></param>
        private async void SearchContacts(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                var accounts = new List<SelectableItem<Account>>(Contacts);
                Contacts.Clear();
                var filterAccounts = accounts.Where(x => x.Item.Name.ToLower().Contains(searchText.ToLower())).ToList();
                if (filterAccounts.Any())
                {
                    foreach (var account in filterAccounts)
                    {
                        Contacts.Add(account);
                    }
                }
                else
                    Contacts.Clear();
            }
            else
            {
                await GetPhoneBookContacts();
            }
        }


        private async Task<bool> SaveContacts()
        {
            if (SelectedContacts != null && SelectedContacts.Any())
            {
                var selectedContactList = SelectedContacts.ToList();
                await App.Database.Accounts.AddUpdateAccountFromContacts(selectedContactList);
                return true;
            }

            return false;
        }
    }
}
