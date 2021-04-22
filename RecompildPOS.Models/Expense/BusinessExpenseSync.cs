using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Expense
{
    public class BusinessExpenseSync : AuditEntity
    {
        [PrimaryKey]
        public int BusinessExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public decimal ExpenseAmount { get; set; }
        public int ExpenseCycle { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
