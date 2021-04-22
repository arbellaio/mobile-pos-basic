using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Extensions;
using RecompildPOS.Modules;
using RecompildPOS.Resources.Language;
using RecompildPOS.Views;

namespace RecompildPOS.Helpers.Connection
{
    public class ConnectionHelper
    {
        public static async Task<bool> IsConnected()
        {
            var status = await App.RecompildPosService.ServerPing.CheckPortConnection(ModulesConfig.SerialNo);
            switch (status)
            {
                case Services.ServerPing.ServerStatusEnum.Ok:
                    return true;
                case Services.ServerPing.ServerStatusEnum.Unauthorized:
                    AppResources.CONNECTION_HELPER_CHECK_PERMISSIONS.ToToast();
                    return false;
                case Services.ServerPing.ServerStatusEnum.TimeOut:
                    AppResources.CONNECTION_HELPER_UNABLE_TO_CONNECT_SERVER.ToToast();
                    return false;
            }
            return true;
        }
    }
}
