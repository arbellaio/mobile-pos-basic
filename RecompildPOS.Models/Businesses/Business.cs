using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Businesses
{
    public class Business : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int LocalBusinessId { get; set; }
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsDeleted { get; set; }

        //Business Owner
        public int BusinessOwnerUserId { get; set; }
    }
}