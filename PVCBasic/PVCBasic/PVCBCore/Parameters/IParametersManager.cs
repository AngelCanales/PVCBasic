using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Parameters
{
  public interface IParametersManager
    {
        Task<IEnumerable<PVCBasic.Database.Models.Parameters>> GetAllAsync();

        Task<PVCBasic.Database.Models.Parameters> FindByIdAsync(string key);
  
        Task CreateAsync(PVCBasic.Database.Models.Parameters invoices);

        Task EditAsync(PVCBasic.Database.Models.Parameters invoices);

        Task DeleteAsync(PVCBasic.Database.Models.Parameters invoices);
    }
}
