using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Customers
{
  public interface ICustomersManager
    {
        Task<IEnumerable<PVCBasic.Database.Models.Customers>> GetAllAsync();

        Task<PVCBasic.Database.Models.Customers> FindByIdAsync(int id);
  
        Task CreateAsync(PVCBasic.Database.Models.Customers customers);

        Task EditAsync(PVCBasic.Database.Models.Customers customers);

        Task DeleteAsync(PVCBasic.Database.Models.Customers customers);
    }
}
