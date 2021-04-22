using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Products;
using RecompildPOS.Resources.Constants;
using RecompildPOS.ViewModels.Products;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Products.AddProducts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductPage : BasePage
    {
        public AddProductViewModel ViewModel => BindingContext as AddProductViewModel;
        public AddProductPage()
        {
            InitializeComponent();
            ViewModel.ProductSync = new ProductSync();
        }

        private void GenerateBarCode(object sender, EventArgs e)
        {
            QrCodeLabel.Opacity = 0;
            QrCodeLabel.FadeTo(1, 200);
            ViewModel.Code = ViewModel.ProductSync.SkuCode;
            ViewModel.CodeFormat = AppConstants.BarCode;
            ViewModel.GenerateCodeCommand?.Execute(null);
        }

        private void GenerateQrCode(object sender, EventArgs e)
        {
            QrCodeLabel.Opacity = 0;
            QrCodeLabel.FadeTo(1, 200);
            ViewModel.Code = ViewModel.Product.QrCode;
            ViewModel.CodeFormat = AppConstants.QrCode;
            ViewModel.GenerateCodeCommand?.Execute(null);
        }
    }
}