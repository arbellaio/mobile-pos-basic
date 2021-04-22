using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Modules
{
    public static class ModulesConfig
    {
//        public static string deviceIMEI => DependencyService.Get<IDeviceIMEI>().GetIdentifier();
        public static string SerialNo = "357579080015581";//deviceIMEI; WH 355046080153780 VS 357579080015581
        public static DateTime SyncDate = new DateTime(2000, 01, 01);
        public static int SyncTime = 2; //2 minutes.
        public static int SyncInterval = 3; //3 minutes.
    }
}
