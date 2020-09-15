using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Models
{
  public class InvoicesViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal Total { get; set; }

        public string InvoicesTypes { get; set; }
    }
}
