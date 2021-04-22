using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Orders
{
    public class OrderDetailSync : AuditEntity
    {
        [PrimaryKey]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string Notes { get; set; }
        public decimal Quantity { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public bool IsDeleted { get; set; }
        public int? OrderDetailStatusId { get; set; }
        public int BusinessId { get; set; }

    }
}
