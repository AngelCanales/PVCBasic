using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PVCBasic.Database.Models;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using PVCBasic.PVCBCore.Inventories;
using PVCBasic.DependencyService;
using PVCBasic.PVCBCore.Parameters;
using System.Collections.Generic;
using DryIoc;

namespace PVCBasic.PrintInvoice
{
    public class PrintInvoice
    {
        private readonly IInvoicesManager invoicesManager;
        private readonly IInventoriesManager inventoriesManager;
        private readonly IParametersManager parametersManager;
        private readonly IBthPrint bthPrint;
        public PrintInvoice(IInvoicesManager invoicesManager, IInventoriesManager inventoriesManager, IBthPrint bthPrint, IParametersManager parametersManager)
        {
            this.invoicesManager = invoicesManager;
            this.inventoriesManager = inventoriesManager;
            this.parametersManager = parametersManager;
            this.bthPrint = bthPrint;
        }

        public async Task PrintInvoices(string id)
        {
            try
            {
                var nameprint = await parametersManager.FindByIdAsync(ConstantName.ConstantName.NamePrint);
                this.bthPrint.NamePrint = nameprint.Value;

                var inv = await invoicesManager.FindByNumberInvoicesAsync(id);

                await this.bthPrint.ConnectAsync();

                await ExecutePrintInvoicesHeader();
                await ExecutePrintInvoicesDetail(inv.DetailInvoices);
                await ExecutePrintInvoicesFoother(inv);
                await this.bthPrint.DisconnectAsync();
            }
            catch (Exception e)
            {
                CrossToastPopUp.Current.ShowToastError($"PrintInvoices Error: {e.Message}", ToastLength.Long);
            }
           
        }


        public async Task ExecutePrintInvoicesHeader()
        {
            try
            {
                var parameter = await parametersManager.GetAllAsync();
                var storeName = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.StoreName).Value;
                var address = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Address).Value;
                var phonenumber = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Phonenumbe).Value;
                var email = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Email).Value;
                var logo = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Logo).ValueImage;
              
                await this.bthPrint.PrintImage(logo);
                await this.bthPrint.WriteAsync("                  ");
                await this.bthPrint.WriteAsync(this.Title);
                await this.bthPrint.WriteAsync(storeName);
                await this.bthPrint.WriteAsync("Direccion: " + address);
                await this.bthPrint.WriteAsync("Telefono: " + phonenumber);
                await this.bthPrint.WriteAsync("RTN:");
                await this.bthPrint.WriteAsync("Correo: " + email);
                await this.bthPrint.WriteAsync("                  ");
                await this.bthPrint.WriteAsync("Factura No: ");
               // await this.bthPrint.WriteAsync("C.A.I.: ");
                await this.bthPrint.WriteAsync("Fecha de emisión" + DateTime.Now.ToString("dd/MM/yyyy"));
                // await this.bthPrint.WriteAsync("Cliente.: ");
                // await this.bthPrint.WriteAsync("RTN: ");
                
                

            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error: {e.Message}", ToastLength.Long);
            }
        }


        public async Task ExecutePrintInvoicesDetail(List<DetailInvoices> detailInvoices)
        {
            try
            {
                await this.bthPrint.WriteAsync("            Detalle             ");
                await this.bthPrint.WriteAsync("================================");


                foreach (var item in detailInvoices)
                {
                    await this.bthPrint.WriteAsync(item.Description);
                }


            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error: {e.Message}", ToastLength.Long);
            }
        }

        public async Task ExecutePrintInvoicesFoother(Invoices invoices)
        {
            try
            {
                var parameter = await parametersManager.GetAllAsync();
                var thankMessage = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.ThankMessage).Value;
                await this.bthPrint.WriteAsync("================================");
                var subtotal = invoices.DetailInvoices.Sum(s => s.TotalItem).ToString("C2");
                await this.bthPrint.WriteAsync("SubTotal:" + subtotal);
                await this.bthPrint.WriteAsync("Impuesto:" + invoices.Tax.ToString("C2"));
                await this.bthPrint.WriteAsync("Total:" + invoices.Total.ToString("C2"));
                await this.bthPrint.WriteAsync("Efectivo:" + invoices.Cash.ToString("C2"));
                await this.bthPrint.WriteAsync("Cambio:" + invoices.Exchange.ToString("C2"));
                await this.bthPrint.WriteAsync("                  ");
                await this.bthPrint.WriteAsync("*******************************");
                await this.bthPrint.WriteAsync(thankMessage);
                await this.bthPrint.WriteAsync("*******************************");
                await this.bthPrint.WriteAsync("                  ");
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error: {e.Message}", ToastLength.Long);
            }
        }

        public string Title { get; set; }


    }
}
