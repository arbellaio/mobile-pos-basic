using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Finances
{
    public class BusinessFinanceSync : AuditEntity
    {
        [PrimaryKey]
        public int BusinessFinanceId { get; set; }
        public decimal MonthlyEarning { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
