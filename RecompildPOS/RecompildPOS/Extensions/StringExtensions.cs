using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Providers.ToastNotifier;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecompildPOS.Extensions
{
    public static partial class StringExtensions
    {
        public static void ToToast(this string message, string title = null, bool showOnTop = false)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var toaster = DependencyService.Get<IToastNotifier>();
                toaster?.Notify(title, message, TimeSpan.FromMilliseconds(300), showOnTop: showOnTop);
            });
        }
    }
}