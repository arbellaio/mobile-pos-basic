using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.Alert;
using RecompildPOS.Helpers.CommandLocker;
using RecompildPOS.Resources.Colors;
using RecompildPOS.Resources.Language;
using RecompildPOS.ViewModels.Base;
using RecompildPOS.Views;
using RecompildPOS.Views.MasterTab;
using RecompildPOS.Views.Menu;
using RecompildPOS.Views.Register;
using Xamarin.Forms;

namespace RecompildPOS.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            var item = App.GetLoginInfo();
            Username = item.Item1;
            Password = item.Item2;
        }


        public ICommand LoginCommand => new Command(LoginCommandLocker.Execute);

        private CommandLockerHelper LoginCommandLocker =>
            new CommandLockerHelper(async () => { await GoToMenuPage(); });


        public ICommand RegisterCommand => new Command(RegisterCommandLocker.Execute);

        private CommandLockerHelper RegisterCommandLocker => new CommandLockerHelper(async () =>
        {
            await GoToRegisterPage();
        });

        public ICommand CheckBoxCommand => new Command(CheckBoxCommandLocker.Execute);
        private CommandLockerHelper CheckBoxCommandLocker => new CommandLockerHelper(RememberMeClicked);

        #region Properties

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoginEnabled
        {
            get
            {
                OnPropertyChanged();
                return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            }
        }

        private bool _isChecked = false;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        #endregion


        private async Task GoToRegisterPage()
        {
            await App.NavigationService.PushAsync(new RegisterPage());
        }


        private async Task GoToMenuPage()
        {
            var user = await App.Database.Users.LoginUser(Username, Password);
            if (user != null)
            {
                var business = await App.Database.Businesses.GetBusinessByBusinessId(user.BusinessId);
                if (business != null)
                {
                    App.Business.Business = business;
                }

                App.Users.User = user;
                App.NavigationService.SetMainPage(new MasterPage());
            }
            else
            {
                var loggedInUser = await App.RecompildPosService.Auth.Login(Username, Password);
                if (loggedInUser?.User != null && loggedInUser.Business != null)
                {
                    App.Business.Business = loggedInUser.Business;
                    App.Users.User = loggedInUser.User;

                    await App.Database.Businesses.AddUpdateBusiness(loggedInUser.Business);
                    await App.Database.Users.AddUpdateUsers(loggedInUser.User);
                    App.NavigationService.SetMainPage(new MasterPage());
                }
                else
                {
                        await Alert.ShowAlert(AppResources.USERDOESNOTEXIST);
                        if (string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                        {
                            App.NavigationService.SetMainPage(new MasterPage());
                        }
                }
            }
        }


        private void RememberMeClicked()
        {
            IsChecked = !_isChecked;
            if (IsChecked)
            {
                App.SaveLoginInfo(Username, Password);
            }
        }
    }
}