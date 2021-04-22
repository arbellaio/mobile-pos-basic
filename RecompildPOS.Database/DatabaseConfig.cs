using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Database
{
    public static class DatabaseConfig
    {
        public static string DatabaseName = "RecompildPOS.db3";
        public enum Tables
        {
            UserSync,
            User,
            AccountSync,
            Account,
            ProductSync,
            Product,
            InventoryStockSync,
            InventoryStock,
            OrderSync,
            Order,
            OrderProcessSync,
            OrderProcess,
            EndOfDayReportSync,
            EndOfDayReport,
            TransactionSync,
            Transaction,
            BusinessSync,
            Business,
            BusinessFinanceSync,
            BusinessFinance,
            OrderDetailSync,
            OrderDetail,
            OrderDetailProcessSync,
            OrderDetailProcess,
            SyncLog,
            BusinessExpenseSync,
            BusinessExpense
        }
    }
}
