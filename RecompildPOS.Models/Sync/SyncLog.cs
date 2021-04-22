using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Sync
{
    public class SyncLog
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Request { get; set; }
        public string TableName { get; set; }
        public string SerialNo { get; set; }
        public int ErrorCode { get; set; }
        public bool Synced { get; set; }
        public int ResultCount { get; set; }
        public bool IsPost { get; set; }
        public bool IsPending { get; set; }
        public DateTime RequestedTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public string RequestUrl { get; set; }
        public string TerminalLogId { get; set; }
     

    }
}
