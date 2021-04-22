using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Orders;

namespace RecompildPOS.Database.Orders
{
    public class OrderDetailsTable : IOrderDetailsTables
    {
        public LocalDatabase Handler { get; private set; }
        public OrderDetailsTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateOrderDetails(List<OrderDetailSync> orderDetails)
        {
            if (orderDetails != null && orderDetails.Any())
            {
                foreach (var orderDetail in orderDetails)
                {
                    var orderDetailInDb = await GetOrderDetailsByOrderId(orderDetail.OrderId);
                    if (orderDetailInDb == null)
                    {
                        if (!orderDetail.IsDeleted)
                        {
                            await Handler.Database.InsertAsync(orderDetail);
                        }
                    }
                    else
                    {
                        if (orderDetail.IsDeleted)
                        {
                            await Handler.Database.DeleteAsync(orderDetail);
                        }
                        else
                        {
                            await Handler.Database.UpdateAsync(orderDetail);
                        }
                    }
                }
            }
        }

        public async Task<List<OrderDetailSync>> GetOrderDetailsById(int id)
        {
            return await Handler.Database.Table<OrderDetailSync>().Where(x => x.OrderDetailId.Equals(id)).ToListAsync();
        }

        public async Task<List<OrderDetailSync>> GetOrderDetailsByOrderId(int orderId)
        {
            return await Handler.Database.Table<OrderDetailSync>().Where(x => x.OrderId.Equals(orderId)).ToListAsync();
        }

        public async Task<List<OrderDetailSync>> GetOrderDetailsByBusinessId(int businessId, int orderId)
        {
            return await Handler.Database.Table<OrderDetailSync>().Where(x => x.OrderId.Equals(orderId) && x.BusinessId.Equals(businessId)).ToListAsync();
        }
    }

    public interface IOrderDetailsTables
    {
        Task AddUpdateOrderDetails(List<OrderDetailSync> orderDetails);
        Task<List<OrderDetailSync>> GetOrderDetailsById(int id);
        Task<List<OrderDetailSync>> GetOrderDetailsByOrderId(int orderId);
        Task<List<OrderDetailSync>> GetOrderDetailsByBusinessId(int businessId, int orderId);

    }
}
