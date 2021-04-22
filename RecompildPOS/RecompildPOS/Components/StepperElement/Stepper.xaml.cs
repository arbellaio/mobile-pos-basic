using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Components.StepperElement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stepper : ContentView
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(Stepper), 0.0, BindingMode.TwoWay, coerceValue: (bindable, value) =>
        {
            var stepper = (Stepper)bindable;
            return ((double)value).Clamp(stepper.Minimum, stepper.Maximum);
        }, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var stepper = (Stepper)bindable;
            EventHandler<ValueChangedEventArgs> eh = stepper.ValueChanged;
            if (eh != null)
                eh(stepper, new ValueChangedEventArgs((double)oldValue, (double)newValue));
            stepper.UpdateCount();
        });

        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(Stepper), 100000000.0, validateValue: (bindable, value) =>
        {
            var stepper = (Stepper)bindable;
            return (double)value > stepper.Minimum;
        }, coerceValue: (bindable, value) =>
        {
            var stepper = (Stepper)bindable;
            stepper.Value = stepper.Value.Clamp(stepper.Minimum, (double)value);
            return value;
        });

        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(Stepper), 0.0, validateValue: (bindable, value) =>
        {
            var stepper = (Stepper)bindable;
            return (double)value < stepper.Maximum;
        }, coerceValue: (bindable, value) =>
        {
            var stepper = (Stepper)bindable;
            stepper.Value = stepper.Value.Clamp((double)value, stepper.Maximum);
            return value;
        });

        public static readonly BindableProperty IncrementProperty = BindableProperty.Create(nameof(Increment), typeof(double), typeof(Stepper), 1.0);


        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }


        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public Stepper()
        {
            InitializeComponent();
            UpdateCount();
        }

        void Decrease_Clicked(object sender, EventArgs e)
        {
            Value = Value - Increment;
        }

        private void Increase_Clicked(object sender, EventArgs e)
        {
            Value = Value + Increment;
        }

        void UpdateCount()
        {
            count.Text = Value.ToString();
        }

        private void Count_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double result = 0.0;
            if (!string.IsNullOrEmpty(count.Text))
            {
                if (!Double.TryParse(count.Text, System.Globalization.NumberStyles.AllowDecimalPoint, null, out result))
                {
                    Value = result;
                }
                else
                {
                    Value = Double.Parse(count.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
                }


            }
            else
                Value = 0;
        }
    }
}