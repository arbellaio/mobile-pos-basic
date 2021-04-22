using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.ServicesModels.Register;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.Business
{
    public class BusinessService : BasicService, IBusinessService
    {
        public RecompildPOSService Client { get; private set; }
        public BusinessService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }
        
        public async Task<BusinessSyncCollection> GetBusinesses(string serialNo,int businessId, DateTime dateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.BusinessesUrl + $"?serialNo={serialNo}&businessId={businessId}&requestedDateTime={dateTime}").ToString();

            var businessSyncCollection = await CallApi<BusinessSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return businessSyncCollection;
        }

        public async Task<bool> PostBusinesses(BusinessSyncCollection businessSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostBusinessesUrl).ToString();

            var isbusinessCollectionPosted = await CallApi<bool, BusinessSyncCollection>(new Uri(url), Client, BasicService.MethodType.POST, businessSyncCollection);
            return isbusinessCollectionPosted;
        }
    }

    public interface IBusinessService
    {
        Task<BusinessSyncCollection> GetBusinesses(string serialNo,int businessId, DateTime dateTime);
        Task<bool> PostBusinesses(BusinessSyncCollection businessSyncCollection);
    }
}
