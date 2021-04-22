using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.OrderProcesses;

namespace RecompildPOS.Database.OrderProcesses
{
    public class OrderProcessDetailTable : IOrderProcessDetailTable
    {
        public LocalDatabase Handler { get; private set; }
        public OrderProcessDetailTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }
        public async Task AddUpdateOrderProcessDetail(List<OrderProcessDetailSync> orderProcessDetails)
        {
            if (orderProcessDetails != null && orderProcessDetails.Any())
            {
                foreach (var orderProcessDetail in orderProcessDetails)
                {
                    var orderProcessDetailInDb = await GetOrderProcessDetailByOrderProcessId(orderProcessDetail.OrderProcessId);
                    if (orderProcessDetailInDb == null)
                    {
                        if (!orderProcessDetail.IsDeleted)
                        {
                            await Handler.Database.InsertAsync(orderProcessDetail);
                        }
                    }
                    else
                    {
                        if (orderProcessDetail.IsDeleted)
                        {
                            await Handler.Database.DeleteAsync(orderProcessDetail);
                        }
                        else
                        {
                            await Handler.Database.UpdateAsync(orderProcessDetail);
                        }
                    }
                }
            }
        }

        public async Task<OrderProcessDetailSync> GetOrderProcessDetailById(int id)
        { 
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.OrderProcessDetailId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByOrderProcessId(int orderProcessId)
        {
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.OrderProcessId.Equals(orderProcessId)).ToListAsync();
        }

        public async Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByOrderId(int orderId)
        {
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.OrderId.Equals(orderId)).ToListAsync();
        }

        public async Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByAccountId(int accountId)
        {
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.AccountId.Equals(accountId)).ToListAsync();
        }

        public async Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByBusinessId(int businessId)
        {
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.BusinessId.Equals(businessId)).ToListAsync();
        }

        public async Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByBusinessIdAndOrderProcessId(int businessId, int orderProcessId)
        {
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.BusinessId.Equals(businessId) && x.OrderProcessId.Equals(orderProcessId)).ToListAsync();
        }

        public async Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByBusinessIdAndOrderId(int businessId, int orderId)
        {
            return await Handler.Database.Table<OrderProcessDetailSync>().Where(x => x.BusinessId.Equals(businessId) && x.OrderId.Equals(orderId)).ToListAsync();
        }
    }

    public interface IOrderProcessDetailTable
    {
        Task AddUpdateOrderProcessDetail(List<OrderProcessDetailSync> orderProcessDetail);
        Task<OrderProcessDetailSync> GetOrderProcessDetailById(int id);
        Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByOrderProcessId(int orderProcessDetail);
        Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByOrderId(int orderId);
        Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByAccountId(int accountId);
        Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByBusinessId(int businessId);
        Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByBusinessIdAndOrderProcessId(int businessId, int orderProcessId);
        Task<List<OrderProcessDetailSync>> GetOrderProcessDetailByBusinessIdAndOrderId(int businessId, int orderId);
    }
}
