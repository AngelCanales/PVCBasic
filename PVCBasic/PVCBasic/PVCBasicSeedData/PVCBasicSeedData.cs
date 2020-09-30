using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PVCBasic.Database;
using PVCBasic.Database.Models;
using PVCBasic.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.PVCBasicSeedData
{
   public class PVCBasicSeedData 
    {

      

        public static  async Task EnsurePVCBasicSeedData(PVCBContext context)
        {
            bool exist = false;

            try
            {
                SQLitePCL.Batteries_V2.Init();
                exist = context.Database.EnsureCreated();
            }
            catch (Exception)
            {

                throw;
            }
            if (!exist)
            {
                return;
            }
            await InsertParameters(context);
        }


        private static async Task InsertParameters(PVCBContext context)
        {
            
                context.Add(new Parameters { Key = "EmissionPoint", Value = "" });
                context.Add(new Parameters { Key = "Establishment", Value = "" });
                context.Add(new Parameters { Key = "DocumentType", Value = "" });
                context.Add(new Parameters { Key = "CurrenInvoiceNumber", Value = "" });
                context.Add(new Parameters { Key = "FirstInvoiceNumber", Value = "" });
                context.Add(new Parameters { Key = "LastInvoiceNumber", Value = "" });
                context.Add(new Parameters { Key = "PrintCode", Value = "" });
                context.Add(new Parameters { Key = "StoreName", Value = "ParameterStoreName" });
                context.Add(new Parameters { Key = "Logo", Value = "Parameter" });
                context.Add(new Parameters { Key = "ValidUntilDate", Value = "" });
                context.Add(new Parameters { Key = "NamePrint", Value = "ParameterNamePrint" });

                context.Add(new Parameters { Key = "CAI", Value = "Parameter" });

                context.Add(new Parameters { Key = "Email", Value = "ParameterEmail" });
                context.Add(new Parameters { Key = "Phonenumbe", Value = "ParameterPhonenumbe" });
                context.Add(new Parameters { Key = "Address", Value = "ParameterAddress" });
                context.Add(new Parameters { Key = "RTN", Value = "ParameterRTN" });
                context.Add(new Parameters { Key = "ThankMessage", Value = "ParameterThankMessage" });
                await context.SaveChangesAsync();
           
        }

    }
}
