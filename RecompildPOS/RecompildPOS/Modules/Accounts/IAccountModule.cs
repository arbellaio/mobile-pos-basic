using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Sync;

namespace RecompildPOS.Modules.Accounts
{
    public interface IAccountModule
    {
        Task SyncAccounts();
    }
}