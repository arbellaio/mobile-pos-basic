using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Transactions
{
    public class AccountTransactionSync : AuditEntity
    {
        [PrimaryKey]
        public int AccountTransactionId { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        
        // OrderCost + Tax if any and without Discount
        public decimal OrderAmount { get; set; }

        // OrderTotal is with Tax and Discount
        public decimal OrderCost {
            get
            {
                return (OrderAmount) - TotalDiscount;
            }
            set { }
        }
        public decimal PaidAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public bool IsDeleted { get; set; }
        public string OrderToken { get; set; }
        public int OrderId { get; set; }
        public string Notes { get; set; }
        public string InvoiceNo { get; set; }
        public int OrderProcessId { get; set; }
        public int? AccountPaymentModeId { get; set; }

        public decimal ClosingAccountBalance
        {
            get { return (OpeningAccountBalance + OrderCost) - PaidAmount; }
            set { }
        }

        public decimal OpeningAccountBalance { get; set; }
        public int BusinessId { get; set; }


    }
}
