using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.EndOfDayReports;

namespace RecompildPOS.Modules.EndOfDayReports
{
    public interface IEndOfDayReportModule
    {
        Task SyncEndOfDayReports();
       
    }
}