using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class RoutesRepository : DbContextRepositoryBase<Routes>
    {
        public RoutesRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Routes> All()
        {
            return this.Context.Routes;
        }

        protected Routes MapNewValuesToOld(Routes oldEntity, Routes newEntity)
        {
            
            oldEntity.Code = newEntity.Code;
            
            return oldEntity;
        }
    }
}

