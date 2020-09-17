using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBCore.Invoices
{
  public interface IInvoicesManager
    {
        Task<IEnumerable<PVCBasic.Database.Models.Invoices>> GetAllAsync();

        Task<IEnumerable<PVCBasic.Database.Models.Invoices>> GetAllByDateAsync(DateTime date);

        Task<IEnumerable<PVCBasic.Database.Models.Invoices>> GetAllByDateYearAsync(DateTime datestar, DateTime dateend);

        Task<PVCBasic.Database.Models.Invoices> FindByIdAsync(int id);

        Task<PVCBasic.Database.Models.Invoices> FindAllByDateAsync(DateTime date);

        Task CreateAsync(PVCBasic.Database.Models.Invoices invoices);

        Task EditAsync(PVCBasic.Database.Models.Invoices invoices);

        Task DeleteAsync(PVCBasic.Database.Models.Invoices invoices);
    }
}
