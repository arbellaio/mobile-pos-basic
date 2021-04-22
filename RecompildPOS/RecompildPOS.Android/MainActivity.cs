using System;
using System.Net;
using Android.App;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RecompildPOS.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecompildPOS.Droid
{
    [Activity(Label = "RPos", Icon = "@drawable/recopos", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //Aurora License Key
        //bNcQaFSk0DTACeQR4c122U3r4tHerxHDLB5f+bmasjl3SyS/WIaba6DHN7Hl1VGifAc2Ws9TMgQumOydpqC1JMob2pZ5P1hUNjwdx7GarzuHBzySpRa17YO85MDf6ZvfGgvR4bS/qGt8VuRyRjagSoMUznpu+mveegASv0AA2fM=
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Aurora.ComponentLoader.Init("bNcQaFSk0DTACeQR4c122U3r4tHerxHDLB5f+bmasjl3SyS/WIaba6DHN7Hl1VGifAc2Ws9TMgQumOydpqC1JMob2pZ5P1hUNjwdx7GarzuHBzySpRa17YO85MDf6ZvfGgvR4bS/qGt8VuRyRjagSoMUznpu+mveegASv0AA2fM=");

            LoadApplication(new App());
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 198, 73, 90));

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SetupScreenSize()
        {
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
        }
    }
}