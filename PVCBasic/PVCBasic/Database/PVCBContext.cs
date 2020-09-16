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

        public PVCBContext(DbContextOptions<PVCBContext> options)
      : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
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
