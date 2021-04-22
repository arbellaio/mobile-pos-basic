using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.Users;
using RecompildPOS.ViewModels.Register;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : BasePage
    {
        public RegisterViewModel ViewModel => BindingContext as RegisterViewModel;
        public RegisterPage()
        {
            InitializeComponent();
            ViewModel.Business = new BusinessSync();
            ViewModel.User = new UserSync();
        }
    }
}