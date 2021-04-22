using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Helpers.Alert;
using RecompildPOS.ViewModels.MakeSale.MakeSaleScan.CameraScan;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace RecompildPOS.Views.CameraScan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraScanPage : ZXingScannerPage
    {
        public CameraScanViewModel ViewModel => BindingContext as CameraScanViewModel;
        public CameraScanPage()
        {
            InitializeComponent();
        }

        private void Handle_OnScanResult(Result result)
        {
            if (result != null)
            { 
                ViewModel.ScanCompleted(result);
            }
        }
    }
}