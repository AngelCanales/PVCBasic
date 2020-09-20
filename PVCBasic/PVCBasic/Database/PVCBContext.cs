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
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<DetailInvoices> DetailInvoices { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Parameters> Parameters { get; set; }

        public PVCBContext(DbContextOptions<PVCBContext> options)
      : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailInvoices>()
                .HasOne(i => i.Invoices)
                .WithMany(c => c.DetailInvoices)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Parameters>().HasData(
            //    new { Key = "EmissionPoint", Value = "" },
            //    new { Key = "Establishment", Value = "" },
            //    new { Key = "DocumentType", Value = "" },
            //    new { Key = "CurrenInvoiceNumber", Value = "" },
            //    new { Key = "FirstInvoiceNumber", Value = "" },
            //    new { Key = "LastInvoiceNumber", Value = "" },
            //    new { Key = "PrintCode", Value = "" },
            //    new { Key = "Store", Value = "" },
            //    new { Key = "Logo", Value = "" },
            //    new { Key = "ValidUntilDate", Value = "" }
            //    );


        }
        //public PVCBContext()
        //{
        //    SQLitePCL.Batteries_V2.Init();

            //    this.Database.EnsureCreated();
            //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PVCB.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }     
    }
}
