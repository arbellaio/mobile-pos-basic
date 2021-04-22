using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.ViewModels.Business;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Business.BusinessFinance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusinessFinancePage : BasePage
    {
        public BusinessFinanceViewModel ViewModel => BindingContext as BusinessFinanceViewModel;

        public BusinessFinancePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.IsBusy = true;
            ViewModel.IsBusy = false;
        }
    }
}