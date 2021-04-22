using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Components.Borderless;
using RecompildPOS.UWP.Renderers.Borderless;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Thickness = Windows.UI.Xaml.Thickness;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(BorderlessEntryRenderer), new[] { typeof(BorderlessEntryVisual) })]
namespace RecompildPOS.UWP.Renderers.Borderless
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Thickness(0);
            }
        }
    }
}
