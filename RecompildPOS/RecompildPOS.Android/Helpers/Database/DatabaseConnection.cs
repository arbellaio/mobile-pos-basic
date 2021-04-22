using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RecompildPOS.Database;
using RecompildPOS.Droid.Helpers.Database;
using RecompildPOS.Helpers.DatabaseExportHelper;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace RecompildPOS.Droid.Helpers.Database
{
    public class DatabaseConnection : IDatabaseConnection, IDatabaseExportHelper
    {
        public string GetDatabasePath(string dbName)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                dbName);

            return path;
        }

        public string ExportAppDatabase()
        {
            string path = "/data/user/0/com.recompild.pos/files/Recompild.db3";

            var databaseBackupPath = string.Format("{0}/{1}Recompild.db3",
                Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                DateTime.UtcNow.ToFileTimeUtc().ToString());

            if (File.Exists(path))
            {
                File.Copy(path, databaseBackupPath, true);
            }

            return databaseBackupPath;
        }
    }
}