using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.Expense;

namespace RecompildPOS.Database.BusinessFinances
{
    public class BusinessExpensesTable : IBusinessExpensesTable
    {
        public LocalDatabase Handler { get; private set; }
        public BusinessExpensesTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateBusinessExpenses(List<BusinessExpenseSync> businessExpenses)
        {
            if (businessExpenses != null && businessExpenses.Any())
            {
                foreach (var businessExpense in businessExpenses)
                {
                    await AddUpdateBusinessExpense(businessExpense);
                }
            }
        }
        public async Task AddUpdateBusinessExpense(BusinessExpenseSync businessExpense)
        {
            if (businessExpense != null)
            {
                if (await GetBusinessExpensesById(businessExpense.BusinessExpenseId) != null)
                {
                    if (businessExpense.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(businessExpense);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(businessExpense);
                    }
                }
                else
                {
                    await Handler.Database.InsertAsync(businessExpense);
                }   
            }
        }

        public async Task<List<BusinessExpenseSync>> GetBusinessExpenses()
        {
           return await Handler.Database.Table<BusinessExpenseSync>().ToListAsync();
        }

        public async Task<List<BusinessExpenseSync>> GetBusinessExpensesByBusinessId(int businessId)
        {
            return await Handler.Database.Table<BusinessExpenseSync>().Where(x => x.BusinessId.Equals(businessId)).ToListAsync();
        }

        public async Task<BusinessExpenseSync> GetBusinessExpensesById(int id)
        {
            return await Handler.Database.Table<BusinessExpenseSync>().Where(x => x.BusinessExpenseId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<BusinessExpenseSync>> GetBusinessExpensesByBusinessOwnerUserId(int businessId, int userId)
        {
            if (await Handler.Database.Table<BusinessSync>().Where(x => x.BusinessId.Equals(businessId) && x.BusinessOwnerUserId.Equals(userId)).FirstOrDefaultAsync() != null)
            {
                return await GetBusinessExpensesByBusinessId(businessId);
            }

            return null;
        }

        public async Task<BusinessExpenseSync> GetBusinessExpenseByName(string expenseName)
        {
            if (!string.IsNullOrEmpty(expenseName))
            {
                return await Handler.Database.Table<BusinessExpenseSync>().Where(x => x.ExpenseName.Equals(expenseName))
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<BusinessExpenseSync> GetBusinessExpenseByAmount(decimal expenseAmount)
        {
            return await Handler.Database.Table<BusinessExpenseSync>().Where(x => x.ExpenseAmount.Equals(expenseAmount))
                .FirstOrDefaultAsync();
        }
        
        public async Task<List<BusinessExpense>> GetAllUnSyncedBusinessExpenses()
        {
            return await Handler.Database.Table<BusinessExpense>().Where(x => x.IsPost && !x.IsSynced)
                .ToListAsync();
        }

        public async Task UpdateBusinessExpense(BusinessExpense businessExpense)
        {
            if (businessExpense != null)
            {
                await Handler.Database.UpdateAsync(businessExpense);
            }
        }
    }

    public interface IBusinessExpensesTable
    {
        Task AddUpdateBusinessExpenses(List<BusinessExpenseSync> businessExpenses);
        Task AddUpdateBusinessExpense(BusinessExpenseSync businessExpense);
        Task<List<BusinessExpenseSync>> GetBusinessExpenses();
        Task<List<BusinessExpenseSync>> GetBusinessExpensesByBusinessId(int businessId);
        Task<BusinessExpenseSync> GetBusinessExpensesById(int id);
        Task<List<BusinessExpenseSync>> GetBusinessExpensesByBusinessOwnerUserId(int businessId, int userId);
        Task<BusinessExpenseSync> GetBusinessExpenseByName(string expenseName);
        Task<BusinessExpenseSync> GetBusinessExpenseByAmount(decimal expenseAmount);
        Task<List<BusinessExpense>> GetAllUnSyncedBusinessExpenses();
        Task UpdateBusinessExpense(BusinessExpense businessExpense);


    }
}
