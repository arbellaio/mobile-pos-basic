using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using RecompildPOS.Views;
using UIKit;

namespace RecompildPOS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        //Aurora License Key
        // bNcQaFSk0DTACeQR4c122U3r4tHerxHDLB5f+bmasjl3SyS/WIaba6DHN7Hl1VGifAc2Ws9TMgQumOydpqC1JMob2pZ5P1hUNjwdx7GarzuHBzySpRa17YO85MDf6ZvfGgvR4bS/qGt8VuRyRjagSoMUznpu+mveegASv0AA2fM=
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
