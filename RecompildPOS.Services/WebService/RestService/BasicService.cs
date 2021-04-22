using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;

namespace RecompildPOS.Services.WebService.RestService
{
    public class BasicService
    {
        public async Task<T> CallApi<T, U>(Uri Url, RecompildPOSService Client, MethodType methodType, U RequestContent)
        {
            try
            {
                HttpRequestMessage _httpRequest = new HttpRequestMessage();
                HttpResponseMessage _httpResponse = null;
                _httpRequest.Method = new HttpMethod(methodType == MethodType.GET ? "GET" : "POST");
                _httpRequest.RequestUri = Url;

                string _requestContent = null;
                if (RequestContent != null)
                {
                    _requestContent = JsonConvert.SerializeObject(RequestContent);
                    _httpRequest.Content = new StringContent(_requestContent, Encoding.UTF8);
                    _httpRequest.Content.Headers.ContentType =
                        MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                }
                _httpResponse = await Client.HttpClient.SendAsync(_httpRequest).ConfigureAwait(false);
                string responseContent = null;
                responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent(this.GetType().Name + " Exception: " + ex.Message);
                return default(T);
            }
        }

        public enum MethodType
        {
            GET,
            POST
        }
    }
}