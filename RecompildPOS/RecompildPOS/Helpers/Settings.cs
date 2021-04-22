using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace RecompildPOS.Helpers
{
    public static class Settings
    { 
        const string TokenKey = nameof(TokenKey);
        public static string Token
        {
            get => Preferences.Get(TokenKey, string.Empty);
            set => Preferences.Set(TokenKey, value);
        }
    }
}
