using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Models.Products;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.Products.AddProducts;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Products
{
    public class ProductsViewModel : BaseViewModel
    {
        public ICommand AddNewProductCommand => new Command(AddNewProductCommandLocker.Execute);
        protected CommandLockerHelper AddNewProductCommandLocker => new CommandLockerHelper(async () =>
        {
            await AddNewProduct();
        });

        private ObservableCollection<ProductSync> _products;
        public ObservableCollection<ProductSync> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }


        public async Task GetAllProducts()
        {
            var productsInDb = await App.Database.Products.GetAllProducts();
            if (productsInDb != null && productsInDb.Any())
            {
                Products = new ObservableCollection<ProductSync>(productsInDb);
            }
        }

        private async Task AddNewProduct()
        {
            await App.NavigationService.PushAsync(new AddProductPage());
        }
    }
}
