using System;
using Xamarin.Forms;

namespace RecompildPOS.Components.SketchControl
{
    public class SketchView : View, ISketchController
    {

        public string documentType { get; set; }
        public bool IsSketch { get; set; }
        public static readonly BindableProperty InkColorProperty = BindableProperty.Create("InkColor", typeof(Color), typeof(SketchView), Color.Blue);

        public event EventHandler SketchUpdated;

        public Color InkColor
        {
            get { return (Color)GetValue(InkColorProperty); }
            set { SetValue(InkColorProperty, value); }
        }

        public void Clear()
        {
            MessagingCenter.Send(this, "Clear");
            IsSketch = false;
        }
        public void CaptureImage()
        {
            MessagingCenter.Send(this, "CaptureImage");
        }

        void ISketchController.SendSketchUpdated()
        {

            if (SketchUpdated != null)
            {
                SketchUpdated(this, EventArgs.Empty);


            }

            MessagingCenter.Send(this, "SketchUpdate");
        }
        public Action<byte[]> ImageCaptured { get; set; }
    }
}

