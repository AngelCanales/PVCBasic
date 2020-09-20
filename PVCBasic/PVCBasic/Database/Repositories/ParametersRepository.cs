using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class ParametersRepository : DbContextRepositoryBase<Parameters>
    {
        public ParametersRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Parameters> All()
        {
            return this.Context.Parameters;
        }

        protected Parameters MapNewValuesToOld(Parameters oldEntity, Parameters newEntity)
        {
            oldEntity.Key = newEntity.Key;
            oldEntity.Value = newEntity.Value;
            oldEntity.ValueImage = newEntity.ValueImage;
            oldEntity.ValueDate = newEntity.ValueDate;
            oldEntity.ValueInt = newEntity.ValueInt;
            oldEntity.ValueDecimal = newEntity.ValueDecimal;
            oldEntity.ValueBool = newEntity.ValueBool;

            return oldEntity;
        }
    }
}

