using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.Users
{
    public interface IUserService
    {
        Task<UserSyncCollection> GetUsers(string serialNo, int businessId, DateTime requestedDateTime);
        Task<bool> PostUsers(UserSyncCollection userSyncCollection);
    }

    public class UserService : BasicService, IUserService
    {
        public RecompildPOSService Client { get; private set; }
        public UserService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }


        public async Task<UserSyncCollection> GetUsers(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.UsersUrl + $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var userSyncCollection = await CallApi<UserSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return userSyncCollection;
        }

        public async Task<bool> PostUsers(UserSyncCollection userSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostUsersUrl).ToString();

            var isUsersCollectionPosted = await CallApi<bool, UserSyncCollection>(new Uri(url), Client, BasicService.MethodType.POST, userSyncCollection);
            return isUsersCollectionPosted;
        }
    }
}
