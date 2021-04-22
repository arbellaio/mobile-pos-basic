using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.ViewModels.Business;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Business.BusinessFinance.AddExpense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExpensePage : BasePage
    {
        public AddExpenseViewModel ViewModel =>  BindingContext as AddExpenseViewModel;
        public AddExpensePage()
        {
            InitializeComponent();
        }
    }
}