using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Models
{
    public class MonthlyReportModel
    {
        public int mes { get; set; }

        public decimal TotalSales { get; set; }

        public decimal TotalPurchase { get; set; }

        public decimal Difference { get; set; }

        public string MonthName { get; set; }

        public DateTime Date { get; set; }

        public string TypeInvoice { get; set; }

        public int CountSales { get; set; }

        public int CountPurchase { get; set; }

        public string DifferenceTextColor { get; set; }
    }
}
