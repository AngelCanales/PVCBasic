using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class InvoicesRepository : DbContextRepositoryBase<Invoices>
    {
        public InvoicesRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Invoices> All()
        {
            return this.Context.Invoices;
        }

        protected Invoices MapNewValuesToOld(Invoices oldEntity, Invoices newEntity)
        {
            oldEntity.Date = newEntity.Date;
            oldEntity.Description = newEntity.Description;
            oldEntity.InvoicesTypes = newEntity.InvoicesTypes;
            oldEntity.Total = newEntity.Total;
            oldEntity.DetailInvoices = newEntity.DetailInvoices;
            return oldEntity;
        }
    }
}

