using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Helpers.Audio;
using RecompildPOS.Resources.Language;
using RecompildPOS.Views;
using Xamarin.Forms;

namespace RecompildPOS.Helpers.Alert
{
    public class Alert
    {
        //todo will have to add translation for alert box options

        public static async Task ShowAlert(string alertText, string noBtnText = "Cancel", string alertHeading = "Alert")
        {
            await Application.Current.MainPage.DisplayAlert(alertHeading, alertText, noBtnText);
        }

        public static async Task<bool> ShowChoiceAlert(string alertText, string yesBtnText = "Ok", string noBtnText = "Cancel", string alertHeading = "Alert")
        {
            return await Application.Current.MainPage.DisplayAlert(alertHeading, alertText, yesBtnText, noBtnText);
        }


        public static async Task ShowAlertSound(string alertText, string noBtnText = "Cancel", string alertHeading = "Alert")
        {
            AudioHelper.PlayBeep(); 
            await Application.Current.MainPage.DisplayAlert(alertHeading, alertText, noBtnText);
        }

        public static async Task<bool> ShowChoiceAlertSound(string alertText, string yesBtnText = "Ok", string noBtnText = "Cancel", string alertHeading = "Alert")
        {
            AudioHelper.PlayBeep();
            return await Application.Current.MainPage.DisplayAlert(alertHeading, alertText, yesBtnText, noBtnText);
        }

      

    }
}