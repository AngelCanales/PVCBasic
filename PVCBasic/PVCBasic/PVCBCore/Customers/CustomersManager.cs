using Microsoft.EntityFrameworkCore;
using PVCBasic.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Customers
{
    public class CustomersManager : ICustomersManager
    {
        private readonly IRepository<PVCBasic.Database.Models.Customers> customers;

        public CustomersManager(IRepository<PVCBasic.Database.Models.Customers> customers)
        {
            this.customers = customers;
        }

        public async Task CreateAsync(Database.Models.Customers ncustomers)
        {
            this.customers.Create(ncustomers);
            await this.customers.SaveChangesAsync();
        }

        public async Task DeleteAsync(Database.Models.Customers ncustomers)
        {
            
            this.customers.Delete(ncustomers);
            await this.customers.SaveChangesAsync();
        }

        public async Task EditAsync(Database.Models.Customers ncustomers)
        {
            this.customers.Update(ncustomers);
            await this.customers.SaveChangesAsync();
        }

        public async Task<Database.Models.Customers> FindByIdAsync(int id)
        {
            var ncustomers = await this.customers.All().FirstOrDefaultAsync( w => w.Id == id);
            return ncustomers; 
        }

        public async Task<IEnumerable<Database.Models.Customers>> GetAllAsync()
        {
            var ncustomers = await this.customers.All().ToListAsync();
            return ncustomers;
        }
    }
}
