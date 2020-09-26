using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.Models
{
   public class ProvidersModel
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Location { get; set; }

        public string City { get; set; }

        public string ContactName { get; set; }

        public string ContactPhoneNumber { get; set; }
    }
}
