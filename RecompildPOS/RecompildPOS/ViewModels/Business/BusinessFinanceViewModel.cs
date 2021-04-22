using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Helpers.Navigation;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.Business.BusinessFinance.AddExpense;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Business
{
    public class BusinessFinanceViewModel : BaseViewModel
    {
        public ICommand GoToAddExpensePageCommand => new Command(GoToAddExpensePageCommandLocker.Execute);
        protected CommandLockerHelper GoToAddExpensePageCommandLocker => new CommandLockerHelper(async () =>
        {
            await GoToAddExpensePage();
        });

        private async Task GoToAddExpensePage()
        {
            await App.NavigationService.PushAsync(new AddExpensePage());
        }
    }
}
