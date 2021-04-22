using System;
using System.Collections.Generic;
using RecompildPOS.Helpers.Skia;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace RecompildPOS.Components.ValueSwitch
{
    public partial class Switch : SKCanvasView
    {
        private const int _pixelMargin = 1;
        private const int _buttonMargin = 6;
        private const int _animationDuration = 250;

        private float _radius;
        private float _buttonRadius;
        private SKColor? _animatedColor = null;
        private SKPoint _buttonPosition;
        private bool _isAnimating;

        /// <summary>
        /// Attached property for <seealso cref="OffColor" />
        /// </summary>
        public static BindableProperty OffColorProperty = BindableProperty.Create(nameof(OffColor), typeof(Color), typeof(Switch), default(Color), propertyChanged: OnColorChanged);

        /// <summary>
        /// Attached property for <seealso cref="OnColor" />
        /// </summary>
        public static BindableProperty OnColorProperty = BindableProperty.Create(nameof(OnColor), typeof(Color), typeof(Switch), default(Color), propertyChanged: OnColorChanged);

        /// <summary>
        /// Attached property for <seealso cref="IsToggled" />
        /// </summary>
        public static BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(Switch), false, propertyChanged: OnToggledChanged, defaultBindingMode: BindingMode.TwoWay);

        public Switch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raised when IsToggled is changed
        /// </summary>
        public event EventHandler<ToggledEventArgs> Toggled;

        /// <summary>
        /// Gets or sets OffColorProperty
        /// </summary>
        public Color OffColor
        {
            get
            {
                return (Color)GetValue(OffColorProperty);
            }

            set
            {
                SetValue(OffColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets OnColorProperty
        /// </summary>
        public Color OnColor
        {
            get
            {
                return (Color)GetValue(OnColorProperty);
            }

            set
            {
                SetValue(OnColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IsToggledProperty
        /// </summary>
        public bool IsToggled
        {
            get
            {
                return (bool)GetValue(IsToggledProperty);
            }

            set
            {
                SetValue(IsToggledProperty, value);
            }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null)
            {
                PaintSurface += OnPaintSurface;
            }
            else
            {
                PaintSurface -= OnPaintSurface;
            }
        }

        protected virtual void Draw(SKCanvas canvas, SKImageInfo info)
        {
            canvas.Clear();

            var width = info.Width - _pixelMargin;
            var height = info.Height - _pixelMargin;

            _radius = height / 2;
            _buttonRadius = _radius - _buttonMargin;

            SKRoundRect outlineRect = new SKRoundRect(new SKRect(_pixelMargin, _pixelMargin, width, height), _radius, _radius);

            if (!_isAnimating)
            {
                _buttonPosition = new SKPoint(_radius + _pixelMargin, _radius + _pixelMargin);
                _buttonPosition.X = IsToggled ? width - _radius : _radius;
            }

            using (SKPaint outline = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2,
                Color = _animatedColor != null ? _animatedColor.Value : IsToggled ? OnColor.ToSKColor() : OffColor.ToSKColor(),
                IsAntialias = true
            },
            circleFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = _animatedColor != null ? _animatedColor.Value : IsToggled ? OnColor.ToSKColor() : OffColor.ToSKColor(),
                IsAntialias = true
            })
            {
                canvas.DrawRoundRect(outlineRect, outline);
                canvas.DrawCircle(_buttonPosition, _buttonRadius, circleFill);
            }
        }

        protected virtual void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Draw(e.Surface.Canvas, e.Info);
        }

        private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SKCanvasView skCanvasView)
            {
                skCanvasView.InvalidateSurface();
            }
        }

        private static void OnToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
            {
                ((Switch)bindable)?.Toggle();

                ((Switch)bindable)?.Toggled?.Invoke(bindable, new ToggledEventArgs((bool)newValue));
            }
        }

        private void Animate(bool isToggled)
        {
            _isAnimating = true;

            new Animation((value) =>
            {
                double colorPartWeight = 1 - value;
                _animatedColor = SkiaUtils.CalculateWeightedSKColor(isToggled ? OffColor : OnColor, isToggled ? OnColor : OffColor, colorPartWeight, value);
                PrimaryCanvas.InvalidateSurface();
            }).Commit(this, "colorAnimation", length: _animationDuration, repeat: () => false, easing:Easing.SinInOut);

            new Animation((value) =>
            {
                if(IsToggled)
                {
                    _buttonPosition.X = _radius + (float)(value * (_buttonRadius * 2));
                }
                else
                {
                    _buttonPosition.X = _radius + (_buttonRadius * 2) - (float)(value * (_buttonRadius * 2));
                }
            }).Commit(this, "positionAnimation", length: _animationDuration, repeat: () => false, finished: (v, c) => { _buttonPosition.X = _radius + (isToggled ? _buttonRadius * 2 : 0); _isAnimating = false; }, easing: Easing.SinInOut);
        }

        private void OnElementTapped(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;
        }

        private void Toggle()
        {
            Animate(IsToggled);
        }
    }
}
