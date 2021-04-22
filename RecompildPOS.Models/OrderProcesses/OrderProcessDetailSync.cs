using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.OrderProcesses
{
    public class OrderProcessDetailSync : AuditEntity
    {
        [PrimaryKey]
        public int OrderProcessDetailId { get; set; }
        public int OrderProcessId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityProcessed { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int AccountId { get; set; }
        public int TransactionTypeId { get; set; }
        public int BusinessId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OrderProcessDetail : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int OrderProcessDetailId { get; set; }
        public int OrderProcessId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityProcessed { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int AccountId { get; set; }
        public int TransactionTypeId { get; set; }
        public int BusinessId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
