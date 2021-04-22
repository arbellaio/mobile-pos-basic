using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Businesses;
using RecompildPOS.ViewModels.Business;
using RecompildPOS.Views.Base;
using RecompildPOS.Views.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Business
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusinessPage : BasePage
    {
        public BusinessViewModel ViewModel => BindingContext as BusinessViewModel;

        public BusinessPage(BusinessSync business = null)
        {
            InitializeComponent();
            ViewModel.IsBusy = true;
            if (business == null)
                business = new BusinessSync();

            ViewModel.Business = business;
            ViewModel.IsBusy = false;

        }
    }
}