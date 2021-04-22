using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RecompildPOS.Annotations;
using RecompildPOS.Services;
using Xamarin.Forms;

namespace RecompildPOS.Helpers.Signalr
{
     public class SigRHelper : INotifyPropertyChanged
    {
        public SigRHelper()
        {
            CreateConnection();
        }

        private HubConnection _hubConnection;
        public HubConnection HubConnection
        {
            get => _hubConnection;
            set
            {
                _hubConnection = value;
                OnPropertyChanged(nameof(HubConnection));
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        public async Task Connect()
        {
            try
            {
                await _hubConnection.StartAsync();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task Disconnect()
        {
            try
            {
                await _hubConnection.StopAsync();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CreateConnection()
        {
            var ip = WebServiceConfig.BaseUrl;
            if (Device.RuntimePlatform == Device.Android)
                ip = "10.0.2.2:5000";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{ip}/requestHub")
                .Build();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}