using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Database.Models
{
   public class Parameters
    {
        public int Id { get; set; }
        public string Key  { get; set; }

        public string Value { get; set; }

        public byte[] ValueImage { get; set; }

        public DateTime ValueDate { get; set; }

        public int ValueInt { get; set; }

        public decimal ValueDecimal { get; set; }

        public bool ValueBool { get; set; }

    }
}
