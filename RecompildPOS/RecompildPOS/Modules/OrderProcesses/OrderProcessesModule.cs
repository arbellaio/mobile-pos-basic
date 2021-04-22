using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.OrderProcesses;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.OrderProcesses
{
    public class OrderProcessesModule : IOrderProcessesModule
    {
        public async Task AddUpdateOrderProcessInSyncLog(OrderProcessSync orderProcess)
        {
            if (orderProcess != null)
            {
                await App.Database.OrderProcesses.AddUpdateOrderProcess(orderProcess);
            }
        }

        public async Task<OrderProcessSync> GetOrderProcessByOrderProcessId(int id)
        {
            if (id > 0)
            {
                await App.Database.OrderProcesses.GetOrderProcessByOrderProcessId(id);
            }

            return null;
        }

        public async Task<OrderProcessSync> GetOrderProcessByOrderId(int orderId)
        {
            if (orderId > 0)
            {
                await App.Database.OrderProcesses.GetOrderProcessByOrderId(orderId);
            }

            return null;
        }

        public async Task<List<OrderProcessSync>> GetOrderProcessFromSyncLog()
        {
            throw new System.NotImplementedException();
        }
    }
}