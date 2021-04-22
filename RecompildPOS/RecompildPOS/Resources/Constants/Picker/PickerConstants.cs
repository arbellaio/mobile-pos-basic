using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Resources.Constants.Picker
{
    public static class PickerConstants
    {
        public static List<string> BusinessType = new List<string>
        {
            "Sole Proprietorship",
            "Partnership",
            "Corporation",
            "Limited Liability Company"
        };

        public static List<string> BusinessCategory = new List<string>
        {
            "Retailer",
            "Health Practitioner",
            "Distributor (Finish Goods)",
            "Food Service",
            "Supplier / Raw Ingredient (Distributor)",
            "Manufacturer",
        };

        public static int[] MinutesList = new int[] { 1, 2, 3, 4, 5, 10, 15, 30, 45, 60 };


    }
}
