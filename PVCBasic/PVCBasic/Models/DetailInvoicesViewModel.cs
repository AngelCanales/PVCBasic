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

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
