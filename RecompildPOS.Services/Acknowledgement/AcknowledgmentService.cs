using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RecompildPOS.Services.WebService;

namespace RecompildPOS.Services.Acknowledgement
{
    public class AcknowledgmentService : IAcknowledgementService
    {
        public RecompildPOSService Client { get; private set; }
        public AcknowledgmentService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }
        public async Task<HttpResponseMessage> VerifyAckAsync(string terminalLogId, int count, string serialNo)
        {
            try
            {
                var baseUrl = this.Client.BaseUri.AbsoluteUri;
                var _url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")), WebServiceConfig.VerifyAck).ToString();
                List<string> queryParameters = new List<string>();
                if (!string.IsNullOrEmpty(terminalLogId))
                {
                    queryParameters.Add($"id={Uri.EscapeDataString(terminalLogId)}");
                }
                queryParameters.Add($"count={Uri.EscapeDataString(JsonConvert.SerializeObject(count).Trim('"'))}");

                if (!string.IsNullOrEmpty(serialNo))
                {
                    queryParameters.Add($"serialNo={Uri.EscapeDataString(serialNo)}");
                }
                if (queryParameters.Count > 0)
                {
                    _url += "?" + string.Join("&", queryParameters);
                }
                HttpRequestMessage httpRequest = new HttpRequestMessage();
                HttpResponseMessage httpResponse = null;
                httpRequest.Method = new HttpMethod("GET");
                httpRequest.RequestUri = new Uri(_url);

                httpResponse = await this.Client.HttpClient.SendAsync(httpRequest).ConfigureAwait(false);
                return httpResponse;
            }
            catch
            {
                return null;
            }
        }
    }

    public interface IAcknowledgementService
    {
        Task<HttpResponseMessage> VerifyAckAsync(string terminalLogId, int count, string serialNo);
    }
}
