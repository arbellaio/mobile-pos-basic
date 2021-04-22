using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.CameraScan;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.MakeSale.MakeSaleScan
{
    public class MakeSaleScanViewModel : BaseViewModel
    {
        public ICommand GoToCameraScanPageCommand => new Command(GoToCameraScanPageCommandLocker.Execute);
        private CommandLockerHelper GoToCameraScanPageCommandLocker =>
            new CommandLockerHelper(async () => { await GoToCameraScanPage(); });

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value; 
                OnPropertyChanged(nameof(Code));
            }
        }


        private async Task GoToCameraScanPage()
        {
            var cameraScanPage = new CameraScanPage();
            await App.NavigationService.PushAsync(cameraScanPage);
            cameraScanPage.ViewModel.scannedCode += ScannedCode;
        }

        private void ScannedCode(string code)
        {
            Code = code;
        }
    }
}
