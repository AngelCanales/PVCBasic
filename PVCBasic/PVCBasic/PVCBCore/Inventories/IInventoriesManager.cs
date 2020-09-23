using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Inventories
{
  public interface IInventoriesManager
    {
        Task<IEnumerable<PVCBasic.Database.Models.Inventories>> GetAllAsync();

        Task<PVCBasic.Database.Models.Inventories> FindByIdAsync(int id);
  
        Task CreateAsync(PVCBasic.Database.Models.Inventories inventories);

        Task EditAsync(PVCBasic.Database.Models.Inventories inventories);

        Task DeleteAsync(PVCBasic.Database.Models.Inventories inventories);
    }
}
