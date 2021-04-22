using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Orders;

namespace RecompildPOS.Database.Orders
{
    public class OrdersTable : IOrdersTable
    {
        public LocalDatabase Handler { get; private set; }
        public OrdersTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateOrders(List<OrderSync> orders)
        {
            if (orders != null && orders.Any())
            {
                foreach (var order in orders)
                {
                    await AddUpdateOrders(order);
                }
            }
        }

        public async Task AddUpdateOrders(OrderSync order)
        {
            if (order != null)
            {
                var orderInDb = await GetOrderByOrderId(order.OrderId);
                if (orderInDb == null)
                {
                    if (!order.IsDeleted)
                    { 
                        await Handler.Database.InsertAsync(order);
                    }
                }
                else
                {
                    if (order.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(order);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(order);
                    }
                }

                if (order.OrderDetails != null && order.OrderDetails.Any())
                {
                    await Handler.OrderDetails.AddUpdateOrderDetails(order.OrderDetails);
                }
            }
        }


        public async Task<OrderSync> GetOrderById(int id)
        {
            var orderInDb = await Handler.Database.Table<OrderSync>().Where(x => x.OrderId.Equals(id)).FirstOrDefaultAsync();
            var orderDetailInDb = await Handler.OrderDetails.GetOrderDetailsById(orderInDb.OrderId);
            orderInDb.OrderDetails = orderDetailInDb;
            return orderInDb;
        }
        public async Task<OrderSync> GetOrderByOrderId(int id)
        {
            var orderInDb = await Handler.Database.Table<OrderSync>().Where(x => x.OrderId.Equals(id)).FirstOrDefaultAsync();
            var orderDetailInDb = await Handler.OrderDetails.GetOrderDetailsByOrderId(orderInDb.OrderId);
            orderInDb.OrderDetails = orderDetailInDb;
            return orderInDb;
        }

        public async Task<OrderSync> GetOrderByOrderNumber(string orderNumber)
        {
            var orderInDb = await Handler.Database.Table<OrderSync>().Where(x => x.OrderNumber.Equals(orderNumber)).FirstOrDefaultAsync();
            var orderDetailInDb = await Handler.OrderDetails.GetOrderDetailsByOrderId(orderInDb.OrderId);
            orderInDb.OrderDetails = orderDetailInDb;
            return orderInDb;
        }

        public async Task<OrderSync> GetOrderByInvoiceNumber(string invoiceNumber)
        {
            var orderInDb = await Handler.Database.Table<OrderSync>().Where(x => x.InvoiceNo.Equals(invoiceNumber)).FirstOrDefaultAsync();
            var orderDetailInDb = await Handler.OrderDetails.GetOrderDetailsByOrderId(orderInDb.OrderId);
            orderInDb.OrderDetails = orderDetailInDb;
            return orderInDb;
        }

        public async Task<List<OrderSync>> GetOrderByAccountId(int accountId)
        {
            var ordersInDb = await Handler.Database.Table<OrderSync>().Where(x => x.AccountId.Equals(accountId)).ToListAsync();
            foreach (var orderInDb in ordersInDb)
            {
                var orderDetailInDb = await Handler.OrderDetails.GetOrderDetailsByOrderId(orderInDb.OrderId);
                orderInDb.OrderDetails = orderDetailInDb;
            }

            return ordersInDb;
        }
    }

    public interface IOrdersTable
    {
        Task AddUpdateOrders(List<OrderSync> orders);
        Task AddUpdateOrders(OrderSync order);
        Task<OrderSync> GetOrderById(int id);
        Task<OrderSync> GetOrderByOrderId(int id);
        Task<OrderSync> GetOrderByOrderNumber(string orderNumber);
        Task<OrderSync> GetOrderByInvoiceNumber(string invoiceNumber);
        Task<List<OrderSync>> GetOrderByAccountId(int accountId);
    }
}
