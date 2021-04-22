using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.Products
{
    public class ProductService : BasicService, IProductService
    {
        public RecompildPOSService Client { get; private set; }
        public ProductService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<ProductsSyncCollection> GetProducts(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.ProductsUrl + $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var productSyncCollection = await CallApi<ProductsSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return productSyncCollection;
        }

        public async Task<bool> PostProducts(ProductsSyncCollection productsSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostProductsUrl).ToString();

            var isProductsCollectionPosted = await CallApi<bool, ProductsSyncCollection>(new Uri(url), Client, BasicService.MethodType.POST, productsSyncCollection);
            return isProductsCollectionPosted;
        }
    }

    public interface IProductService
    {
        Task<ProductsSyncCollection> GetProducts(string serialNo, int businessId, DateTime requestedDateTime);
        Task<bool> PostProducts(ProductsSyncCollection productsSyncCollection);
    }
}
