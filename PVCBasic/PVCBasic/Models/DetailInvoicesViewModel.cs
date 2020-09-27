using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Models
{
   public class DetailInvoicesViewModel
    {
        public int IdD { get; set; }
        public Guid Id { get; set; }
        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal TotalItem { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public int? IdProduct { get; set; }

        public string CodeProduct { get; set; }

    }
}
