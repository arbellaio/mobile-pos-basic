using System.Collections.Generic;
using System.Threading.Tasks;
using RecompildPOS.Models.Products;

namespace RecompildPOS.Modules.Products
{
    public interface IProductsModule
    {
        Task SyncProducts();
    }
}