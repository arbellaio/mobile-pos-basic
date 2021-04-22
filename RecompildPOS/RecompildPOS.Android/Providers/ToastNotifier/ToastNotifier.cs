using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RecompildPOS.Droid.Providers.ToastNotifier;
using RecompildPOS.Providers.ToastNotifier;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ToastNotifierProvider))]
namespace RecompildPOS.Droid.Providers.ToastNotifier
{
    public class ToastNotifierProvider : IToastNotifier
    {
        public Task<bool> Notify(string title, string description, TimeSpan duration, object context = null, bool showOnTop = true)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var toast = Toast.MakeText(Android.App.Application.Context, description, ToastLength.Short);
            if (showOnTop)
                toast.SetGravity(GravityFlags.Top, 0, 0);
            toast.Show();
            return taskCompletionSource.Task;
        }

        public void HideAll()
        {
        }
    }
}