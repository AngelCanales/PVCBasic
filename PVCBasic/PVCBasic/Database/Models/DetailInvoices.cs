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

        public int IdInvoices { get; set; }

        public Invoices Invoices { get; set; }
    }
}
