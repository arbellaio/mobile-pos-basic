using System;
using System.Collections.Generic;
using System.Text;
using RecompildPOS.Database.DatabaseHandler;

namespace RecompildPOS.Database.GenericDatabase
{
    public class BaseTable<T>
    {
        public LocalDatabase Handler { get; private set; }
        public BaseTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

       
    }
}
