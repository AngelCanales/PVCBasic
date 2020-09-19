using PVCBasic.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVCBasic.Database.Repositories
{
   public class ProductRepository : DbContextRepositoryBase<Products>
    {
        public ProductRepository(PVCBContext context)
         : base(context)
        {
        }

        public override IQueryable<Products> All()
        {
            return this.Context.Products;
        }

        protected Products MapNewValuesToOld(Products oldEntity, Products newEntity)
        {
            oldEntity.Date = newEntity.Date;
            oldEntity.Name = newEntity.Name;
            oldEntity.ShortName = newEntity.ShortName;
            oldEntity.Price = newEntity.Price;
            oldEntity.Cost = newEntity.Cost;
            oldEntity.Tax = newEntity.Tax;
            oldEntity.Discount = newEntity.Discount;
            oldEntity.Code = newEntity.Code;
            oldEntity.Image = newEntity.Image;
            return oldEntity;
        }
    }
}

