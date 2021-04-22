using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Transactions;

namespace RecompildPOS.Database.AccountTransactions
{
    public class AccountTransactionTable : IAccountTransactionTable
    {
        public LocalDatabase Handler { get; private set; }
        public AccountTransactionTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        #region AccountSync Methods
        public async Task AddUpdateAccountTransactionsSync(List<AccountTransactionSync> accountTransactions)
        {
            if (accountTransactions != null && accountTransactions.Any())
            {
                foreach (var accountTransaction in accountTransactions)
                {
                    await AddUpdateAccountTransactionSync(accountTransaction);
                }
            }
        }

        public async Task AddUpdateAccountTransactionSync(AccountTransactionSync accountTransaction)
        {
            if (accountTransaction != null)
            {
                var accountTransactionInDb = await GetAccountTransactionSyncById(accountTransaction.AccountTransactionId);
                if (accountTransactionInDb == null)
                {
                    if (!accountTransaction.IsDeleted)
                    {
                        await Handler.Database.InsertAsync(accountTransaction);
                    }
                }
                else
                {
                    if (accountTransaction.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(accountTransaction);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(accountTransaction);
                    }
                }
            }
        }

        public async Task<AccountTransactionSync> GetAccountTransactionSyncById(int id)
        {
            return await Handler.Database.Table<AccountTransactionSync>().Where(x => x.AccountTransactionId.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<AccountTransactionSync>> GetAccountTransactionSyncByBusinessAndAccountId(int businessId, int accountId)
        {
            return await Handler.Database.Table<AccountTransactionSync>().Where(x => x.AccountId.Equals(accountId) && x.BusinessId.Equals(businessId))
                .ToListAsync();
        }

        public async Task<AccountTransactionSync> GetAccountTransactionSyncByOrderId(int orderId)
        {
            return await Handler.Database.Table<AccountTransactionSync>().Where(x => x.OrderId.Equals(orderId))
                .FirstOrDefaultAsync();
        }

        public async Task<AccountTransactionSync> GetAccountTransactionSyncByInvoiceNumber(string invoiceNumber)
        {
            return await Handler.Database.Table<AccountTransactionSync>().Where(x => x.InvoiceNo.Equals(invoiceNumber))
                .FirstOrDefaultAsync();
        }

        

        #endregion
        
        #region Account Methods

        public async Task AddUpdateAccountTransaction(AccountTransaction accountTransaction)
        {
            if (accountTransaction != null)
            {
                var accountTransactionInDb = await GetAccountTransactionById(accountTransaction.AccountId);
                if (accountTransactionInDb == null)
                {
                    await Handler.Database.InsertAsync(accountTransaction);
                }
                else
                {
                    await Handler.Database.UpdateAsync(accountTransaction);
                }
            }
        }

        
        public async Task<List<AccountTransaction>> GetAllUnSyncedAccountTransactions()
        {
           return await Handler.Database.Table<AccountTransaction>().Where(x => x.IsPost && !x.IsSynced).ToListAsync();
        }

        public async Task<AccountTransaction> GetAccountTransactionById(int id)
        {
            return await Handler.Database.Table<AccountTransaction>().Where(x => x.AccountId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task UpdateAccountTransactions(AccountTransaction accountTransaction)
        {
            if (accountTransaction != null)
            {
                await Handler.Database.UpdateAsync(accountTransaction);
            }
        }

        #endregion

    }

    public interface IAccountTransactionTable
    {

        #region AccountTransactionSync
        Task AddUpdateAccountTransactionsSync(List<AccountTransactionSync> accountTransactions);
        Task AddUpdateAccountTransactionSync(AccountTransactionSync accountTransaction);
        Task<AccountTransactionSync> GetAccountTransactionSyncById(int id);
        Task<List<AccountTransactionSync>> GetAccountTransactionSyncByBusinessAndAccountId(int businessId, int accountId);
        Task<AccountTransactionSync> GetAccountTransactionSyncByOrderId(int orderId);
        Task<AccountTransactionSync> GetAccountTransactionSyncByInvoiceNumber(string invoiceNumber);

        #endregion

        #region AccountTransaction

        Task AddUpdateAccountTransaction(AccountTransaction accountTransaction);
        Task<List<AccountTransaction>> GetAllUnSyncedAccountTransactions();
        Task<AccountTransaction> GetAccountTransactionById(int id);
        Task UpdateAccountTransactions(AccountTransaction accountTransaction);

        #endregion

    }
}
