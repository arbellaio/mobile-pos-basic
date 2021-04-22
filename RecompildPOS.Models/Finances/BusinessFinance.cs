using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Finances
{
    public class BusinessFinance : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int LocalBusinessFinanceId { get; set; }

        public int BusinessFinanceId { get; set; }
        public decimal MonthlyEarning { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public bool IsDeleted { get; set; }
    }
}