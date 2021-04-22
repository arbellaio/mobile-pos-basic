using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Models.Selectable;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Components.ItemPicker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPicker : ContentView
    {
        /// <summary>
        /// Attached property for <seealso cref="Keyboard" />
        /// </summary>
        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(ItemPicker), Keyboard.Default,
            coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);

        /// <summary>
        /// Attached property for <seealso cref="Errors" />
        /// </summary>
        public static readonly BindableProperty ErrorsProperty =
            BindableProperty.Create(nameof(Errors), typeof(List<string>), typeof(ItemPicker), new List<string>(), BindingMode.OneWay);

        /// <summary>
        /// Attached property for <seealso cref="IsValid" />
        /// </summary>
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ItemPicker), true, BindingMode.OneWay);

        /// <summary>
        /// Attached property for <seealso cref="IsDirty" />
        /// </summary>
        public static readonly BindableProperty IsDirtyProperty =
            BindableProperty.Create(nameof(IsDirty), typeof(bool), typeof(ItemPicker), false, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="IsPassword" />
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(ItemPicker), false, BindingMode.OneWay);

        /// <summary>
        /// Attached property for <seealso cref="Placeholder" />
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ItemPicker), string.Empty, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="PlaceholderColor" />
        /// </summary>
        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ItemPicker), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="Text" />
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ItemPicker), default(string), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="TextColor" />
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ItemPicker), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="IsTopBorderVisibleWhenValid" />
        /// </summary>
        public static readonly BindableProperty IsTopBorderVisibleWhenValidProperty =
            BindableProperty.Create(nameof(IsTopBorderVisibleWhenValid), typeof(bool), typeof(ItemPicker), false, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="IsBottomBorderVisibleWhenValid" />
        /// </summary>
        public static readonly BindableProperty IsBottomBorderVisibleWhenValidProperty =
            BindableProperty.Create(nameof(IsBottomBorderVisibleWhenValid), typeof(bool), typeof(ItemPicker), false, BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="BorderColor" />
        /// </summary>
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ItemPicker), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="EntryBackgroundColor" />
        /// </summary>
        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(ItemPicker), default(Color), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="TrailingIcon" />
        /// </summary>
        public static readonly BindableProperty TrailingIconProperty =
            BindableProperty.Create(nameof(TrailingIcon), typeof(ImageSource), typeof(ItemPicker), default(ImageSource), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="TrailingIconCommand" />
        /// </summary>
        public static readonly BindableProperty TrailingIconCommandProperty =
            BindableProperty.Create(nameof(TrailingIconCommand), typeof(ICommand), typeof(ItemPicker), default(ICommand), BindingMode.TwoWay);



        /// <summary>
        /// Attached property for <seealso cref="EntryIcon" />
        /// </summary>
        public static readonly BindableProperty EntryIconProperty =
            BindableProperty.Create(nameof(EntryIcon), typeof(ImageSource), typeof(ItemPicker), default(ImageSource), BindingMode.TwoWay);

        /// <summary>
        /// Attached property for <seealso cref="ItemsSource" />
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(Picker), default(IEnumerable));

         /// <summary>
        /// Attached property for <seealso cref="SelectedItem" />
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(Picker), default(string));

        /// <summary>
        /// Attached property for <seealso cref="SelectedItemIndex" />
        /// </summary>
        public static readonly BindableProperty SelectedItemIndexProperty =
             BindableProperty.Create(nameof(SelectedItemIndex), typeof(int), typeof(Picker), default(int));

        public ItemPicker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets ErrorsProperty
        /// </summary>
        public List<string> Errors
        {
            get
            {
                return (List<string>)GetValue(ErrorsProperty);
            }

            set
            {
                SetValue(ErrorsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IsValidProperty
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }

            set
            {
                SetValue(IsValidProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IsPasswordProperty
        /// </summary>
        public bool IsPassword
        {
            get
            {
                return (bool)GetValue(IsPasswordProperty);
            }

            set
            {
                SetValue(IsPasswordProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets KeyboardProperty
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
        /// Gets or sets TextProperty
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
        /// Gets or sets TextColorProperty
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
        /// Gets or sets IsDirty
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return (bool)GetValue(IsDirtyProperty);
            }

            set
            {
                SetValue(IsDirtyProperty, value);
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
        /// Gets or sets PlaceholderColorProperty
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
        /// Gets or sets IsTopBorderVisibleWhenValidProperty
        /// </summary>
        public bool IsTopBorderVisibleWhenValid
        {
            get
            {
                return (bool)GetValue(IsTopBorderVisibleWhenValidProperty);
            }

            set
            {
                SetValue(IsTopBorderVisibleWhenValidProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IsBottomBorderVisibleWhenValidProperty
        /// </summary>
        public bool IsBottomBorderVisibleWhenValid
        {
            get
            {
                return (bool)GetValue(IsBottomBorderVisibleWhenValidProperty);
            }

            set
            {
                SetValue(IsBottomBorderVisibleWhenValidProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets BorderColorProperty
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
        /// Gets or sets EntryBackgroundColorProperty
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
        /// Gets or sets TrailingIconProperty
        /// </summary>
        public ImageSource TrailingIcon
        {
            get
            {
                return (ImageSource)GetValue(TrailingIconProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets TrailingIconCommandPropertys
        /// </summary>
        public ICommand TrailingIconCommand
        {
            get
            {
                return (ICommand)GetValue(TrailingIconCommandProperty);
            }

            set
            {
                SetValue(TrailingIconCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrailingIconProperty
        /// </summary>
        public ImageSource EntryIcon
        {
            get
            {
                return (ImageSource)GetValue(EntryIconProperty);
            }

            set
            {
                SetValue(EntryIconProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ItemsSource
        /// </summary>
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets SelectedItem
        /// </summary>
        public string SelectedItem
        {
            get
            {
                return (string)GetValue(SelectedItemProperty);
            }

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets SelectedItemIndex
        /// </summary>
        public int SelectedItemIndex
        {
            get
            {
                return (int)GetValue(SelectedItemIndexProperty);
            }

            set
            {
                SetValue(SelectedItemIndexProperty, value);
            }
        }


    }
}