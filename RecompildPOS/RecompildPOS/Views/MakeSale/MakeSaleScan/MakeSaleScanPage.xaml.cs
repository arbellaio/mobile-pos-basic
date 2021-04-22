using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.ViewModels.MakeSale;
using RecompildPOS.ViewModels.MakeSale.MakeSaleScan;
using RecompildPOS.Views.Base;
using RecompildPOS.Views.CameraScan;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.MakeSale.MakeSaleScan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MakeSaleScanPage : BasePage
    {
        public MakeSaleScanViewModel ViewModel => BindingContext as MakeSaleScanViewModel;
        public MakeSaleScanPage()
        {
            InitializeComponent();
            ScanEntry.Focus();
        }

        
    }
}