using Android.Provider;
using RecompildPOS.Droid.Helpers.ContactsHelper;
using RecompildPOS.Helpers.ContactsHelper;
using RecompildPOS.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(ContactHelper))]
namespace RecompildPOS.Droid.Helpers.ContactsHelper
{
    public class ContactHelper : IContactHelper
    {
        public async Task<List<Account>> GetDeviceContactsAsync()
        {
            List<Account> contactList = new List<Account>();
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            string[] projection = { ContactsContract.Contacts.InterfaceConsts.Id, ContactsContract.Contacts.InterfaceConsts.DisplayName, ContactsContract.CommonDataKinds.Phone.Number};
            await Task.Run((() =>
            {
                var cursor = Application.Context.ContentResolver.Query(uri, projection, null, null, null);
                if (cursor.MoveToFirst())
                {
                    do
                    {
                        contactList.Add(new Account()
                        {
                            Name = cursor.GetString(cursor.GetColumnIndex(projection[1])),
                            PhoneNumber = cursor.GetString(cursor.GetColumnIndex(projection[2])),
                        });
                    } while (cursor.MoveToNext());
                }
            }));
            
            return contactList;
        }
        private object ManagedQuery(Android.Net.Uri uri, string[] projection, object p1, object p2, object p3)
        {
            throw new NotImplementedException();
        }
    }
}