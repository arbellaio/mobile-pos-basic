using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Finances;

namespace RecompildPOS.Database.BusinessFinances
{
    public class BusinessFinancesTable : IBusinessFinancesTable
    {
        public LocalDatabase Handler { get; private set; }
        public BusinessFinancesTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateBusinessFinances(List<BusinessFinanceSync> businessFinances)
        {
            if (businessFinances != null && businessFinances.Any())
            {
                foreach (var businessFinance in businessFinances)
                {
                    await AddUpdateBusinessFinances(businessFinance);
                }
            }
        }

        public async Task AddUpdateBusinessFinances(BusinessFinanceSync businessFinance)
        {
            if (businessFinance != null)
            {
                var businessFinancesInDb = await GetBusinessFinancesById(businessFinance.BusinessFinanceId);
                if (businessFinancesInDb == null)
                {
                    if (!businessFinance.IsDeleted)
                    {
                        await Handler.Database.InsertAsync(businessFinance);
                    }
                }
                else
                {
                    if (businessFinance.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(businessFinance);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(businessFinance);
                    }
                }
            }
        }

        public async Task<BusinessFinanceSync> GetBusinessFinancesById(int id)
        {
            return await Handler.Database.Table<BusinessFinanceSync>().Where(x => x.BusinessFinanceId.Equals(id))
                .FirstOrDefaultAsync();
        }

        

        public async Task<BusinessFinanceSync> GetBusinessFinancesByBusinessId(int businessId)
        {
            return await Handler.Database.Table<BusinessFinanceSync>().Where(x => x.BusinessId.Equals(businessId))
                .FirstOrDefaultAsync();
        }

        public async Task<BusinessFinanceSync> GetBusinessFinancesByBusinessName(string businessName)
        {
            if (!string.IsNullOrEmpty(businessName))
            {
                return await Handler.Database.Table<BusinessFinanceSync>().Where(x => x.BusinessName.Equals(businessName))
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<BusinessFinance>> GetAllUnSyncedBusinessFinances()
        {
            return await Handler.Database.Table<BusinessFinance>().Where(x => x.IsPost && !x.IsSynced).ToListAsync();
        }

        public async Task UpdateBusinessFinance(BusinessFinance businessFinance)
        {
            if (businessFinance != null)
            {
                await Handler.Database.UpdateAsync(businessFinance);
            }
        }
    }

    public interface IBusinessFinancesTable
    {
        Task AddUpdateBusinessFinances(BusinessFinanceSync businessFinance);
        Task<BusinessFinanceSync> GetBusinessFinancesById(int id);
        Task<BusinessFinanceSync> GetBusinessFinancesByBusinessId(int businessId);
        Task<BusinessFinanceSync> GetBusinessFinancesByBusinessName(string businessName);
        Task AddUpdateBusinessFinances(List<BusinessFinanceSync> businessFinances);
        Task<List<BusinessFinance>> GetAllUnSyncedBusinessFinances();
        Task UpdateBusinessFinance(BusinessFinance businessFinance);
    }
}
