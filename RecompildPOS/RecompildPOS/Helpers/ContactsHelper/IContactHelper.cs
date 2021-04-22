using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Models.Accounts;

namespace RecompildPOS.Helpers.ContactsHelper
{
    public interface IContactHelper
    {
        Task<List<Account>> GetDeviceContactsAsync();
    }
}
