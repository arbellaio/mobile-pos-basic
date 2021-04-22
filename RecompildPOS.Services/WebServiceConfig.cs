using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Services
{
    public static class WebServiceConfig
    {
        public const string BaseUrl = "http://10.0.2.2:5000";
        public const string RegisterUrl = "/api/BusinessApi/registerBusiness";
        public const string LoginUrl = "/api/BusinessApi/loginBusiness";
        public const string CheckServerConnectionUrl = "/api/AcknowledgementApi/serverPing";
        public const string BusinessesUrl = "/api/BusinessApi/businesses";
        public const string PostBusinessesUrl = "/api/BusinessApi/postBusinesses";
        public const string VerifyAck = "/api/acknowledgementApi/verifyAcknowlegement";
        public const string UsersUrl = "/api/UsersApi/users";
        public const string PostUsersUrl = "/api/UsersApi/postUsers";
        public const string AccountsUrl = "/api/accountApi/accounts";
        public const string PostAccountsUrl = "/api/accountApi/postAccounts";
        public const string ProductsUrl = "/api/productApi/products";
        public const string PostProductsUrl = "/api/productApi/postProducts";
        public const string AccountTransactionUrl = "/api/accountTransactionApi/accountTransactions";
        public const string PostAccountTransactionUrl = "/api/accountTransactionApi/postAccountTransactions";
        public const string OrdersUrl = "/api/orderApi/orders";
        public const string PostOrdersUrl = "/api/orderApi/postOrders";
        public const string OrderProcessesUrl = "/api/orderProcessApi/orderProcesses";
        public const string PostOrderProcessesUrl = "/api/orderProcessApi/postOrderProcesses";
        public const string BusinessFinancesUrl = "/api/businessFinanceApi/businessFinances";
        public const string PostBusinessFinancesUrl = "/api/businessFinanceApi/postBusinessFinances";
        public const string BusinessExpensesUrl = "/api/businessExpenseApi/businessExpenses";
        public const string PostBusinessExpensesUrl = "/api/businessExpenseApi/postBusinessExpenses";
        public const string EndOfDayReportUrl = "/api/endOfDayReportApi/endOfDayReports";
        public const string PostEndOfDayReportUrl = "/api/endOfDayReportApi/postEndOfDayReports";





    }
}
