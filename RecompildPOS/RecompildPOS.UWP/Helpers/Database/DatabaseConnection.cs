using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using RecompildPOS.Database;
using RecompildPOS.Helpers.DatabaseExportHelper;
using RecompildPOS.UWP.Helpers.Database;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace RecompildPOS.UWP.Helpers.Database
{
    public class DatabaseConnection : IDatabaseConnection, IDatabaseExportHelper
    {
        public string GetDatabasePath(string dbName)
        {
           
            var path = Path.Combine(ApplicationData.
                    Current.LocalFolder.Path,
                dbName);
            return path;
        }

        public string ExportAppDatabase()
        {
            string path = "/data/user/0/com.recompild.pos/files/Recompild.db3";

//            var databaseBackupPath = string.Format("{0}/{1}Recompild.db3",
//                Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
//                DateTime.UtcNow.ToFileTimeUtc().ToString());

//            if (File.Exists(path))
//            {
//                File.Copy(path, databaseBackupPath, true);
//            }

//            return databaseBackupPath ;
            return  "";
        }
    }

}
