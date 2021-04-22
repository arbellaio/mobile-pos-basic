using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.Finances;

namespace RecompildPOS.Modules.BusinessFinances
{
    public interface IBusinessFinanceModule
    {
        Task SyncBusinessFinances();
    }
}