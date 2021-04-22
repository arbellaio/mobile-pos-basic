using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Helpers.MappingHelper;
using RecompildPOS.Models.Products;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.Products.GenerateCode;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Products
{
    public class AddProductViewModel : BaseViewModel
    {

        public string Code { get; set; }
        public string CodeFormat { get; set; }
        public ICommand GenerateCodeCommand =>  new Command(GenerateCodeCommandLocker.Execute);
        protected CommandLockerHelper GenerateCodeCommandLocker => new CommandLockerHelper(async () => 
        {
            await GoToGenerateCodePage();
        });

        public ICommand AddUpdateProductCommand => new Command(AddUpdateProductCommandLocker.Execute);
        protected CommandLockerHelper AddUpdateProductCommandLocker => new CommandLockerHelper(async () =>
        {
            await AddUpdateProduct();
        });

        public async Task GoToGenerateCodePage()
        {
            await App.NavigationService.PushAsync(new GenerateBarcodePage(Code, CodeFormat));
        }

        public async Task AddUpdateProduct()
        {
            if (_productSync != null)
            {
                var dataMapper = new DataMappingHelper<Product, ProductSync>();
                _product = dataMapper.MapModel(ProductSync);
                await App.Database.Products.AddUpdateProducts(_product);
            }
        }


        private Product _product;
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        private ProductSync _productSync;
        public ProductSync ProductSync
        {
            get => _productSync;
            set
            {
                _productSync = value;
                OnPropertyChanged(nameof(ProductSync));
            }
        }


    }
}
