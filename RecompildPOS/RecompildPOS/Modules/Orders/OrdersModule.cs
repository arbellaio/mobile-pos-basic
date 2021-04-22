using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Orders;
using RecompildPOS.Views;

namespace RecompildPOS.Modules.Orders
{
    public class OrdersModule : IOrdersModule
    {
        public async Task AddUpdateOrders(OrderSync order)
        {
            if (order != null)
            {
                await App.Database.Orders.AddUpdateOrders(order);
            }
        }

        public async Task<OrderSync> GetOrderById(int id)
        {
            if (id > 0)
            {
                return await App.Database.Orders.GetOrderById(id);
            }

            return null;
        }

        public async Task<OrderSync> GetOrderByOrderId(int id)
        {
            if (id > 0)
            {
               return await App.Database.Orders.GetOrderByOrderId(id);
            }

            return null;
        }

        public async Task<OrderSync> GetOrderByOrderNumber(string orderNumber)
        {
            if (!string.IsNullOrEmpty(orderNumber))
            {
                return await App.Database.Orders.GetOrderByOrderNumber(orderNumber);
            }

            return null;
        }

        public async Task<OrderSync> GetOrderByInvoiceNumber(string invoiceNumber)
        {
            if (!string.IsNullOrEmpty(invoiceNumber))
            {
                return await App.Database.Orders.GetOrderByInvoiceNumber(invoiceNumber);
            }

            return null;
        }

        public async Task<List<OrderSync>> GetOrderByAccountId(int accountId)
        {
            if (accountId > 0)
            {
                return await App.Database.Orders.GetOrderByAccountId(accountId);
            }
            return null;
        }
    }
}
