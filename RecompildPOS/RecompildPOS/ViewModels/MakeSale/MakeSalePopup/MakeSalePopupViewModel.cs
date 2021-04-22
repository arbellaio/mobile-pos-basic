using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Products;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;

namespace RecompildPOS.ViewModels.MakeSale.MakeSalePopup
{
    public class MakeSalePopupViewModel : BaseViewModel
    {
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

        public async Task GetProducts()
        {
           var productsInDb = await App.Database.Products.GetAllProducts();
           if (productsInDb != null)
           {
               Products = _products;
           }
        }
    }
}
