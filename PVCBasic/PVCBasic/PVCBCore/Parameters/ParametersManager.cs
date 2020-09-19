using Microsoft.EntityFrameworkCore;
using PVCBasic.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Parameters
{
    public class ParametersManager : IParametersManager
    {
        private readonly IRepository<PVCBasic.Database.Models.Parameters> parameters;

        public ParametersManager(IRepository<PVCBasic.Database.Models.Parameters> parameters)
        {
            this.parameters = parameters;
        }

        public async Task CreateAsync(Database.Models.Parameters parameters)
        {
            this.parameters.Create(parameters);
            await this.parameters.SaveChangesAsync();
        }

        public async Task DeleteAsync(Database.Models.Parameters parameters)
        {
            
            this.parameters.Delete(parameters);
            await this.parameters.SaveChangesAsync();
        }

        public async Task EditAsync(Database.Models.Parameters parameters)
        {
            this.parameters.Update(parameters);
            await this.parameters.SaveChangesAsync();
        }

        public async Task<Database.Models.Parameters> FindByIdAsync(string key)
        {
            var parameters = await this.parameters.All().FirstOrDefaultAsync( w => w.Key == key);
            return parameters; 
        }

        public async Task<IEnumerable<Database.Models.Parameters>> GetAllAsync()
        {
            var parameters = await this.parameters.All().ToListAsync();
            return parameters;
        }
    }
}
