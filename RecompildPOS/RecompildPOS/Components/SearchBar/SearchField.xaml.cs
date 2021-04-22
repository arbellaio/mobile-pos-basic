using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Components.SearchBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchField : ContentView
    {
        /// <summary>
        /// Attached property for <seealso cref="Keyboard" />
        /// </summary>
        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(SearchField), Keyboard.Default,
            coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);


        /// <summary>
        /// Attached property for <seealso cref="Placeholder" />
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(SearchField), string.Empty, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="PlaceholderColor" />
        /// </summary>
        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(SearchField), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="Text" />
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(SearchField), default(string), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="TextColor" />
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SearchField), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="IsTopBorderVisible" />
        /// </summary>
        public static readonly BindableProperty IsTopBorderVisibleProperty =
            BindableProperty.Create(nameof(IsTopBorderVisible), typeof(bool), typeof(SearchField), false, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="IsBottomBorderVisible" />
        /// </summary>
        public static readonly BindableProperty IsBottomBorderVisibleProperty =
            BindableProperty.Create(nameof(IsBottomBorderVisible), typeof(bool), typeof(SearchField), false, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="BorderColor" />
        /// </summary>
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SearchField), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="EntryBackgroundColor" />
        /// </summary>
        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(SearchField), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="SearchCommand" />
        /// </summary>
        public static readonly BindableProperty SearchCommandProperty =
            BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(SearchField), default(ICommand), BindingMode.TwoWay);


        /// <summary>
        /// Attached property for <seealso cref="CancelIconCommand" />
        /// </summary>
        public static readonly BindableProperty CancelIconCommandProperty =
            BindableProperty.Create(nameof(CancelIconCommand), typeof(ICommand), typeof(SearchField), default(ICommand), BindingMode.TwoWay);


        public SearchField()
        {
            InitializeComponent();
            CancelIconCommand = new Command(ClearText);
        }

        /// <summary>
        /// Gets or sets Keyboard
        /// </summary>
        public Keyboard Keyboard
        {
            get
            {
                return (Keyboard)GetValue(KeyboardProperty);
            }

            set
            {
                SetValue(KeyboardProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets Text
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TextColor
        /// </summary>
        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }

            set
            {
                SetValue(TextColorProperty, value);
            }
        }



        /// <summary>
        /// Gets or sets Placeholder
        /// </summary>
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets PlaceholderColor
        /// </summary>
        public Color PlaceholderColor
        {
            get
            {
                return (Color)GetValue(PlaceholderColorProperty);
            }

            set
            {
                SetValue(PlaceholderColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IsTopBorderVisible
        /// </summary>
        public bool IsTopBorderVisible
        {
            get
            {
                return (bool)GetValue(IsTopBorderVisibleProperty);
            }

            set
            {
                SetValue(IsTopBorderVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IsBottomBorderVisible
        /// </summary>
        public bool IsBottomBorderVisible
        {
            get
            {
                return (bool)GetValue(IsBottomBorderVisibleProperty);
            }

            set
            {
                SetValue(IsBottomBorderVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets BorderColor
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return (Color)GetValue(BorderColorProperty);
            }

            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets EntryBackgroundColor
        /// </summary>
        public Color EntryBackgroundColor
        {
            get
            {
                return (Color)GetValue(EntryBackgroundColorProperty);
            }

            set
            {
                SetValue(EntryBackgroundColorProperty, value);
            }
        }




        /// <summary>
        /// Gets or sets SearchCommandProperty
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                return (ICommand)GetValue(SearchCommandProperty);
            }

            set
            {
                SetValue(SearchCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets CancelIconCommand
        /// </summary>
        public ICommand CancelIconCommand
        {
            get
            {
                return (ICommand)GetValue(CancelIconCommandProperty);
            }
            set
            {
                SetValue(CancelIconCommandProperty, value);
            }
        }



        void OnTapped(object sender, EventArgs args)
        {
            if (!Entry.IsFocused)
            {
                Entry.Focus();
            }
        }

        private void Text_Change(object sender, TextChangedEventArgs e)
        {
            SearchCommand?.Execute(e.NewTextValue);
        }

        private void ClearText()
        {
            Entry.Text = string.Empty;
        }
    }
}