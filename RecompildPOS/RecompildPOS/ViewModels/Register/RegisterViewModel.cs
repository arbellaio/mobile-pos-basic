using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using RecompildPOS.Helpers;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.ServicesModels.Register;
using RecompildPOS.Models.Users;
using RecompildPOS.Modules;
using RecompildPOS.Resources.Keys;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Register
{
    public class RegisterViewModel : BaseViewModel
    {
        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }
        
        private UserSync user;
        public UserSync User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private BusinessSync business;
        public BusinessSync Business
        {
            get { return business; }
            set
            {
                business = value;
                OnPropertyChanged(nameof(Business));
            }
        }

        public ICommand RegisterBusinessCommand => new Command(RegisterBusinessCommandLocker.Execute);
        private CommandLockerHelper RegisterBusinessCommandLocker => new CommandLockerHelper(async () =>
        {
            await Register();
        });

        public ICommand SyncBusinessCommand => new Command(SyncBusinessCommandLocker.Execute);
        private CommandLockerHelper SyncBusinessCommandLocker => new CommandLockerHelper(async () =>
        {
            await SyncBusinessUser();
        });

       

        public RegisterViewModel()
        {
//            App.SigRHelper.HubConnection.On<RegisterBusinessSync>("RegisterBusinessSignal", (registerBusiness) =>
//            {
//                App.Database.Businesses.AddUpdateBusiness(registerBusiness.Business);
//                App.Database.Users.AddUpdateUsers(registerBusiness.User);
//            });
        }
        
        private async Task Connect()
        {
            await App.SigRHelper.Connect();
            IsConnected = App.SigRHelper.IsConnected;
            var connectionId = App.SigRHelper.HubConnection.ConnectionId;
        }
        
        private async Task Register()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.NetworkState>();
            }

            var registerBusinessSync = new RegisterBusinessSync
            {
                Business = Business,
                User = User,
                SerialNo = ModulesConfig.SerialNo,
                RequestDate = DateTime.UtcNow,
            };
            // await RegisterBusinessSignal(registerBusinessSync);
            await RegisterBusinessService(registerBusinessSync);
        }

        async Task RegisterBusinessSignal(RegisterBusinessSync registerBusinessSync)
        {
            try
            {
                await Connect();
                await App.SigRHelper.HubConnection.InvokeAsync("RegisterBusinessSignal", registerBusinessSync);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task RegisterBusinessService(RegisterBusinessSync registerBusinessSync)
        {
            var registerBusiness = await App.RecompildPosService.Auth.RegisterUser(registerBusinessSync);
            if (registerBusiness?.Business != null && registerBusiness.User != null)
            {
                await App.Database.Businesses.AddUpdateBusiness(registerBusiness.Business);
                await App.Database.Users.AddUpdateUsers(registerBusiness.User);
            }
        }

        private async Task Disconnect()
        {
            await App.SigRHelper.Disconnect();
            IsConnected = App.SigRHelper.IsConnected;
        }
        private async Task SyncBusinessUser()
        {
            await App.Business.SyncBusinessesModule();
            await App.Users.SyncUsersModule();
        }
    }
}