using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Models.ServicesModels.Register;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.ServerPing
{
    public class ServerPingService : BasicService, IServerPingService
    {
        public RecompildPOSService Client { get; private set; }

        public ServerPingService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<ServerStatusEnum> CheckPortConnection(string serialNo)
        {
            try
            {
                var _baseUrl = Client.BaseUri.AbsoluteUri;
                var _url = new Uri(new Uri(_baseUrl + (_baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                    WebServiceConfig.CheckServerConnectionUrl + $"?serialNo={serialNo}").ToString();

                var _httpResponse = await CallApi<HttpResponseMessage, string>(new Uri(_url), Client,
                    BasicService.MethodType.GET, null);
                if (_httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ServerStatusEnum.Ok;
                }
                else if (_httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ServerStatusEnum.Unauthorized;
                }

                return ServerStatusEnum.TimeOut;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return ServerStatusEnum.TimeOut;
            }
        }
    }

    public enum ServerStatusEnum
    {
        Ok = 1,
        TimeOut = 2,
        Unauthorized = 3
    }

    public interface IServerPingService
    {
        Task<ServerStatusEnum> CheckPortConnection(string serialNo);
    }
}