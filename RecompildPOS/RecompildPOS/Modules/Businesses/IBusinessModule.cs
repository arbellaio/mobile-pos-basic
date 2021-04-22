using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.Sync;
using RecompildPOS.Models.Users;

namespace RecompildPOS.Modules.Businesses
{
    public interface IBusinessModule
    { 
        BusinessSync Business { get; set; }
        Task SyncBusinessesModule();
    }
}