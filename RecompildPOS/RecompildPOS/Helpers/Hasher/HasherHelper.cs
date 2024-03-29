﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RecompildPOS.Helpers.Hasher
{
    public class HasherHelper
    {
        //Method that Hashes Password
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        // Method that Verifies Password Hash
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            try
            {
                byte[] src = Convert.FromBase64String(hashedPassword);
                if ((src.Length != 0x31) || (src[0] != 0))
                {
                    return false;
                }

                byte[] dst = new byte[0x10];
                Buffer.BlockCopy(src, 1, dst, 0, 0x10);
                byte[] buffer3 = new byte[0x20];
                Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
                using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
                {
                    buffer4 = bytes.GetBytes(0x20);
                }

                if (ByteArraysEqual(buffer3, buffer4))
                {
                    Console.WriteLine("Password Matches");
                    return true;
                }

                Console.WriteLine("Password Not Verified");
                return false;
            }
            catch (FormatException)
            {
                Console.WriteLine("Format Exception");
                throw new FormatException("Format Exception");
            }
        }

        // Method that compares two byte arrays
        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }

            return true;
        }
    }
}
