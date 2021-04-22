using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.OrderProcesses;

namespace RecompildPOS.Database.OrderProcesses
{
    public class OrderProcessTable : IOrderProcessTable
    {
        public LocalDatabase Handler { get; private set; }
        public OrderProcessTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateOrderProcesses(List<OrderProcessSync> orderProcesses)
        {
            if (orderProcesses != null && orderProcesses.Any())
            {
                foreach (var orderProcess in orderProcesses)
                {
                    await AddUpdateOrderProcess(orderProcess);
                }
            }
        }
        public async Task AddUpdateOrderProcess(OrderProcessSync orderProcess)
        {
            if (orderProcess != null)
            {
                var orderProcessInDb = await GetOrderProcessByOrderProcessId(orderProcess.OrderProcessId);
                if (orderProcessInDb == null)
                {
                    if (!orderProcess.IsDeleted)
                    {
                        await Handler.Database.InsertAsync(orderProcess);
                    }
                }
                else
                {
                    if (!orderProcess.IsDeleted)
                    {
                        await Handler.Database.UpdateAsync(orderProcess);
                    }
                    else
                    {
                        await Handler.Database.DeleteAsync(orderProcess);
                    }
                }

                if (orderProcess.OrderProcessDetails != null && orderProcess.OrderProcessDetails.Any())
                {
                    await Handler.OrderProcessDetails.AddUpdateOrderProcessDetail(orderProcess.OrderProcessDetails);
                }
                    
            }
        }

        public async Task<OrderProcessSync> GetOrderProcessByOrderToken(string orderToken)
        {
            var orderProcessInDb = await Handler.Database.Table<OrderProcessSync>().Where(x => x.OrderToken.Equals(orderToken)).FirstOrDefaultAsync();
            var orderProcessDetails = await Handler.OrderProcessDetails.GetOrderProcessDetailByOrderProcessId(orderProcessInDb.OrderProcessId);
            orderProcessInDb.OrderProcessDetails = orderProcessDetails;
            return orderProcessInDb;
        }

        public async Task<OrderProcessSync> GetOrderProcessByOrderProcessId(int id)
        {
            var orderProcessInDb = await Handler.Database.Table<OrderProcessSync>().Where(x => x.OrderProcessId.Equals(id)).FirstOrDefaultAsync();
            var orderProcessDetails = await Handler.OrderProcessDetails.GetOrderProcessDetailByOrderProcessId(orderProcessInDb.OrderProcessId);
            orderProcessInDb.OrderProcessDetails = orderProcessDetails;
            return orderProcessInDb;
        }
        public async Task<OrderProcessSync> GetOrderProcessById(int id)
        {
            return await Handler.Database.Table<OrderProcessSync>().Where(x => x.OrderProcessId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<OrderProcessSync> GetOrderProcessByOrderId(int orderId)
        {
            var orderProcessInDb = await Handler.Database.Table<OrderProcessSync>().Where(x => x.OrderId.Equals(orderId)).FirstOrDefaultAsync();
            var orderProcessDetails = await Handler.OrderProcessDetails.GetOrderProcessDetailByOrderProcessId(orderProcessInDb.OrderProcessId);
            orderProcessInDb.OrderProcessDetails = orderProcessDetails;
            return orderProcessInDb;
        }

        public async Task<OrderProcessSync> GetOrderProcessByAccountId(int accountId)
        {
            var orderProcessInDb = await Handler.Database.Table<OrderProcessSync>().Where(x => x.AccountId.Equals(accountId)).FirstOrDefaultAsync();
            var orderProcessDetails = await Handler.OrderProcessDetails.GetOrderProcessDetailByOrderProcessId(orderProcessInDb.OrderProcessId);
            orderProcessInDb.OrderProcessDetails = orderProcessDetails;
            return orderProcessInDb;
        }
    }

    public interface IOrderProcessTable
    {
        Task AddUpdateOrderProcesses(List<OrderProcessSync> orderProcess);
        Task AddUpdateOrderProcess(OrderProcessSync orderProcess);
        Task<OrderProcessSync> GetOrderProcessByOrderToken(string orderToken);
        Task<OrderProcessSync> GetOrderProcessByOrderProcessId(int id);
        Task<OrderProcessSync> GetOrderProcessById(int id);
        Task<OrderProcessSync> GetOrderProcessByOrderId(int orderId);
        Task<OrderProcessSync> GetOrderProcessByAccountId(int accountId);
    }
}
