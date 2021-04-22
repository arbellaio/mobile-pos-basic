using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Accounts
{
    public class AccountSync : AuditEntity
    {
        
        [PrimaryKey]
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }


    }
}
