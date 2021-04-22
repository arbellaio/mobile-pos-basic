using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.ViewModels.TabView.AccountTabView;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.AccountTabView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountTabViewPage : BasePage
    {
        public AccountTabViewViewModel ViewModel => BindingContext as AccountTabViewViewModel;
        public AccountTabViewPage()
        {
            InitializeComponent();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

    }
}