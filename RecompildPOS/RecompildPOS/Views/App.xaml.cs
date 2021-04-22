using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using RecompildPOS.Database.Accounts;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Database.Sync;
using RecompildPOS.Helpers.Navigation;
using RecompildPOS.Helpers.Signalr;
using RecompildPOS.Modules;
using RecompildPOS.Modules.Accounts;
using RecompildPOS.Modules.AccountTransactions;
using RecompildPOS.Modules.Base;
using RecompildPOS.Modules.Businesses;
using RecompildPOS.Modules.BusinessFinances;
using RecompildPOS.Modules.EndOfDayReports;
using RecompildPOS.Modules.OrderProcesses;
using RecompildPOS.Modules.Orders;
using RecompildPOS.Modules.Products;
using RecompildPOS.Modules.Users;
using RecompildPOS.Resources.Colors;
using RecompildPOS.Resources.Keys;
using RecompildPOS.Services.WebService;
using RecompildPOS.Views.Login;
using RecompildPOS.Views.Menu;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

[assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Regular")]
[assembly: ExportFont("Montserrat-Black.ttf", Alias = "Black")]
[assembly: ExportFont("Montserrat-Bold.ttf", Alias = "Bold")]
[assembly: ExportFont("Montserrat-Light.ttf", Alias = "Light")]
[assembly: ExportFont("FontAwesome5Light.otf", Alias = "FontAwesomeLight")]
[assembly: ExportFont("FontAwesome5Regular.otf", Alias = "FontAwesomeRegular")]
[assembly: ExportFont("FontAwesome5Solid.otf", Alias = "FontAwesomeSolid")]
namespace RecompildPOS.Views
{

    public partial class App : Application
    {
        public static bool AutoSync
        {
            get; set;
        } = true;
        public static IBaseModule Base { get; private set; }
        public static IUserModule Users { get; private set; }
        public static IAccountModule Accounts { get; private set; }
        public static IBusinessModule Business { get; private set; }
        public static IBusinessFinanceModule BusinessFinance { get; private set; }
        public static IProductsModule Products { get; private set; }
        public static IOrderProcessesModule OrderProcesses { get; private set; }
        public static IOrdersModule Orders { get; private set; }
        public static IEndOfDayReportModule EndOfDayReports { get; private set; }
        public static IAccountTransactionModule AccountTransactionModules { get; private set; }
        public static IBusinessFinanceExpenseModule BusinessFinanceExpense { get; private set; }


        public static ILocalDatabase Database { get; private set; }
        public static IRecompildPOSService RecompildPosService { get; private set; }
        public static SigRHelper SigRHelper { get; private set; }

        private bool _isDisconnectedStackVisible = false;
        public bool IsDisconnectedStackVisible
        {
            get => _isDisconnectedStackVisible;
            set
            {
                _isDisconnectedStackVisible = value;
                OnPropertyChanged();
            }
        }

        //For Background table sync
        private static Stopwatch stopWatch = new Stopwatch();
        public static int BackgroundTaskTime = ModulesConfig.SyncTime;

        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }

        public static INavigationHelper NavigationService { get; private set; }
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "CarouselView_Experimental", "Expander_Experimental", "SwipeView_Experimental" });

            Initialize();
            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromHex(AppColors.GrapeCityPinkWhite),
                BarTextColor = Color.White
            };
        }

        private static void Initialize()
        {
            RecompildPosService = new RecompildPOSService();
//            SigRHelper = new SigRHelper();

            Database = new LocalDatabase();
            Base = new BaseModule();
            Users = new UserModule();
            Accounts = new AccountModule();
            Business = new BusinessModule();
            BusinessFinance = new BusinessFinanceModule();
            BusinessFinanceExpense = new BusinessFinanceExpenseModule();
            Products = new ProductsModule();
            OrderProcesses = new OrderProcessesModule();
            Orders = new OrdersModule();
            EndOfDayReports = new EndOfDayReportModule();
            AccountTransactionModules = new AccountTransactionModule();

            Database.OpenConnection();

            NavigationService = new NavigationHelper();
        }
        protected override void OnStart()
        {
//            AppCenter.Start("android={508b8ae6-7fa0-4486-b07a-ef20d8d59131}", typeof(Analytics), typeof(Crashes));
            AppCenter.Start("508b8ae6-7fa0-4486-b07a-ef20d8d59131",
                typeof(Analytics), typeof(Crashes));

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static void ToggleAutoSync(bool autoSync)
        {
            if (!autoSync)
                stopWatch.Reset();
            else
            {
                if (!stopWatch.IsRunning)
                    stopWatch.Start();
            }

            Preferences.Set(AppKeys.AutoSync, autoSync);
            AutoSync = autoSync;
        }


        public static void SaveLoginInfo(string username, string password)
        {
            Preferences.Set(AppKeys.Username, username);
            Preferences.Set(AppKeys.Password, password);
        }

        public static (string,string) GetLoginInfo()
        {
            var username = Preferences.Get(AppKeys.Username,String.Empty);
            var password = Preferences.Get(AppKeys.Password, String.Empty);
            return (username, password);
        }
    }
}
