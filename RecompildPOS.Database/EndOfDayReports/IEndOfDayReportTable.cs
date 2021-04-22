using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.EndOfDayReports;

namespace RecompildPOS.Database.EndOfDayReports
{
    public class EndOfDayReportTable : IEndOfDayReportTable
    {
        public LocalDatabase Handler { get; private set; }

        public EndOfDayReportTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateEndOfDayReports(List<EndOfDayReportSync> endOfDayReports)
        {
            if (endOfDayReports != null && endOfDayReports.Any())
            {
                foreach (var endOfDayReport in endOfDayReports)
                {
                    await AddUpdateEndOfDayReport(endOfDayReport);
                }
            }
        }
        public async Task AddUpdateEndOfDayReport(EndOfDayReportSync endOfDayReport)
        {
            if (endOfDayReport != null)
            {
                var endOfDayInDb = await GetEndOfDayReportById(endOfDayReport.EndOfDayReportId);
                if (endOfDayInDb == null)
                {
                    if (!endOfDayReport.IsDeleted)
                    {
                        await Handler.Database.InsertAsync(endOfDayReport);
                    }
                }
                else
                {
                    if (endOfDayReport.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(endOfDayReport);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(endOfDayReport);
                    }
                }
            }
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportById(int id)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.EndOfDayReportId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportByOrderToken(string orderToken)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.OrderToken.Equals(orderToken)).FirstOrDefaultAsync();
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportByOrderTransactionLogId(string transactionLogId)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.TransactionLogId.Equals(transactionLogId)).FirstOrDefaultAsync();
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportByUserId(int userId, int businessId)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.UserId.Equals(userId) && x.BusinessId.Equals(businessId)).FirstOrDefaultAsync();
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportByUserId(int userId)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportOfSpecificDay(DateTime dateTime, int businessId)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.SubmittedDate.Equals(dateTime) && x.BusinessId.Equals(businessId)).FirstOrDefaultAsync();
        }

        public async Task<EndOfDayReportSync> GetEndOfDayReportOfSpecificDay(DateTime dateTime)
        {
            return await Handler.Database.Table<EndOfDayReportSync>().Where(x => x.SubmittedDate.Equals(dateTime)).FirstOrDefaultAsync();
        }

        public async Task<List<EndOfDayReport>> GetAllUnSyncedEndOfDayReports()
        {
            return await Handler.Database.Table<EndOfDayReport>().Where(x => x.IsPost && !x.IsSynced).ToListAsync();
        }

        public async Task UpdateEndOfDayReport(EndOfDayReport report)
        {
            if (report != null)
            {
                await Handler.Database.UpdateAsync(report);
            }
        }
    }

    public interface IEndOfDayReportTable
    {
        Task AddUpdateEndOfDayReports(List<EndOfDayReportSync> endOfDayReports);
        Task AddUpdateEndOfDayReport(EndOfDayReportSync endOfDayReport);
        Task<EndOfDayReportSync> GetEndOfDayReportById(int id);
        Task<EndOfDayReportSync> GetEndOfDayReportByOrderToken(string orderToken);
        Task<EndOfDayReportSync> GetEndOfDayReportByOrderTransactionLogId(string transactionLogId);
        Task<EndOfDayReportSync> GetEndOfDayReportByUserId(int userId, int businessId);
        Task<EndOfDayReportSync> GetEndOfDayReportOfSpecificDay(DateTime dateTime, int businessId);
        Task<EndOfDayReportSync> GetEndOfDayReportOfSpecificDay(DateTime dateTime);
        Task<EndOfDayReportSync> GetEndOfDayReportByUserId(int userId);

        Task<List<EndOfDayReport>> GetAllUnSyncedEndOfDayReports();
        Task UpdateEndOfDayReport(EndOfDayReport report);
    }
}