using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class CustomersRepository : DbContextRepositoryBase<Customers>
    {
        public CustomersRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Customers> All()
        {
            return this.Context.Customers;
        }

        protected Customers MapNewValuesToOld(Customers oldEntity, Customers newEntity)
        {
            oldEntity.Code = newEntity.Code;
            oldEntity.Name = newEntity.Name;
            oldEntity.ShortName = newEntity.ShortName;
            oldEntity.PhoneNumber = newEntity.PhoneNumber;
            oldEntity.Location = newEntity.Location;
            oldEntity.Address = newEntity.Address;
            oldEntity.City = newEntity.City;
            oldEntity.DateCreate = newEntity.DateCreate;

            return oldEntity;
        }
    }
}

