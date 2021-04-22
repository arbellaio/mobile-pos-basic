using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace RecompildPOS.Modules.Sync
{
    public interface ISyncModule
    {
        bool IsSyncing { get; set; }
        Task SyncAllModules();
        Action SyncDone { get; set; }
    }
}
