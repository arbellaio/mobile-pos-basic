using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.Account;
using RecompildPOS.Views.Business;
using RecompildPOS.Views.Business.BusinessFinance;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand AccountsCommand => new Command(AccountsCommandLocker.Execute);
        protected CommandLockerHelper AccountsCommandLocker => new CommandLockerHelper(async () =>
        {
           await GotoAccountsPage();
        });

        public ICommand BusinessFinancesCommand => new Command(BusinessFinancesCommandLocker.Execute);
        protected CommandLockerHelper BusinessFinancesCommandLocker => new CommandLockerHelper(async () =>
        {
            await GoToBusinessFinancePage();
        });

//        public ICommand EndOfDayReportCommand => new Command(EndOfDayReportCommandLocker.Execute);
//        protected CommandLockerHelper EndOfDayReportCommandLocker => new CommandLockerHelper(GotoAccountsPage);
//
//        public ICommand ProductsCommand => new Command(ProductsCommandLocker.Execute);
//        protected CommandLockerHelper ProductsCommandLocker => new CommandLockerHelper(GotoAccountsPage);
//
//        public ICommand FinancialTransactionsCommand => new Command(FinancialTransactionsCommandLocker.Execute);
//        protected CommandLockerHelper FinancialTransactionsCommandLocker => new CommandLockerHelper(GotoAccountsPage);

        public ICommand BusinessCommand => new Command(BusinessCommandLocker.Execute);
        protected CommandLockerHelper BusinessCommandLocker => new CommandLockerHelper(async () =>
        {
            await GotoBusinessPage();
        });



        private async Task GotoAccountsPage()
        { 
          await App.NavigationService.PushAsync(new AccountPage());
        }

        private async Task GotoBusinessPage()
        {
            await App.NavigationService.PushAsync(new BusinessPage(App.Business.Business));
        }

        private async Task GoToBusinessFinancePage()
        {
           await App.NavigationService.PushAsync(new BusinessFinancePage());
        }

    }
}
