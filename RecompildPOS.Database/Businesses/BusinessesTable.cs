using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Businesses;

namespace RecompildPOS.Database.Businesses
{
    public class BusinessesTable : IBusinessesTable
    {
        public LocalDatabase Handler { get; private set; }
        public BusinessesTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateBusinesses(List<BusinessSync> businesses)
        {
            if (businesses != null && businesses.Any())
            {
                foreach (var business in businesses)
                {
                    await AddUpdateBusiness(business);
                }
            }
        }
        public async Task AddUpdateBusiness(BusinessSync business)
        {
            if (business != null)
            {
                var businessInDb = await GetBusinessByBusinessId(business.BusinessId);
                if (businessInDb == null)
                {
                    if (!business.IsDeleted)
                    {
                        await Handler.Database.InsertAsync(business);
                    }
                }
                else
                {
                    if (!business.IsDeleted)
                    {
                        await Handler.Database.UpdateAsync(business);
                    }
                    else
                    {
                        await Handler.Database.DeleteAsync(business);
                    }
                }
            }
        }

        public async Task<BusinessSync> GetBusinessByLicenseNumber(string licenseNumber)
        {
           return await Handler.Database.Table<BusinessSync>().Where(x => x.LicenseNumber.Equals(licenseNumber)).FirstOrDefaultAsync();
        }

        public async Task<BusinessSync> GetBusinessByBusinessId(int businessId)
        {
            return await Handler.Database.Table<BusinessSync>().Where(x => x.BusinessId.Equals(businessId)).FirstOrDefaultAsync();
        }

        public async Task<List<BusinessSync>> GetBusinessByOwnerUserId(int userId)
        {
            return await Handler.Database.Table<BusinessSync>().Where(x => x.BusinessOwnerUserId.Equals(userId))
                .ToListAsync();
        }

        public async Task<List<BusinessSync>> GetBusinessByOwnerName(string ownerName)
        {
            if (!string.IsNullOrEmpty(ownerName))
            {
                return await Handler.Database.Table<BusinessSync>().Where(x => x.Owner.Equals(ownerName))
                    .ToListAsync();
            }

            return null;
        }

        public async Task<List<BusinessSync>> GetBusinessByBusinessName(string businessName)
        {
            if (!string.IsNullOrEmpty(businessName))
            {
                return await Handler.Database.Table<BusinessSync>().Where(x => x.Name.Equals(businessName))
                    .ToListAsync();
            }

            return null;
        }

        public async Task<List<Business>> GetAllUnSyncedBusinessInformation()
        {
            return await Handler.Database.Table<Business>().Where(x => x.IsPost && !x.IsSynced).ToListAsync();
        }

        public async Task UpdateBusiness(Business business)
        {
            if (business != null)
            {
                await Handler.Database.UpdateAsync(business);
            }
        }
    }

    public interface IBusinessesTable
    {
        Task AddUpdateBusinesses(List<BusinessSync> businesses);
        Task AddUpdateBusiness(BusinessSync business);
        Task<BusinessSync> GetBusinessByLicenseNumber(string licenseNumber);
        Task<BusinessSync> GetBusinessByBusinessId(int businessId);
        Task<List<BusinessSync>> GetBusinessByOwnerUserId(int userId);
        Task<List<BusinessSync>> GetBusinessByOwnerName(string ownerName);
        Task<List<BusinessSync>> GetBusinessByBusinessName(string businessName);
        Task<List<Business>> GetAllUnSyncedBusinessInformation();
        Task UpdateBusiness(Business business);


    }
}
