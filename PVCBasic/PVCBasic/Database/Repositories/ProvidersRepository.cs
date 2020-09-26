using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class ProvidersRepository : DbContextRepositoryBase<Providers>
    {
        public ProvidersRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Providers> All()
        {
            return this.Context.Providers;
        }

        protected Providers MapNewValuesToOld(Providers oldEntity, Providers newEntity)
        {
            oldEntity.Code = newEntity.Code;
            oldEntity.Name = newEntity.Name;
            oldEntity.ShortName = newEntity.ShortName;
            oldEntity.PhoneNumber = newEntity.PhoneNumber;
            oldEntity.Location = newEntity.Location;
            oldEntity.Address = newEntity.Address;
            oldEntity.City = newEntity.City;
            oldEntity.ContactName = newEntity.ContactName;
            oldEntity.ContactPhoneNumber = newEntity.ContactPhoneNumber;
            oldEntity.DateCreate = newEntity.DateCreate;

            return oldEntity;
        }
    }
}

