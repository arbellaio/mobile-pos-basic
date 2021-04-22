using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Resources.Colors;
using RecompildPOS.Resources.Icons;
using RecompildPOS.Resources.Language;
using RecompildPOS.Views.Account;
using RecompildPOS.Views.Business;
using RecompildPOS.Views.Products;
using RecompildPOS.Views.User;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.MasterTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public List<MasterPageList> MenuList { get; set; }

        public MasterPage()
        {
            InitializeComponent();
            NavigationDrawerList.ItemsSource = PopulateNavigationDrawer();
        }

      

        private List<MasterPageList> PopulateNavigationDrawer()
        {
            return new List<MasterPageList>
            {
                new MasterPageList() { Title = AppResources.HOME_PAGE_HOME, Icon = FontAwesomeIcons.Home},
                new MasterPageList() { Title = AppResources.MENU_PAGE_BUSINESS_PROFILE, Icon = FontAwesomeIcons.Briefcase},
                new MasterPageList() { Title = AppResources.MENU_PAGE_ACCOUNTS, Icon = FontAwesomeIcons.Users},
                new MasterPageList() { Title = AppResources.MENU_PAGE_PRODUCTS, Icon = FontAwesomeIcons.BoxFull},
                new MasterPageList() { Title = AppResources.MENU_PAGE_USERS, Icon = FontAwesomeIcons.UserEdit},
                new MasterPageList() { Title = AppResources.MENU_PAGE_FINANCIAL, Icon = FontAwesomeIcons.Receipt},
                new MasterPageList() { Title = AppResources.MENU_PAGE_BUSINESS_FINANCES, Icon = FontAwesomeIcons.ChartBar},
                new MasterPageList() { Title = AppResources.MENU_PAGE_END_OF_DAY, Icon = FontAwesomeIcons.FileInvoice},
                new MasterPageList() { Title = AppResources.MENU_PAGE_SETTINGS, Icon = FontAwesomeIcons.Cogs},
                new MasterPageList() { Title = AppResources.MENU_PAGE_ABOUT, Icon = FontAwesomeIcons.AddressBook},
            };
        }

        private void ImageButton_OnClicked(object sender, EventArgs e)
        {
            this.IsPresented = !this.IsPresented;
        }

        private void OnMenuSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemIndex.Equals(0))
            {
                Detail = new TabbedMenuPage();
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(1))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(2))
            {
                Detail = new AccountPage();
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(3))
            {
                Detail = new ProductsPage();
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(4))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(5))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(6))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(7))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(8))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (e.ItemIndex.Equals(9))
            {
                Detail = new BusinessPage(App.Business.Business);
                IsPresented = false;
            }
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }

    //This class used for binding ListView. We can move it to other separate files as well   
    public class MasterPageList
    {
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}