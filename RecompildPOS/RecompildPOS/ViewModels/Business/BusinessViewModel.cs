using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Models.Businesses;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Business
{
    public class BusinessViewModel : BaseViewModel
    {
        public ICommand SaveBusinessInfoCommand => new Command(SaveBusinessInfoCommandLocker.Execute);
        protected CommandLockerHelper SaveBusinessInfoCommandLocker => new CommandLockerHelper(async () =>
        {
            await SaveBusinessProfile();
        });

       
        private BusinessSync _business;
		public BusinessSync Business
		{
			get { return _business; }
            set
            {
                _business = value;
                OnPropertyChanged(nameof(Business));
            }
		}


        private async Task<bool> SaveBusinessProfile()
        {
            if (Business != null)
            {
                await App.Database.Businesses.AddUpdateBusiness(Business);
                return true;
            }

            return false;
        }

    }
}
