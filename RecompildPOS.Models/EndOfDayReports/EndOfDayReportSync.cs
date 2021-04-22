using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.EndOfDayReports
{
    public class EndOfDayReportSync : AuditEntity
    {

        [PrimaryKey]
        public int EndOfDayReportId { get; set; }
        public string OrderToken { get; set; }
        public decimal TotalSale { get; set; }
        public decimal TotalPaidCash { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalCashSubmitted { get; set; }
        public DateTime SubmittedDate { get; set; }
        public decimal TotalNetSale { get; set; }
        public decimal TotalNetTax { get; set; }
        public int UserId { get; set; }
        public string TransactionLogId { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }
       
    }
}
