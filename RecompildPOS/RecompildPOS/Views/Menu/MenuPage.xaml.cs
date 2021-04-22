using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.ViewModels.Menu;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : BasePage
    {
        public MenuViewModel ViewModel => BindingContext as MenuViewModel;

        public MenuPage()
        {
            InitializeComponent();
        }

        void Init()
        {
            
        }
    }
}