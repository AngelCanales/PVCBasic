using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Products
{
  public interface IProductsManager
    {
        Task<IEnumerable<PVCBasic.Database.Models.Products>> GetAllAsync();

        Task<PVCBasic.Database.Models.Products> FindByIdAsync(int id);
  
        Task CreateAsync(PVCBasic.Database.Models.Products invoices);

        Task EditAsync(PVCBasic.Database.Models.Products invoices);

        Task DeleteAsync(PVCBasic.Database.Models.Products invoices);
    }
}
