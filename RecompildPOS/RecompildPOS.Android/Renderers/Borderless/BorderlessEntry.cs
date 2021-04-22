using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RecompildPOS.Components.Borderless;
using RecompildPOS.Droid.Renderers.Borderless;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(BorderlessEntryRenderer), new[] { typeof(BorderlessEntryVisual) })]
namespace RecompildPOS.Droid.Renderers.Borderless
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer(Context context) : base(context)
        {
            // empty
        }

        /// <summary>
        /// On element change method
        /// </summary>
        /// <param name="e">entry parametar</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control?.SetPadding(0, 0, 0, 0);
        }
    }
}