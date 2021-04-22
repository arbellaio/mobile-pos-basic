using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.ServicesModels;
using RecompildPOS.Services.WebService;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.EndOfDayReport
{
    public interface IEndOfDayReportService
    {
        Task<EndOfDayReportSyncCollection> GetEndOfDayReports(string serialNo, int businessId,
            DateTime requestedDateTime);

        Task<bool> PostEndOfDayReports(EndOfDayReportSyncCollection endOfDayReportSyncCollection);
    }

    public class EndOfDayReportService : BasicService, IEndOfDayReportService
    {
        public RecompildPOSService Client { get; private set; }
        public EndOfDayReportService(RecompildPOSService client)
        {
            if (client == null)
                throw new ArgumentNullException("Client");
            this.Client = client;
        }

        public async Task<EndOfDayReportSyncCollection> GetEndOfDayReports(string serialNo, int businessId, DateTime requestedDateTime)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.EndOfDayReportUrl +
                $"?serialNo={serialNo}&{businessId}&requestedDateTime={requestedDateTime}").ToString();

            var endOfDayReportSyncCollection= await CallApi<EndOfDayReportSyncCollection, string>(new Uri(url), Client,
                BasicService.MethodType.GET, null);
            return endOfDayReportSyncCollection;
        }

        public async Task<bool> PostEndOfDayReports(EndOfDayReportSyncCollection endOfDayReportSyncCollection)
        {
            var baseUrl = this.Client.BaseUri.AbsoluteUri;

            var url = new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/", StringComparison.Ordinal) ? "" : "/")),
                WebServiceConfig.PostEndOfDayReportUrl).ToString();

            var isEndOfDayReportCollectionPosted = await CallApi<bool, EndOfDayReportSyncCollection>(new Uri(url), Client,
                BasicService.MethodType.POST, endOfDayReportSyncCollection);
            return isEndOfDayReportCollectionPosted;
        }
    }
}
