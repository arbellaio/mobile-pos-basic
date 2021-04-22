using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace RecompildPOS.Helpers.Skia
{
    public static class SkiaUtils
    {
        public static SKColor CalculateWeightedSKColor(Xamarin.Forms.Color from, Xamarin.Forms.Color to, double weightA, double weightB)
        {
            double r = (from.R * weightA) + (to.R * weightB);
            double g = (from.G * weightA) + (to.G * weightB);
            double b = (from.B * weightA) + (to.B * weightB);
            double a = (from.A * weightA) + (to.A * weightB);

            byte bR = (byte)Math.Max(0, Math.Min(255, (int)Math.Floor(r * 256.0)));
            byte bG = (byte)Math.Max(0, Math.Min(255, (int)Math.Floor(g * 256.0)));
            byte bB = (byte)Math.Max(0, Math.Min(255, (int)Math.Floor(b * 256.0)));
            byte bA = (byte)Math.Max(0, Math.Min(255, (int)Math.Floor(a * 256.0)));

            return new SKColor(bR, bG, bB, bA);
        }
    }
}
