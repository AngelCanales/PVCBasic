using Microsoft.EntityFrameworkCore;
using PVCBasic.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Providers
{
    public class ProvidersManager : IProvidersManager
    {
        private readonly IRepository<PVCBasic.Database.Models.Providers> providers;

        public ProvidersManager(IRepository<PVCBasic.Database.Models.Providers> providers)
        {
            this.providers = providers;
        }

        public async Task CreateAsync(Database.Models.Providers nproviders)
        {
            this.providers.Create(nproviders);
            await this.providers.SaveChangesAsync();
        }

        public async Task DeleteAsync(Database.Models.Providers nproviders)
        {
            
            this.providers.Delete(nproviders);
            await this.providers.SaveChangesAsync();
        }

        public async Task EditAsync(Database.Models.Providers nproviders)
        {
            this.providers.Update(nproviders);
            await this.providers.SaveChangesAsync();
        }

        public async Task<Database.Models.Providers> FindByIdAsync(int id)
        {
            var nproviders = await this.providers.All().FirstOrDefaultAsync( w => w.Id == id);
            return nproviders; 
        }

        public async Task<IEnumerable<Database.Models.Providers>> GetAllAsync()
        {
            var nproviders = await this.providers.All().ToListAsync();
            return nproviders;
        }
    }
}
