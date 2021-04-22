using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.InventoryStocks
{
    public class InventoryStock : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int InventoryStockId { get; set; }
        public int ProductId { get; set; }
        public decimal InStock { get; set; }
        public decimal Available { get; set; }
        public bool IsDeleted { get; set; }
    }
}