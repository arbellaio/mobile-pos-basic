using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Resources.Colors;
using RecompildPOS.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace RecompildPOS.Helpers.Navigation
{
    public class NavigationHelper : INavigationHelper
    {
        public async Task<Page> PopAsync()
        {
           return await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async Task PopPopupAsync()
        {
             await PopupNavigation.Instance.PopAsync();
        }

        public async Task PushAsync(Page page)
        { 
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
        public async Task PushPopupAsync(PopupPage page)
        {
            await PopupNavigation.Instance.PushAsync(page);
        }

        public async Task PushModalAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public void SetMainPage(Page page)
        {
            Application.Current.MainPage = new NavigationPage(page) { 
                BarBackgroundColor = Color.FromHex(AppColors.GrapeCityPinkWhite),
                BarTextColor = Color.White
            }; ;
        }

        private Page MainPage
        {
            get { return Application.Current.MainPage; }
        }
    }

    public interface INavigationHelper
    {
        Task<Page> PopAsync();
        Task PushAsync(Page page);
        Task PushModalAsync(Page page);
        void SetMainPage(Page page);
        Task PopPopupAsync();
        Task PushPopupAsync(PopupPage page);
    }
}