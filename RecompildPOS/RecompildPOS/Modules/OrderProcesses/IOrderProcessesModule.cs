using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.OrderProcesses;

namespace RecompildPOS.Modules.OrderProcesses
{
    public interface IOrderProcessesModule
    {
        Task AddUpdateOrderProcessInSyncLog(OrderProcessSync orderProcess);
        Task<OrderProcessSync> GetOrderProcessByOrderProcessId(int id);
        Task<OrderProcessSync> GetOrderProcessByOrderId(int orderId);
        Task<List<OrderProcessSync>> GetOrderProcessFromSyncLog();
    }
}
