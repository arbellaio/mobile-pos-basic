using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using RecompildPOS.Models.Accounts;

namespace RecompildPOS.Helpers.DummyData
{
    public class DummyDataGenerator
    {
        public static ObservableCollection<AccountSync> GetAllAccount()
        {
            var accounts = new ObservableCollection<AccountSync>
            {
                new AccountSync
                {
                    Name = "Ahmed",
                    PhoneNumber = "03365576946"
                },
                new AccountSync
                {
                    Name = "Umer",
                    PhoneNumber = "03365576946"
                },
                new AccountSync
                {
                    Name = "Ali",
                    PhoneNumber = "03365576946"
                },
                new AccountSync
                {
                    Name = "Faizan",
                    PhoneNumber = "03365576946"
                },
                new AccountSync
                {
                    Name = "Irtaza",
                    PhoneNumber = "03365576946"
                },
                new AccountSync
                {
                    Name = "Shahroz",
                    PhoneNumber = "03365576946"
                },
                new AccountSync
                {
                    Name = "John",
                    PhoneNumber = "03365576946"
                },
            };
            return accounts;
        }
    }
}
