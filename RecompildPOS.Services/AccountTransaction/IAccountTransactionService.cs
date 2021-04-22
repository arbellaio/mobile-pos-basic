using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.AccountTransaction
{
    public interface IAccountTransactionService
    {
        Task<AccountTransactionSyncCollection> GetAccountTransactions(string serialNo, int businessId,
            DateTime requestedDateTime);

        Task<bool> PostAccountTransaction(AccountTransactionSyncCollection accountTransactionSyncCollection);
    }

    public class AccountTransactionService : BasicService, IAccountTransactionService
    {
        public RecompildPOSService Client { get; private set; }
        public AccountTransactionService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<AccountTransactionSyncCollection> GetAccountTransactions(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.AccountTransactionUrl + $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var accountTransactionSyncCollection = await CallApi<AccountTransactionSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return accountTransactionSyncCollection;
        }

        public async Task<bool> PostAccountTransaction(AccountTransactionSyncCollection accountTransactionSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostAccountTransactionUrl).ToString();

            var isAccountTransactionCollectionPosted = await CallApi<bool, AccountTransactionSyncCollection>(new Uri(url), Client, BasicService.MethodType.POST, accountTransactionSyncCollection);
            return isAccountTransactionCollectionPosted;
        }
    }
}
