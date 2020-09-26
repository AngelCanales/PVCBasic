using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class SalesTypesRepository : DbContextRepositoryBase<SalesTypes>
    {
        public SalesTypesRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<SalesTypes> All()
        {
            return this.Context.SalesTypes;
        }

        protected SalesTypes MapNewValuesToOld(SalesTypes oldEntity, SalesTypes newEntity)
        {
            oldEntity.Code = newEntity.Code;
            oldEntity.Description = newEntity.Description;

            return oldEntity;
        }
    }
}

