using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.EndOfDayReports;
using RecompildPOS.Models.Expense;
using RecompildPOS.Models.Finances;
using RecompildPOS.Models.InventoryStocks;
using RecompildPOS.Models.OrderProcesses;
using RecompildPOS.Models.Orders;
using RecompildPOS.Models.Products;
using RecompildPOS.Models.Transactions;
using RecompildPOS.Models.Users;

namespace RecompildPOS.Models.ServicesModels
{
    public class AccountSyncCollection
    {
        public List<AccountSync> Accounts { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }

        public int BusinessId { get; set; }

    }

    public class AccountTransactionSyncCollection
    {
        public List<AccountTransactionSync> AccountTransactions { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }

        public int BusinessId { get; set; }

    }

    public class OrderProcessSyncCollection
    {
        public List<OrderProcessSync> OrderProcesses { get; set; }
        public int Count { get; set; }
        public string SerialNo { get; set; }

        public string TerminalLogId { get; set; }
        public int BusinessId { get; set; }

    }

    public class OrderSyncCollection
    {
        public List<OrderSync> Orders { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }

    public class UserSyncCollection
    {
        public List<UserSync> Users { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }
    }

    public class EndOfDayReportSyncCollection
    {
        public List<EndOfDayReportSync> EndOfDayReports { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }

    public class BusinessSyncCollection
    {
        public List<BusinessSync> Businesses { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }

    public class BusinessFinanceSyncCollection
    {
        public List<BusinessFinanceSync> BusinessFinances { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }

    public class BusinessExpenseSyncCollection
    {
        public List<BusinessExpenseSync> BusinessExpenses { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }

    public class ProductsSyncCollection
    {
        public List<ProductSync> Products { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }

    public class InventorySyncCollection
    {
        public List<InventoryStockSync> InventoryStocks { get; set; }
        public int Count { get; set; }
        public string TerminalLogId { get; set; }
        public string SerialNo { get; set; }
        public int BusinessId { get; set; }


    }
}
