using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Database.Models
{
  public class Invoices
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal Total { get; set; }

        public string InvoicesTypes { get; set; }

        public List<DetailInvoices> DetailInvoices { get; set; } = new List<DetailInvoices>();

    }
}
