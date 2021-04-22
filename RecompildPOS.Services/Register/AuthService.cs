using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.ServicesModels.Register;
using RecompildPOS.Models.Users;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.Register
{
    public class AuthService : BasicService, IAuthService
    {
        public RecompildPOSService Client { get; private set; }

        public AuthService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<RegisterBusinessSync> RegisterUser(RegisterBusinessSync request)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.RegisterUrl).ToString();

            var registerResponse = await CallApi<RegisterBusinessSync, RegisterBusinessSync>(new Uri(url), Client,
                BasicService.MethodType.POST, request);
            return registerResponse;
        }

        public async Task<RegisterBusinessSync> Login(string username, string password)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.LoginUrl+ $"?username={username}&password={password}").ToString();

            var registerResponse = await CallApi<RegisterBusinessSync, String>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return registerResponse;
        }
    }

    public interface IAuthService
    {
        Task<RegisterBusinessSync> RegisterUser(RegisterBusinessSync request);
        Task<RegisterBusinessSync> Login(string username, string password);
    }
}