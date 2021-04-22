using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.Accounts
{
    public interface IAccountService
    {
        Task<AccountSyncCollection> GetAccounts(string serialNo, int businessId, DateTime requestedDateTime);
        Task<bool> PostAccounts(AccountSyncCollection accountSyncCollection);
    }

    public class AccountService : BasicService, IAccountService
    {
        public RecompildPOSService Client { get; private set; }
        public AccountService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<AccountSyncCollection> GetAccounts(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.AccountsUrl + $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var accountSyncCollection = await CallApi<AccountSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return accountSyncCollection;
        }

        public async Task<bool> PostAccounts(AccountSyncCollection accountSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostAccountsUrl).ToString();

            var isAccountsCollectionPosted = await CallApi<bool, AccountSyncCollection>(new Uri(url), Client, BasicService.MethodType.POST, accountSyncCollection);
            return isAccountsCollectionPosted;
        }
    }
}
