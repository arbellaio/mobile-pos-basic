using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Modules;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.Account;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Accounts
{
    public class AddAccountViewModel : BaseViewModel
    {
        public bool IsNewAccount { get; set; }
        public ICommand SaveAccountCommand => new Command(SaveAccountCommandLocker.Execute);
        private CommandLockerHelper SaveAccountCommandLocker => new CommandLockerHelper(async () => { await SaveAccountInDb(); });
        
        private AccountSync _account;
		public AccountSync Account
		{
			get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged(nameof(Account));
            }
		}
        
        private async Task<bool> SaveAccountInDb()
        {
            var account = new Account
            {
                Address = Account.Address,
                Balance = Account.Balance,
                Email = Account.Email,
                Name = Account.Name,
                AccountCode = Account.AccountCode,
                BusinessId = Account.BusinessId,
                CreatedBy = App.Users.User.Email,
                CreatedDate = DateTime.Today,
                CreditLimit = Account.CreditLimit,
                PhoneNumber = Account.PhoneNumber,
                IsPost = true,
                SerialNo = ModulesConfig.SerialNo,
                TerminalLogId = Guid.NewGuid().ToString()
            };
            await App.Database.Accounts.AddOrUpdateAccount(account);
            return true;
        }
	}
}
