using Microsoft.EntityFrameworkCore;
using PVCBasic.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Products
{
    public class ProductsManager : IProductsManager
    {
        private readonly IRepository<PVCBasic.Database.Models.Products> products;

        public ProductsManager(IRepository<PVCBasic.Database.Models.Products> products)
        {
            this.products = products;
        }

        public async Task CreateAsync(Database.Models.Products products)
        {
            this.products.Create(products);
            await this.products.SaveChangesAsync();
        }

        public async Task DeleteAsync(Database.Models.Products products)
        {
            
            this.products.Delete(products);
            await this.products.SaveChangesAsync();
        }

        public async Task EditAsync(Database.Models.Products products)
        {
            this.products.Update(products);
            await this.products.SaveChangesAsync();
        }

        public async Task<Database.Models.Products> FindByIdAsync(int id)
        {
            var products = await this.products.All().FirstOrDefaultAsync( w => w.Id == id);
            return products; 
        }

        public async Task<IEnumerable<Database.Models.Products>> GetAllAsync()
        {
            var products = await this.products.All().ToListAsync();
            return products;
        }
    }
}
