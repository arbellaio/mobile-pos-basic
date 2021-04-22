using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Users
{
    public class UserSync : AuditEntity
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }
        public int UserRole { get; set; }

        [JsonIgnore, Ignore]
        public int SyncLogId { get; set; }
    }
}
