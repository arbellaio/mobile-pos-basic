using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RecompildPOS.Database.Accounts;
using RecompildPOS.Database.AccountTransactions;
using RecompildPOS.Database.Businesses;
using RecompildPOS.Database.BusinessFinances;
using RecompildPOS.Database.EndOfDayReports;
using RecompildPOS.Database.OrderProcesses;
using RecompildPOS.Database.Orders;
using RecompildPOS.Database.Product;
using RecompildPOS.Database.Sync;
using RecompildPOS.Database.Users;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.EndOfDayReports;
using RecompildPOS.Models.Expense;
using RecompildPOS.Models.Finances;
using RecompildPOS.Models.InventoryStocks;
using RecompildPOS.Models.OrderProcesses;
using RecompildPOS.Models.Orders;
using RecompildPOS.Models.Products;
using RecompildPOS.Models.Sync;
using RecompildPOS.Models.Transactions;
using RecompildPOS.Models.Users;
using SQLite;
using Xamarin.Forms;

namespace RecompildPOS.Database.DatabaseHandler
{
    public interface ILocalDatabase
    {
        IUsersTable Users { get; }
        IAccountsTable Accounts { get; }
        IBusinessesTable Businesses { get; }
        IEndOfDayReportTable EndOfDayReports { get; }
        IBusinessFinancesTable BusinessFinances { get; }
        IAccountTransactionTable AccountTransactions { get; }
        IOrderProcessTable OrderProcesses { get; }
        IOrderProcessDetailTable OrderProcessDetails { get; }
        IOrdersTable Orders { get; }
        IOrderDetailsTables OrderDetails { get; }
        IProductsTable Products { get; }
        IBusinessExpensesTable BusinessExpenses { get; }
        ISyncLogTable SyncLog { get; }

        void OpenConnection();
        void CloseConnection();
    }

    public class LocalDatabase : ILocalDatabase
    {
        private readonly string _databasePath;
        private SQLiteAsyncConnection _database;
        public SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                    OpenConnection();

                return _database;
            }
        }

        public virtual IUsersTable Users { get; private set; }
        public virtual IAccountsTable Accounts { get; private set; }
        public virtual IBusinessesTable Businesses { get; private set; }
        public virtual IEndOfDayReportTable EndOfDayReports { get; private set; }
        public virtual IBusinessFinancesTable BusinessFinances { get; private set; }
        public virtual IAccountTransactionTable AccountTransactions { get; private set; }
        public virtual IProductsTable Products { get; private set; }
        public virtual IOrdersTable Orders { get; private set; }
        public virtual IOrderDetailsTables OrderDetails { get; private set; }
        public virtual IOrderProcessTable OrderProcesses { get; private set; }
        public virtual IOrderProcessDetailTable OrderProcessDetails { get; private set; }
        public virtual IBusinessExpensesTable BusinessExpenses { get; private set; }
        public virtual ISyncLogTable SyncLog { get; private set; }
        
        

        public LocalDatabase()
        {
            try
            {
                _databasePath = DependencyService.Get<IDatabaseConnection>().GetDatabasePath(DatabaseConfig.DatabaseName);
                Initialize();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //_exceptionService.Handle(ex);
            }
            finally
            {
                CloseConnection();
            }
        }

       
        private void Initialize()
        {
            OpenConnection();
            Users = new UsersTable(this);
            Accounts = new AccountsTable(this);
            Businesses = new BusinessesTable(this);
            EndOfDayReports = new EndOfDayReportTable(this);
            BusinessFinances = new BusinessFinancesTable(this);
            AccountTransactions = new AccountTransactionTable(this);
            Products = new ProductsTable(this);
            Orders = new OrdersTable(this);
            OrderDetails = new OrderDetailsTable(this);
            OrderProcessDetails = new OrderProcessDetailTable(this);
            OrderProcesses = new OrderProcessTable(this);
            BusinessExpenses = new BusinessExpensesTable(this);
            SyncLog = new SyncLogTable(this);
        }

        public void OpenConnection()
        {
            _database = new SQLiteAsyncConnection(_databasePath); 

            //todo: need to check if enabling WAL make any database transaction improvement.
            _database.EnableWriteAheadLoggingAsync();
            CreateTables();
        }

        private void CreateTables()
        {
            _database.CreateTableAsync<AccountSync>();
            _database.CreateTableAsync<Account>();
            _database.CreateTableAsync<UserSync>();
            _database.CreateTableAsync<User>();
            _database.CreateTableAsync<BusinessSync>();
            _database.CreateTableAsync<Business>();
            _database.CreateTableAsync<BusinessFinanceSync>();
            _database.CreateTableAsync<BusinessFinance>();
            _database.CreateTableAsync<AccountTransactionSync>();
            _database.CreateTableAsync<AccountTransaction>();
            _database.CreateTableAsync<EndOfDayReportSync>();
            _database.CreateTableAsync<EndOfDayReport>();
            _database.CreateTableAsync<OrderSync>();
            _database.CreateTableAsync<Order>();
            _database.CreateTableAsync<OrderDetailSync>();
            _database.CreateTableAsync<OrderDetail>();
            _database.CreateTableAsync<OrderProcessSync>();
            _database.CreateTableAsync<OrderProcess>();
            _database.CreateTableAsync<OrderProcessDetailSync>();
            _database.CreateTableAsync<OrderProcessDetail>();
            _database.CreateTableAsync<InventoryStockSync>();
            _database.CreateTableAsync<InventoryStock>();
            _database.CreateTableAsync<ProductSync>();
            _database.CreateTableAsync<Models.Products.Product>();
            _database.CreateTableAsync<BusinessExpenseSync>();
            _database.CreateTableAsync<BusinessExpense>();
            _database.CreateTableAsync<SyncLog>();
        }

        public void CloseConnection()
        {
            if (_database != null)
            {
                //_database.GetConnection().Close();
                _database = null;
            }
        }

    }
}
