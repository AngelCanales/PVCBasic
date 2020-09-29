using Microsoft.EntityFrameworkCore;
using PVCBasic.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Inventories
{
    public class InventoriesManager : IInventoriesManager
    {
        private readonly IRepository<PVCBasic.Database.Models.Inventories> inventories;

        public InventoriesManager(IRepository<PVCBasic.Database.Models.Inventories> inventories)
        {
            this.inventories = inventories;
        }

        public async Task CreateAsync(Database.Models.Inventories inventories)
        {
            this.inventories.Create(inventories);
            await this.inventories.SaveChangesAsync();
        }

        public async Task DeleteAsync(Database.Models.Inventories inventories)
        {
            
            this.inventories.Delete(inventories);
            await this.inventories.SaveChangesAsync();
        }

        public async Task EditAsync(Database.Models.Inventories inventories)
        {
            try
            {
                this.inventories.Update(inventories);
                await this.inventories.SaveChangesAsync();
               
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<Database.Models.Inventories> FindByIdAsync(int id)
        {
            var inventories = await this.inventories.All().Where( w => w.IdProduct == id).FirstOrDefaultAsync();
            return inventories; 
        }

        public async Task<IEnumerable<Database.Models.Inventories>> GetAllAsync()
        {
            var inventories = await this.inventories.All().ToListAsync();
            return inventories;
        }
    }
}
