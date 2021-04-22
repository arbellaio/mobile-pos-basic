using System.Collections.Generic;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Orders
{
    public class Order : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Notes { get; set; }
        public int? AccountId { get; set; }
        public int? StatusId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? OrderCost { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}