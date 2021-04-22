using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Views.Login;
using RecompildPOS.Views.Sync;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.MasterTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMenuPage : TabbedPage
    {
        public TabbedMenuPage()
        {
            InitializeComponent();
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            await App.NavigationService.PushAsync(new SyncPage());
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage(); 
        }
    }
}