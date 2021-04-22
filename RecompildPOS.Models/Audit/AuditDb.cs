using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Models.Audit
{
    public class AuditDb
    {
        public bool IsSynced { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime LastSynced { get; set; }
        public DateTime ResponseTime { get; set; }
        public string RequestUrl { get; set; }
        public string TerminalLogId { get; set; }
        public bool IsPost { get; set; }
        public bool IsPending { get; set; }
        public DateTime RequestedTime { get; set; }
        public string SerialNo { get; set; }
        public int ErrorCode { get; set; }
    }
}
