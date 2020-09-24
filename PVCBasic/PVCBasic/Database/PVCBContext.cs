using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PVCBasic.Database.Models;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace PVCBasic.Database
{
    public class PVCBContext : DbContext
    {
        public PVCBContext()
        {

        }

        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<DetailInvoices> DetailInvoices { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Parameters> Parameters { get; set; }

        public DbSet<Inventories> Inventories { get; set; }
        
      //  public PVCBContext(DbContextOptions<PVCBContext> options)
      //: base(options)
      //  {
      //      try
      //      {
      //          SQLitePCL.Batteries_V2.Init();
      //          this.Database.EnsureCreated();
      //      }
      //      catch (Exception)
      //      {

      //          throw;
      //      }
            
      //  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailInvoices>()
                .HasOne(i => i.Invoices)
                .WithMany(c => c.DetailInvoices)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            //modelBuilder.Entity<Parameters>().HasData(
            //    new { Id = 1, Key = "EmissionPoint", Value = "" },
            //    new { Id = 2, Key = "Establishment", Value = "" },
            //    new { Id = 3, Key = "DocumentType", Value = "" },
            //    new { Id = 4, Key = "CurrenInvoiceNumber", Value = "" },
            //    new { Id = 5, Key = "FirstInvoiceNumber", Value = "" },
            //    new { Id = 5, Key = "LastInvoiceNumber", Value = "" },
            //    new { Id = 7, Key = "PrintCode", Value = "" },
            //    new { Id = 8, Key = "Store", Value = "" },
            //    new { Id = 9, Key = "Logo", Value = "" },
            //    new { Id = 10, Key = "ValidUntilDate", Value = "" }
            //    );
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PVCB.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }     
    }
}
