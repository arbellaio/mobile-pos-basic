using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.ViewModels.Products;
using RecompildPOS.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : BasePage
    {
        public ProductsViewModel ViewModel => BindingContext as ProductsViewModel;
        public ProductsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.GetAllProducts();
        }
    }
}