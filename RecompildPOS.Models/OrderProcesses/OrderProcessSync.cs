using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Audit;
using RecompildPOS.Models.Orders;
using SQLite;

namespace RecompildPOS.Models.OrderProcesses
{
    public class OrderProcessSync : AuditEntity
    {
        [PrimaryKey]
        public int OrderProcessId { get; set; }
        public string OrderToken { get; set; }
        public int? OrderId { get; set; }
        public int? AccountId { get; set; }
        public string OrderNotes { get; set; }
        public decimal OrderProcessTotal { get; set; }
        public int? OrderProcessStatusId { get; set; }
        public int TransactionTypeId { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }
        public List<OrderProcessDetailSync> OrderProcessDetails { get; set; }
    }

    public class OrderProcess : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int OrderProcessId { get; set; }
        public string OrderToken { get; set; }
        public int? OrderId { get; set; }
        public int? AccountId { get; set; }
        public string OrderNotes { get; set; }
        public decimal OrderProcessTotal { get; set; }
        public int? OrderProcessStatusId { get; set; }
        public int TransactionTypeId { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }
        public List<OrderProcessDetail> OrderProcessDetails { get; set; }
    }
}
