using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class EmployeesRepository : DbContextRepositoryBase<Employees>
    {
        public EmployeesRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Employees> All()
        {
            return this.Context.Employees;
        }

        protected Employees MapNewValuesToOld(Employees oldEntity, Employees newEntity)
        {
            oldEntity.Name = newEntity.Name;
            oldEntity.Code = newEntity.Code;
            oldEntity.Route = newEntity.Route;

            return oldEntity;
        }
    }
}

