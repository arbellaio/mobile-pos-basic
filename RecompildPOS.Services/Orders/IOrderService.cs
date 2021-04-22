using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderSyncCollection> GetOrders(string serialNo, int businessId, DateTime requestedDateTime);
        Task<bool> PostOrders(OrderSyncCollection orderSyncCollection);
    }

    public class OrderService : BasicService, IOrderService
    {
        public RecompildPOSService Client { get; private set; }
        public OrderService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<OrderSyncCollection> GetOrders(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.OrdersUrl + $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var orderSyncCollection = await CallApi<OrderSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return orderSyncCollection;
        }

        public async Task<bool> PostOrders(OrderSyncCollection orderSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostOrdersUrl).ToString();

            var isOrdersCollectionPosted = await CallApi<bool, OrderSyncCollection>(new Uri(url), Client, BasicService.MethodType.POST, orderSyncCollection);
            return isOrdersCollectionPosted;
        }
    }
}
