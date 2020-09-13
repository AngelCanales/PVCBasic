using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Database.Repositories
{
    public abstract class DbContextRepositoryBase<TEntity> : RepositoryBase<TEntity, PVCBContext>
       where TEntity : class
    {
        public DbContextRepositoryBase(PVCBContext context)
            : base(context)
        {
            this.Context = context;
        }
    }
}
