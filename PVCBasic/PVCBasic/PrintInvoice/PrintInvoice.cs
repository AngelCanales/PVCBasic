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

        public async Task<string> GetHtmlInvoices(string id)
        {
            string html = string.Empty;
            try
            {
                var inv = await invoicesManager.FindByNumberInvoicesAsync(id);
                 html = await this.ReceiptGenerateAsyc(inv);
                
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($" Error: {e.Message}", ToastLength.Long);
            }
            return html;
        }

        public async Task PrintInvoices(string id)
        {
            try
            {
                var nameprint = await parametersManager.FindByIdAsync(ConstantName.ConstantName.NamePrint);
                this.bthPrint.NamePrint = nameprint.Value;

                var inv = await invoicesManager.FindByNumberInvoicesAsync(id);

                await this.bthPrint.ConnectAsync();
                if (this.bthPrint.IsConnected)
                {
                    await ExecutePrintInvoicesHeader();
                    await ExecutePrintInvoicesDetail(inv.DetailInvoices);
                    await ExecutePrintInvoicesFoother(inv);
                    await this.bthPrint.DisconnectAsync();
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError($"Error: Not Connected", ToastLength.Long);
                }
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


        public async Task<string> ReceiptGenerateAsyc(Invoices invoices)
        {
            var parameter = await parametersManager.GetAllAsync();

            var storeName = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.StoreName).Value;
            var address = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Address).Value;
            var phonenumber = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Phonenumbe).Value;
            var email = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Email).Value;
            var logo = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.Logo).ValueImage;
            var thankMessage = parameter.FirstOrDefault(c => c.Key == ConstantName.ConstantName.ThankMessage).Value;

            string imgSrc = string.Empty;

            if (logo != null)
            {
                imgSrc = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(logo));
            }

            decimal value = 0;
            var exchangenewx = invoices.Exchange != value ? invoices.Exchange.ToString("C2") : value.ToString("C2");
            var cashnew = invoices.Cash != value ? invoices.Cash.ToString("C2") : value.ToString("C2");
            var reci = @" <!DOCTYPE html>
<html>
<body>
<section class='container'>
<div style = 'text-align:center'>
<img src='" + imgSrc + "'" + @" />
</div> 
                        <div style = 'font-family:monospace'>
<section>
<div>
<div>
 
                     <dl style = 'text-align:center' >
                      
                        <dt style = 'color:black;font-weight: bold; font-family:Arial'>
                            " + $"{this.Title}" + @"
                        </dt>
                        <dt style = 'color:black;font-weight: bold; font-family:Arial'>" + $"{storeName}" + @"</dt>                       
                        <dt>
                            Fecha: " + $"{DateTime.Now.ToString("dd/mm/yyyy")}" + @"
                        </dt>
                        <dt>
                            Dirrecion:" + $"{address}" + @"
                        </dt>
                        <dt>
                            Telefono:" + $"{phonenumber}" + @"
                        </dt>
                        <dt>
                            Email: " + $"{email}" + @"
                        </dt>
                    </dl>
                 
                </div>
            </div>
            <hr style = 'border:1px dashed black; width:300px' />
            <div>
                <div>
                    <table style='text-align:left'>
                        <thead>
                            <tr>
                                <th style = 'text-align:center;color:black;font-weight: bold; font-family:Arial' >
                                    Detalle
                                </th>
                            </tr>
                        </thead>
                        <tbody>
";
            foreach (var item in invoices.DetailInvoices)
            {
                reci = reci + @"<tr>
                                    <td style = 'padding-right:4px;text-align:center;' > " + $"{item.Description}" + @" </td>
                                     </tr>";
            }

            reci = reci + @"</tbody>
                    </table>
                    <br />
                    <hr style = 'border:1px dashed black; width:300px' />
                    <div style='float:right;' class='pull-right'>
                        <table style='text-align:left'>
                        <thead>
                            <tr>
                                <th >
                                </th>
<th >
</th>
                            </tr>
                        </thead>
                        <tbody>
<tr>
<td style = 'padding-right:4px;text-align:center;color:black;font-weight: bold;'> Total:</td><td style = 'padding-right:4px;text-align:center;'>" + invoices.Total.ToString("C2") + @" </td>
</tr>
<tr>
<td style = 'padding-right:4px;text-align:center;color:black;font-weight: bold;'> Efectivo:</td><td style = 'padding-right:4px;text-align:center;'>" + cashnew + @" </td>
</tr>
<tr>
<td style = 'padding-right:4px;text-align:center;color:black;font-weight: bold;'> Cambio:</td><td style = 'padding-right:4px;text-align:center;'>" + exchangenewx + @" </td>
</tr>
                            </tbody>
                    </table>
                    <br />
                    </div>
                </div>
            </div>
        </section>
        <hr style='border:1px dashed black; width:300px' />
        <dl style = 'text-align:center'> ********************</dl>
        <dl style = 'text-align:center'>" + $"{thankMessage}" + @"</dl>
        <dl style = 'text-align:center'> ********************</dl>
        <dl style = 'text-align:center'> Copia</dl>
    </div>

</section>

<style>
    .container {
        width: 320px;
        height: auto;
    }

    dt {
    }

table
{
}

dl
{
}

div
{
}

    .boxooo
{
    position: absolute;
    margin: 0 auto;
    left: 0;
    right: 0;
    width: 320px;
    height: auto;
    border - style: solid;
    background - color: white;
}

    .btnimp
{
    position: absolute;
    width: 20px;
    height: auto;
    margin: 0 auto;
}
</style>
</body>
</html> ";

            return reci;
        }

    }
}
