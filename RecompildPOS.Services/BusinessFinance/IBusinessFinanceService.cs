using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.BusinessFinance
{
    public interface IBusinessFinanceService
    {
        Task<BusinessFinanceSyncCollection> GetBusinessFinances(string serialNo, int businessId,
            DateTime requestedDateTime);
        Task<BusinessExpenseSyncCollection> GetBusinessExpenses(string serialNo, int businessId,
            DateTime requestedDateTime);

        Task<bool> PostBusinessFinances(BusinessFinanceSyncCollection businessFinanceSyncCollection);
        Task<bool> PostBusinessExpenses(BusinessExpenseSyncCollection businessExpenseSyncCollection);
    }

    public class BusinessFinanceService : BasicService, IBusinessFinanceService
    {
        public RecompildPOSService Client { get; private set; }
        public BusinessFinanceService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<BusinessFinanceSyncCollection> GetBusinessFinances(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.BusinessFinancesUrl +
                $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var businessFinanceSyncCollection = await CallApi<BusinessFinanceSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return businessFinanceSyncCollection;
        }

        public async Task<BusinessExpenseSyncCollection> GetBusinessExpenses(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.BusinessExpensesUrl +
                $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var businessExpenseSyncCollection = await CallApi<BusinessExpenseSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return businessExpenseSyncCollection;
        }

        public async Task<bool> PostBusinessFinances(BusinessFinanceSyncCollection businessFinanceSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostBusinessFinancesUrl).ToString();

            var isBusinessFinanceCollectionPosted = await CallApi<bool, BusinessFinanceSyncCollection>(new Uri(url), Client,
                BasicService.MethodType.POST, businessFinanceSyncCollection);
            return isBusinessFinanceCollectionPosted;
        }

        public async Task<bool> PostBusinessExpenses(BusinessExpenseSyncCollection businessExpenseSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostBusinessExpensesUrl).ToString();

            var isBusinessExpenseCollectionPosted = await CallApi<bool, BusinessExpenseSyncCollection>(new Uri(url), Client,
                BasicService.MethodType.POST, businessExpenseSyncCollection);
            return isBusinessExpenseCollectionPosted;
        }
    }
}
