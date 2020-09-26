using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Database.Models
{
   public class DetailInvoices
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal TotalItem { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal? Tax { get; set; }

        public decimal? Discount { get; set; }

        public int? IdProduct { get; set; }

        public string CodeProduct { get; set; }

        public int IdInvoices { get; set; }

        public Invoices Invoices { get; set; }
    }
}
