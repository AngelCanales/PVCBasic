using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class PaymentTypesRepository : DbContextRepositoryBase<PaymentTypes>
    {
        public PaymentTypesRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<PaymentTypes> All()
        {
            return this.Context.PaymentTypes;
        }

        protected PaymentTypes MapNewValuesToOld(PaymentTypes oldEntity, PaymentTypes newEntity)
        {
            oldEntity.Code = newEntity.Code;
            oldEntity.Description = newEntity.Description;

            return oldEntity;
        }
    }
}

