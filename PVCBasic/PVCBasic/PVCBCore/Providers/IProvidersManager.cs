using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Providers
{
  public interface IProvidersManager
    {
        Task<IEnumerable<PVCBasic.Database.Models.Providers>> GetAllAsync();

        Task<PVCBasic.Database.Models.Providers> FindByIdAsync(int id);
  
        Task CreateAsync(PVCBasic.Database.Models.Providers providers);

        Task EditAsync(PVCBasic.Database.Models.Providers providers);

        Task DeleteAsync(PVCBasic.Database.Models.Providers providers);
    }
}
