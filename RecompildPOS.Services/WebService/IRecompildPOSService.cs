using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using RecompildPOS.Models.Accounts;
using RecompildPOS.Services.Accounts;
using RecompildPOS.Services.AccountTransaction;
using RecompildPOS.Services.Acknowledgement;
using RecompildPOS.Services.Business;
using RecompildPOS.Services.BusinessFinance;
using RecompildPOS.Services.EndOfDayReport;
using RecompildPOS.Services.OrderProcess;
using RecompildPOS.Services.Orders;
using RecompildPOS.Services.Products;
using RecompildPOS.Services.Register;
using RecompildPOS.Services.ServerPing;
using RecompildPOS.Services.Users;
using RecompildPOS.Services.WebService.RestService;

namespace RecompildPOS.Services.WebService
{
    public interface IRecompildPOSService
    {
        Uri BaseUri { get; set; }
        IAuthService Auth { get; }
        IBusinessService Business { get; }
        IUserService User { get; }
        IServerPingService ServerPing { get; }
        IAcknowledgementService Acknowledgement { get; }
        IAccountService Accounts { get; }
        IProductService Products { get; }
        IOrderService Orders { get; }
        IOrderProcessService OrderProcess { get; }
        IAccountTransactionService AccountTransaction { get; }
        IBusinessFinanceService BusinessFinance { get; }
        IEndOfDayReportService EndOfDayDayReport { get; }


    }

    public class RecompildPOSService : IRecompildPOSService
    {
        public RecompildPOSService()
        {
            HttpClient = new HttpClientExtended();
            Initialize();
        }

        public HttpClientExtended HttpClient { get; private set; }
        public Uri BaseUri { get; set; }
        public virtual IAuthService Auth { get; private set; }
        public virtual IServerPingService ServerPing { get; private set; }
        public virtual IBusinessService Business { get; private set; }
        public virtual IAcknowledgementService Acknowledgement { get; private set; }
        public virtual IUserService User { get; private set; }
        public virtual IAccountService Accounts { get; private set; }
        public virtual IProductService Products { get; private set; }
        public virtual IOrderService Orders { get; private set; }
        public virtual IOrderProcessService OrderProcess { get; private set; }
        public virtual IAccountTransactionService AccountTransaction { get; private set; }
        public virtual IBusinessFinanceService BusinessFinance { get; private set; }
        public virtual IEndOfDayReportService EndOfDayDayReport { get; private set; }


        private void Initialize()
        {
            BaseUri = new Uri(WebServiceConfig.BaseUrl);
            Auth = new AuthService(this);
            ServerPing = new ServerPingService(this);
            Business = new BusinessService(this);
            Acknowledgement = new AcknowledgmentService(this);
            User = new UserService(this);
            Accounts = new AccountService(this);
            Products = new ProductService(this);
            Orders = new OrderService(this);
            OrderProcess = new OrderProcessService(this);
            AccountTransaction = new AccountTransactionService(this);
            BusinessFinance = new BusinessFinanceService(this);
            EndOfDayDayReport = new EndOfDayReportService(this);
        }
    }
}
