using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Accounts;
using RecompildPOS.ViewModels.Accounts;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Account.AddAccount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAccountPage : BasePage
    {
        private AddAccountViewModel ViewModel => BindingContext as AddAccountViewModel;

        public AddAccountPage(AccountSync selectedAccount = null)
        {
            InitializeComponent();
            if (selectedAccount == null)
            {
                ViewModel.IsNewAccount = true;
                ViewModel.Account = new AccountSync(); 
            }
            else
                ViewModel.Account = selectedAccount;
        }
    }
}