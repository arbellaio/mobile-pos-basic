using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Resources.Strings;

namespace RecompildPOS.Helpers.RandomKeyCode
{
    public class CodeGenerator
    {
        public static string AccountCodeGenerator(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var code = phoneNumber.Substring(phoneNumber.Length - 4);
                return code;
            }
            return "";
        }
    }
}
