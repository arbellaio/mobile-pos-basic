using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Database
{
    public interface IDatabaseConnection
    {
        string GetDatabasePath(string dbName);
    }
}
