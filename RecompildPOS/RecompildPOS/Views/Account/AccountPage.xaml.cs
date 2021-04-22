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

namespace RecompildPOS.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : BasePage
    {
        public AccountViewModel ViewModel => BindingContext as AccountViewModel;

        public AccountPage()
        {
            InitializeComponent();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectedAccount = e.SelectedItem as AccountSync;
            ViewModel.EditAccountCommand?.Execute(ViewModel.SelectedAccount);
            ((ListView)sender).SelectedItem = null;
        }
    }
}