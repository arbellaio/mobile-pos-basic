using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Selectable;
using RecompildPOS.ViewModels.Accounts;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Account.PhoneBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhoneBookContactsPage : BasePage
    {
        public PhoneBookContactsViewModel ViewModel => BindingContext as PhoneBookContactsViewModel;

        public PhoneBookContactsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.GetPhoneBookContacts();
        }

        private void ContactList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectionChangedCommand?.Execute(e.SelectedItem as SelectableItem<AccountSync>);
            ((ListView) sender).SelectedItem = null;
        }
    }
}