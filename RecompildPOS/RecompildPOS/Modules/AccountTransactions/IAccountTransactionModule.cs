using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.Transactions;

namespace RecompildPOS.Modules.AccountTransactions
{
    public interface IAccountTransactionModule
    {
        Task SyncAccountTransactions();
    }
}