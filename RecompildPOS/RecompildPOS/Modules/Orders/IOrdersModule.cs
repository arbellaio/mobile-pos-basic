using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.Orders;

namespace RecompildPOS.Modules.Orders
{
    public interface IOrdersModule
    {
        Task AddUpdateOrders(OrderSync order); 
        Task<OrderSync> GetOrderByOrderId(int id);
        Task<OrderSync> GetOrderById(int id);
        Task<OrderSync> GetOrderByOrderNumber(string orderNumber);
        Task<OrderSync> GetOrderByInvoiceNumber(string invoiceNumber);
        Task<List<OrderSync>> GetOrderByAccountId(int accountId);
    }
}