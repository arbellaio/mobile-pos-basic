using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.OrderProcess
{
    public class OrderProcessService : BasicService, IOrderProcessService
    {
        public RecompildPOSService Client { get; private set; }
        public OrderProcessService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<OrderProcessSyncCollection> GetOrderProcesses(string serialNo, int businessId,
            DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.OrderProcessesUrl +
                $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var orderProcessSyncCollection = await CallApi<OrderProcessSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return orderProcessSyncCollection;
        }

        public async Task<bool> PostOrderProcesses(OrderProcessSyncCollection orderProcessSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostOrderProcessesUrl).ToString();

            var isOrderProcessCollectionPosted = await CallApi<bool, OrderProcessSyncCollection>(new Uri(url), Client,
                BasicService.MethodType.POST, orderProcessSyncCollection);
            return isOrderProcessCollectionPosted;
        }
    }

    public interface IOrderProcessService
    {
        Task<OrderProcessSyncCollection> GetOrderProcesses(string serialNo, int businessId, DateTime requestedDateTime);
        Task<bool> PostOrderProcesses(OrderProcessSyncCollection orderProcessSyncCollection);
    }
}