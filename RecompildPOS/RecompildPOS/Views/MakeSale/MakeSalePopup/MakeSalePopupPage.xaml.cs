using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.ViewModels.MakeSale.MakeSalePopup;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.MakeSale.MakeSalePopup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MakeSalePopupPage : PopupPage
    {
        public MakeSalePopupViewModel ViewModel => BindingContext as MakeSalePopupViewModel;
        public MakeSalePopupPage()
        {
            InitializeComponent();
        }
    }
}