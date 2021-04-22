using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Sync;

namespace RecompildPOS.Database.Accounts
{
    public class AccountsTable : IAccountsTable
    {
        public LocalDatabase Handler { get; private set; }
        public AccountsTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

       

        /// <summary>
        /// This method takes AccountSync list as parameter and adds updates list of accounts
        /// </summary>
        /// <param name="AccountSync"></param>
        /// <returns></returns>
        public async Task AddOrUpdateAccountsSync(List<AccountSync> accountSyncs)
        {
            if (accountSyncs != null && accountSyncs.Any())
            {
                foreach (var account in accountSyncs)
                {
                    var accountInDb = await GetAccountSyncByAccountId(account.AccountId);
                    if (accountInDb == null)
                    {
                        if (account.IsDeleted == false)
                        {
                            await Handler.Database.InsertAsync(account);
                        }
                    }
                    else
                    {
                        if (account.IsDeleted == false)
                        {
                            await Handler.Database.UpdateAsync(account);
                        }
                        else
                        {
                            await Handler.Database.DeleteAsync(accountInDb);
                        }
                    }
                }
            }
        }


        public async Task<AccountSync> GetAccountSyncByEmail(string email)
        {
            return await Handler.Database.Table<AccountSync>().Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Add Accounts From Contact Book
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public async Task AddUpdateAccountFromContacts(List<Account> accounts)
        {
            if (accounts != null && accounts.Any())
            {
                foreach (var account in accounts)
                {
                    var accountInDb = await GetAccountByAccountId(account.LocalAccountId);
                    if (accountInDb == null)
                    {
                        if (account.IsSynced == false)
                        {
                            await Handler.Database.InsertAsync(account);
                        }
                    }
                    else
                    {
                        if (account.IsSynced == false)
                        {
                            await Handler.Database.UpdateAsync(account);
                        }
                        else
                        {
                            await Handler.Database.DeleteAsync(accountInDb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method takes AccountSync as parameter and adds updates account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task AddOrUpdateAccount(Account account)
        {
            var accountInDb = await GetAccountByAccountId(account.LocalAccountId);
            if (accountInDb == null)
            {
                if (account.IsSynced == false)
                {
                    await Handler.Database.InsertAsync(account);
                }
            }
            else
            {
                if (account.IsSynced == false)
                {
                    await Handler.Database.UpdateAsync(account);
                }
                else
                {
                    await Handler.Database.DeleteAsync(accountInDb);
                }
            }
        }

        public async Task<AccountSync> GetAccountSyncByAccountId(int id)
        {
            return await Handler.Database.Table<AccountSync>().Where(x => x.AccountId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountByAccountId(int id)
        {
            return await Handler.Database.Table<Account>().Where(x => x.LocalAccountId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<AccountSync> GetAccountSyncByPhoneNumber(string phoneNumber)
        {
            return await Handler.Database.Table<AccountSync>().Where(x => x.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountByPhoneNumber(string phoneNumber)
        {
            return await Handler.Database.Table<Account>().Where(x => x.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();
        }

        public async Task<AccountSync> GetAccountSyncByName(string name)
        {
            return await Handler.Database.Table<AccountSync>().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetAllUnSyncedAccounts()
        {
            return await Handler.Database.Table<Account>().Where(x => x.IsPost && !x.IsSynced).ToListAsync();
        }

        public async Task<Account> GetAccountByName(string name)
        {
            return await Handler.Database.Table<Account>().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<AccountSync> GetAccountSyncByAccountCode(string accCode)
        {
            return await Handler.Database.Table<AccountSync>().Where(x => x.AccountCode.Equals(accCode)).FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountByAccountCode(string accCode)
        {
            return await Handler.Database.Table<Account>().Where(x => x.AccountCode.Equals(accCode)).FirstOrDefaultAsync();
        }

        
        public async Task<Account> GetAccountByEmail(string email)
        {
            return await Handler.Database.Table<Account>().Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task UpdateAccount(Account account)
        { 
            await Handler.Database.UpdateAsync(account);
        }

       
    }

    public interface IAccountsTable
    {
        Task AddUpdateAccountFromContacts(List<Account> accounts);
        Task AddOrUpdateAccountsSync(List<AccountSync> accounts);
        Task AddOrUpdateAccount(Account account);
        Task<Account> GetAccountByAccountId(int id);
        Task<Account> GetAccountByPhoneNumber(string phoneNumber);
        Task<AccountSync> GetAccountSyncByPhoneNumber(string phoneNumber);
        Task<Account> GetAccountByName(string name);
        Task<Account> GetAccountByAccountCode(string accCode);
        Task<Account> GetAccountByEmail(string email);
        Task<AccountSync> GetAccountSyncByEmail(string email);
        Task<AccountSync> GetAccountSyncByAccountCode(string accCode);
        Task<AccountSync> GetAccountSyncByName(string name);

        Task<List<Account>> GetAllUnSyncedAccounts();
        Task UpdateAccount(Account account);
    }
}