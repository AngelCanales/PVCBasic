using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class InventoriesRepository : DbContextRepositoryBase<Inventories>
    {
        public InventoriesRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Inventories> All()
        {
            return this.Context.Inventories;
        }

        protected Inventories MapNewValuesToOld(Inventories oldEntity, Inventories newEntity)
        {
            oldEntity.IdProduct = newEntity.IdProduct;
            oldEntity.Existence = newEntity.Existence;
            return oldEntity;
        }
    }
}

