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

    //    public PVCBContext(DbContextOptions<PVCBContext> options)
    //: base(options)
    //    {
    //        SQLitePCL.Batteries_V2.Init();
    //        this.Database.EnsureCreated();
    //    }

        public DbSet<Invoices> Invoices { get; set; }

        public DbSet<DetailInvoices> DetailInvoices { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Parameters> Parameters { get; set; }

        public DbSet<Providers> Providers { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Routes> Routes { get; set; }

        public DbSet<SalesTypes> SalesTypes { get; set; }

        public DbSet<PaymentTypes> PaymentTypes { get; set; }

        public DbSet<Inventories> Inventories { get; set; }
        
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailInvoices>()
                .HasOne(i => i.Invoices)
                .WithMany(c => c.DetailInvoices)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PVCB.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
              //  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }     
    }
}
