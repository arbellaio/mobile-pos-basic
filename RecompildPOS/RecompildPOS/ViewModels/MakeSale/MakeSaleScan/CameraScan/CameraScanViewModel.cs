using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Helpers.Alert;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using Xamarin.Forms;
using ZXing;

namespace RecompildPOS.ViewModels.MakeSale.MakeSaleScan.CameraScan
{
    public class CameraScanViewModel : BaseViewModel
    {
        public Action<string> scannedCode { get; set; }
        private string _scannedCode;
        public string ScannedCode
        {
            get { return _scannedCode; }
            set
            {
                _scannedCode = value;
                OnPropertyChanged(nameof(ScannedCode));
            }
        }

        public async void ScanCompleted(Result result)
        {
            ScannedCode = result.Text;
            scannedCode?.Invoke(_scannedCode);
            Device.BeginInvokeOnMainThread((async () => { await Alert.ShowAlert(result.Text); }));
            await App.NavigationService.PopAsync();
        }
    }
}
