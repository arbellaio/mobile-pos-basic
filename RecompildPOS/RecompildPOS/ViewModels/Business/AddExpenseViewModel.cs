using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Models.Expense;
using RecompildPOS.Models.Sync;
using RecompildPOS.Modules;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Business
{
    public class AddExpenseViewModel : BaseViewModel
    {
        public ICommand SaveBusinessExpenseCommand => new Command(SaveBusinessExpenseCommandLocker.Execute);
        protected CommandLockerHelper SaveBusinessExpenseCommandLocker => new CommandLockerHelper(async () =>
        {
            await SaveBusinessExpense();
        });


        private BusinessExpenseSync _businessExpense;
        public BusinessExpenseSync BusinessExpense
        {
            get { return _businessExpense; }
            set
            {
                _businessExpense = value; 
                OnPropertyChanged(nameof(BusinessExpense));
            }
        }


        private async Task SaveBusinessExpense()
        {
            if (BusinessExpense != null && !string.IsNullOrEmpty(BusinessExpense.ExpenseName) && BusinessExpense.ExpenseAmount > 0)
            {
                BusinessExpense.BusinessId = App.Business.Business.BusinessId;
                BusinessExpense.BusinessName = App.Business.Business.Name;
//                var businessExpenseRequest = JsonConvert.SerializeObject(BusinessExpense);
//                var syncLog = new SyncLog
//                {
//                    CreatedBy = App.Users.User.Name,
//                    CreatedDate = DateTime.Today.Date,
//                    IsPending = true,
//                    Request = businessExpenseRequest,
//                    Synced = false,
//                    IsPost = true,
//                    TableName = Database.DatabaseConfig.Tables.BusinessExpense.ToString(),
//                    SerialNo = ModulesConfig.SerialNo,
//                };
                await App.Database.BusinessExpenses.AddUpdateBusinessExpense(BusinessExpense);
            }
        }
    }
}
