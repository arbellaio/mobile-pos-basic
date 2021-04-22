using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RecompildPOS.Components.CashControl
{
    public partial class CashControl : ContentView
    {
        public CashControl()
        {
            InitializeComponent();
        }

        public Action<bool> ValueChanged;
        public static BindableProperty CurrencyValueProperty = BindableProperty.Create(
            propertyName: nameof(CurrencyValue),
            returnType: typeof(double),
            declaringType: typeof(CashControl),
            defaultValue: 0.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var cashControl = (CashControl)bindable;
                if (cashControl != null)
                    cashControl.UpdateLabel();
            }
        );

        public bool IsPence
        {
            get { return (bool)GetValue(IsPenceProperty); }
            set
            {
                SetValue(IsPenceProperty, value);
            }
        }

        public static BindableProperty IsPenceProperty = BindableProperty.Create(
            propertyName: nameof(IsPence),
            returnType: typeof(bool),
            declaringType: typeof(CashControl),
            defaultValue: false,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var cashControl = (CashControl)bindable;
                if (cashControl != null)
                    cashControl.UpdateLabel();
            }
        );

        public bool IsCoin
        {
            get { return (bool)GetValue(IsCoinProperty); }
            set
            {
                SetValue(IsCoinProperty, value);
            }
        }

        public static BindableProperty IsCoinProperty = BindableProperty.Create(
            propertyName: nameof(IsCoin),
            returnType: typeof(bool),
            declaringType: typeof(CashControl),
            defaultValue: false,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var cashControl = (CashControl)bindable;
                if (cashControl != null)
                    cashControl.UpdateLabel();
            }
        );

        public bool IsCheck
        {
            get { return (bool)GetValue(IsCheckProperty); }
            set
            {
                SetValue(IsCheckProperty, value);
            }
        }

        public static BindableProperty IsCheckProperty = BindableProperty.Create(
           propertyName: nameof(IsCheck),
           returnType: typeof(bool),
           declaringType: typeof(CashControl),
           defaultValue: false,
           propertyChanged: (bindable, oldValue, newValue) =>
           {
               var cashControl = (CashControl)bindable;
               if (cashControl != null)
                   cashControl.UpdateLabel();
           }
       );

        public bool IsCredit
        {
            get { return (bool)GetValue(IsCreditProperty); }
            set
            {
                SetValue(IsCreditProperty, value);
            }
        }

        public static BindableProperty IsCreditProperty = BindableProperty.Create(
           propertyName: nameof(IsCredit),
           returnType: typeof(bool),
           declaringType: typeof(CashControl),
           defaultValue: false,
           propertyChanged: (bindable, oldValue, newValue) =>
           {
               var cashControl = (CashControl)bindable;
               if (cashControl != null)
                   cashControl.UpdateLabel();
           }
       );

        public double CurrencyValue
        {
            get { return (double)GetValue(CurrencyValueProperty); }
            set { SetValue(CurrencyValueProperty, value); }
        }

        private double amount;
        public double Amount
        {
            get { return amount; }
            private set { amount = value; }
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                Amount = 0.0;
                amountLabel.Text = (0.00).ToString();
                ValueChanged?.Invoke(true);
                return;
            }

            double n;
            bool isNumeric = double.TryParse(e.NewTextValue, out n);

            if (!isNumeric)
                return;

            if (IsPence)
                Amount = CurrencyValue * n / 100;
            else
                Amount = CurrencyValue * n;

            amountLabel.Text = Amount.ToString();
            ValueChanged?.Invoke(true);
        }

        void UpdateLabel()
        {
            if (IsPence)
                currencyLabel.Text = (CurrencyValue + "P Coins:").ToString();
            else if (IsCoin)
                currencyLabel.Text = ("£" + CurrencyValue + " Coins:").ToString();
            else if (IsCheck)
                currencyLabel.Text = ("T. Cheque Amt £:").ToString();
            else if (IsCredit)
                currencyLabel.Text = ("T. Card Amt £:").ToString();
            else
                currencyLabel.Text = ("£" + CurrencyValue + " Notes:").ToString();
        }
    }
}
