using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Resources.Constants;
using RecompildPOS.ViewModels.Products.GenerateCode;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace RecompildPOS.Views.Products.GenerateCode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenerateBarcodePage : BasePage
    {
        public GenerateCodeViewModel ViewModel => BindingContext as GenerateCodeViewModel;
        public GenerateBarcodePage(string code, string format)
        {
            InitializeComponent();
            if (format.Equals(AppConstants.BarCode))
            {
                BarcodeImageView.BarcodeFormat = BarcodeFormat.CODE_128;
            }
            if(format.Equals(AppConstants.QrCode))
            {
                BarcodeImageView.BarcodeFormat = BarcodeFormat.QR_CODE;
            }

            ViewModel.CodeValue = code;
            BarcodeImageView.BarcodeValue = code;
        }


        private void SaveCode(object sender, EventArgs e)
        {
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }

        }
    }
}