using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Models.Products;

namespace RecompildPOS.Database.Product
{
    public class ProductsTable : IProductsTable
    {
        public LocalDatabase Handler { get; private set; }
        public ProductsTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }

        public async Task AddUpdateProducts(List<ProductSync> products)
        {
            if (products != null && products.Any())
            {
                foreach (var product in products)
                {
                    await AddUpdateProducts(product);
                }
            }
        }

        public async Task AddUpdateProducts(ProductSync product)
        {
            if (product != null)
            {
                var productInDb = await GetProductById(product.ProductId);
                if (productInDb == null)
                {
                    if (!product.IsDeleted)
                    {
                        await Handler.Database.InsertAsync(product);
                    }
                }
                else
                {
                    if (product.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(product);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(product);
                    }
                }
            }
        }

        public async Task AddUpdateProducts(Models.Products.Product product)
        {
            if (product != null)
            {
                var productInDb = await Handler.Database.Table<Models.Products.Product>()
                    .Where(x => x.ProductId.Equals(product.ProductId)).FirstOrDefaultAsync();
                if (productInDb == null)
                {
                    await Handler.Database.InsertAsync(product);
                }
                else
                {
                    if (product.IsDeleted)
                    {
                        await Handler.Database.DeleteAsync(productInDb);
                    }
                    else
                    {
                        await Handler.Database.UpdateAsync(product);
                    }
                }
            }
        }

        public async Task<ProductSync> GetProductById(int id)
        {
            return await Handler.Database.Table<ProductSync>().Where(x => x.ProductId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<ProductSync> GetProductBySkuCode(string skuCode)
        {
            return await Handler.Database.Table<ProductSync>().Where(x => x.SkuCode.Equals(skuCode)).FirstOrDefaultAsync();
        }

        public async Task<ProductSync> GetProductByBarCode(string barCode)
        {
            return await Handler.Database.Table<ProductSync>().Where(x => x.BarCode1.Equals(barCode)).FirstOrDefaultAsync();
        }

        public async Task<ProductSync> GetProductByBarCode2(string barCode2)
        {
            return await Handler.Database.Table<ProductSync>().Where(x => x.BarCode2.Equals(barCode2)).FirstOrDefaultAsync();
        }

        public async Task<ProductSync> GetProductByQrCode(string qrCode)
        {
            return await Handler.Database.Table<ProductSync>().Where(x => x.QrCode.Equals(qrCode)).FirstOrDefaultAsync();
        }

        public async Task<List<ProductSync>> GetProductByCategoryId(int categoryId)
        {
            return await Handler.Database.Table<ProductSync>().Where(x => x.CategoryId.Equals(categoryId)).ToListAsync();
        }

        public async Task<List<Models.Products.Product>> GetAllUnSyncedProducts()
        {
            return await Handler.Database.Table<Models.Products.Product>().Where(x => x.IsPost && !x.IsSynced)
                .ToListAsync();
        }

        public async Task<List<ProductSync>> GetAllProducts()
        {
            return await Handler.Database.Table<ProductSync>().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task UpdateProduct(Models.Products.Product product)
        {
            if (product != null)
            {
                await Handler.Database.UpdateAsync(product);
            }
        }
    }

    public interface IProductsTable
    {
        Task AddUpdateProducts(List<ProductSync> product);
        Task AddUpdateProducts(ProductSync product);
        Task AddUpdateProducts(Models.Products.Product product);
        Task<ProductSync> GetProductById(int id);
        Task<ProductSync> GetProductBySkuCode(string skuCode);
        Task<ProductSync> GetProductByBarCode(string barCode);
        Task<ProductSync> GetProductByBarCode2(string barCode2);
        Task<ProductSync> GetProductByQrCode(string qrCode);
        Task<List<ProductSync>> GetProductByCategoryId(int categoryId);

        Task<List<Models.Products.Product>> GetAllUnSyncedProducts();
        Task<List<ProductSync>> GetAllProducts();
        Task UpdateProduct(Models.Products.Product product);
    }
}
