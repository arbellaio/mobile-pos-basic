using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Helpers;
using RecompildPOS.ViewModels.Login;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BasePage
    {
        public LoginViewModel ViewModel => BindingContext as LoginViewModel;
        public LoginPage()
        {
            InitializeComponent();
        }
    }
}