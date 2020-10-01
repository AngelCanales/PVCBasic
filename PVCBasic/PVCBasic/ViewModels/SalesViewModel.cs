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
using PVCBasic.PVCBCore.Parameters;
using PVCBasic.ConstantName;

namespace PVCBasic.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        private int? quantity;
        private decimal? total;
        private string description;
        private string nameProduct;
        private string typeInvoice;
        private decimal? price;
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private decimal totalitem;
        private decimal? cash;
        private decimal? exchange;
        private string receipt;
        private readonly IInvoicesManager invoicesManager;
        private readonly IInventoriesManager inventoriesManager;
        private readonly IParametersManager parametersManager;
        private int? idProduct;
        private CustomersModel customer;
        private ProvidersModel provider;
        private string typeDecription;
        private ProductsModel product;

        public SalesViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager, IInventoriesManager inventoriesManager, IParametersManager parametersManager) : base(navigationService)
        {
            try
            {
                this.dialogService = dialogService;
                this.invoicesManager = invoicesManager;
                this.inventoriesManager = inventoriesManager;
                this.parametersManager = parametersManager;
                this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
                this.SalvarCommand = new DelegateCommand(async () => await this.ExecuteSalvarCommand());
                this.AddItemCommand = new DelegateCommand(async () => await this.ExecuteAddItemCommand());
                this.UpdateItemCommand = new DelegateCommand(async () => await this.ExecuteUpdateItemCommand());
                this.DeleteItemCommand = new DelegateCommand(async () => await this.ExecuteDeleteItemCommand());
                this.ClearItemsCommand = new DelegateCommand(async () => await this.ExecuteClearItemsCommand());
                this.SearchProductCommand = new DelegateCommand(async () => await this.ExecuteSearchProductCommand());
                this.SearchProviderClitenCommand = new DelegateCommand(async () => await this.ExecuteSearchProviderClitenCommand());
            
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error {e.Message}", ToastLength.Long);
            }
        }

        private async Task ExecuteSearchProviderClitenCommand()
        {

            var param = new NavigationParameters();
            param.Add("SelectedProduct", this.Product);
            param.Add("TypeInvoice", this.TypeInvoice);

            if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
            {

                await this.NavigationService.NavigateAsync("SearchProviderPage", param);
            }

            if (this.TypeInvoice == ConstantName.ConstantName.Sales)
            {
                await this.NavigationService.NavigateAsync("SearchCustomerPage", param);
            }

        }

        private async Task ExecuteSearchProductCommand()
        {
            var param = new NavigationParameters();
            param.Add("SelectedProvider", this.Provider);
            param.Add("SelectedCustomers", this.Customer);
            param.Add("TypeInvoice", this.TypeInvoice);
            await this.NavigationService.NavigateAsync("SearchProductPage", param);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {


                if (parameters.ContainsKey("TypeInvoice"))
                {
                    this.TypeInvoice = parameters["TypeInvoice"] as string;

                    if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
                    {
                        this.TypeDecription = "Proveedor:";
                    }

                    if (this.TypeInvoice == ConstantName.ConstantName.Sales)
                    {
                        this.TypeDecription = "Cliente:";
                    }
                }

                if (parameters.ContainsKey("Title"))
                {
                    this.Title = parameters["Title"] as string;
                }

                if (parameters.ContainsKey("SelectedCustomers"))
                {
                    this.Customer = parameters["SelectedCustomers"] as CustomersModel;
                    if (this.Customer != null)
                    {
                        this.Description = this.Customer.ShortName;
                    }
                }

                if (parameters.ContainsKey("SelectedProvider"))
                {
                    this.Provider = parameters["SelectedProvider"] as ProvidersModel;
                    if (this.Provider != null)
                    {
                        this.Description = this.Provider.ShortName;
                    }
                }

                if (parameters.ContainsKey("SelectedProduct"))
                {
                    this.Product = parameters["SelectedProduct"] as ProductsModel;
                    if (this.Product != null)
                    {


                        this.IdProduct = this.Product.Id;
                        //  CrossToastPopUp.Current.ShowToastSuccess($"price :{product.Price.Value.ToString()}", ToastLength.Long);
                        if (this.TypeInvoice == ConstantName.ConstantName.Sales)
                        {
                            this.NumberPrice = this.Product.Price.Value.ToString();
                            this.Price = this.Product.Price.Value;
                            this.NameProduct = this.Product.ShortName;
                        }
                        if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
                        {

                            this.NumberPrice = this.Product.Cost.Value.ToString();
                            this.Price = this.Product.Cost.Value;
                            this.NameProduct = this.Product.ShortName;
                        }
                    }
                }

                if (parameters.ContainsKey("DetailInvoices"))
                {
                    if (this.DetailInvoices != null)
                    {
                        this.DetailInvoices = parameters["DetailInvoices"] as ObservableCollection<DetailInvoicesViewModel>;
                        this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
                    }
                }

            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error {e.Message}", ToastLength.Long);
            }
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add("DetailInvoices", this.DetailInvoices);
            parameters.Add("Title", this.Title);
            parameters.Add("TypeInvoice", this.TypeInvoice);
            parameters.Add("SelectedProvider", this.Provider);
            parameters.Add("SelectedCustomers", this.Customer); ;
            parameters.Add("SelectedProduct", this.Product);

        }

        public DetailInvoicesViewModel SelectedItemDetails { get; set; }

        public ICommand AddItemCommand { get; set; }

        public ICommand ClearItemsCommand { get; set; }

        public ICommand SearchProductCommand { get; set; }

        public ICommand SearchProviderClitenCommand { get; set; }

        private async Task ExecuteClearItemsCommand()
        {
            this.DetailInvoices.Clear();
            this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
        }

        private async Task ExecuteAddItemCommand()
        {
            if (this.Quantity == null) { return; };
            if (this.Price == null) { return; };

            var des = $"{this.NameProduct} => {this.Quantity.Value} x {this.Price.Value} = {this.TotalItem.ToString("C2")}";
            var item = new DetailInvoicesViewModel();

            item.Id = Guid.NewGuid();
            item.TotalItem = this.Quantity.Value * this.Price.Value;
            item.Description = des;
            item.ProductName = this.NameProduct;
            item.Price = this.Price.Value;
            item.Quantity = this.Quantity.Value;

            if (this.Product != null)
            {
                item.IdProduct = this.Product.Id ;
                item.CodeProduct = this.Product.Code ;
            }
          
            this.DetailInvoices.Add(item);
            this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
            this.NameProduct = "";
            this.TotalItem = 0;
            this.NumberQuantity = string.Empty;
            this.NumberPrice = string.Empty; ;
            this.NumberCash = string.Empty; ;
            this.Product = null;
            this.Exchange = null;
        }

        public ICommand UpdateItemCommand { get; set; }

        private async Task ExecuteUpdateItemCommand()
        {
            foreach (var item in this.DetailInvoices)
            {
                if (item.Id == this.SelectedItemDetails.Id)
                {
                    var des = $"{this.NameProduct} => {this.Quantity} x {this.Price} = {this.TotalItem.ToString("C2")}";

                    item.Description = des;
                    item.ProductName = this.NameProduct;
                    item.Quantity = this.Quantity.Value;
                    item.Price = this.Price.Value;
                }
                this.Total = this.DetailInvoices.Sum(s => s.TotalItem);

                this.TotalItem = 0;
                this.Total = 0;
                this.TotalItem = 0;
                this.NameProduct = "";

                this.Quantity = null;
                this.Price = null;
                this.Cash = null;
                this.NumberQuantity = "";
                this.NumberPrice = "";
                this.NumberCash = string.Empty;
                this.Exchange = null;
            }
        }

        public ICommand DeleteItemCommand { get; set; }

        private async Task ExecuteDeleteItemCommand()
        {
            this.DetailInvoices.Remove(this.SelectedItemDetails);
            this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
        }

        public ICommand SalvarCommand { get; set; }

        private async Task ExecuteSalvarCommand()
        {
            await this.SalvarAsync();
        }

        public async Task SalvarAsync()
        {
            if (!this.DetailInvoices.Any()) { return; }

          
            //if(this.Cash.Value == 0)
            //{
            //    this.Cash = this.Total.Value;
            //}

            string numberinvoice = string.Empty;
            try
            {
                var invoice = new Invoices();
                var detail = this.DetailInvoices.Select(s => new DetailInvoices { IdInvoices = invoice.Id, Invoices = invoice, Description = s.Description, TotalItem = s.TotalItem, Price = s.Price, Quantity = s.Quantity, Tax = s.Tax, IdProduct = s.IdProduct, CodeProduct = s.CodeProduct }).ToList();
                invoice.Date = DateTime.Now;
                invoice.Description = this.Description;
                invoice.InvoicesTypes = this.TypeInvoice;
                invoice.Total = this.DetailInvoices.Sum(t => t.TotalItem);
                invoice.DetailInvoices = detail;
                invoice.Exchange = this.Exchange != null ? this.Exchange.Value : 0;
                invoice.Cash = this.Cash != null ? this.Cash.Value : 0;
                invoice.Tax = this.DetailInvoices.Sum(t => t.Tax);
                numberinvoice = Guid.NewGuid().ToString();
                invoice.NumFactura = numberinvoice;
                if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
                {
                    if(this.Provider != null) 
                    {
                    invoice.CodeProvider = this.Provider.Code;
                    invoice.IdProvider = this.Provider.Id;
                    }
                }

                if (this.TypeInvoice == ConstantName.ConstantName.Sales)
                {
                    if (this.Customer != null)
                    {
                        invoice.CodeCustomer = this.Customer.Code;
                        invoice.IdCustomer = this.Customer.Id;
                    }
                }

                await this.invoicesManager.CreateAsync(invoice);


            }
            catch (Exception e)
            {
                CrossToastPopUp.Current.ShowToastError($"Error Data base{e.Message}", ToastLength.Long);
            }

            try
            {

                foreach (var item in this.DetailInvoices)
                {
                    // await this.dialogService.DisplayAlertAsync("MSG", $"new item", "OK");

                    if (item.IdProduct != null)
                    {
                        //  await this.dialogService.DisplayAlertAsync("MSG", $"IdProduct:{item.IdProduct}", "OK");
                        var exi = await this.inventoriesManager.FindByIdAsync(item.IdProduct.Value);
                        if (exi != null)
                        {
                            // CrossToastPopUp.Current.ShowToastMessage($"Existencia ", ToastLength.Long);
                            System.Diagnostics.Debug.WriteLine("Exi");
                            if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
                            {
                                // await this.dialogService.DisplayAlertAsync("MSG", $"Existencia compra:{exi.IdProduct} - Q{exi.Existence} + P{item.Quantity}", "OK");

                                //CrossToastPopUp.Current.ShowToastMessage($"Existencia compra {exi.Existence} + {item.Quantity}", ToastLength.Long);
                                exi.Existence = exi.Existence + item.Quantity;
                                await this.inventoriesManager.EditAsync(exi);
                                System.Diagnostics.Debug.WriteLine($"Existencia edit Compras-{item.IdProduct.Value}");
                            }
                            if (this.TypeInvoice == ConstantName.ConstantName.Sales)
                            {
                                //await this.dialogService.DisplayAlertAsync("MSG", $"Existencia venta:{exi.IdProduct} - Q{exi.Existence} - P{item.Quantity}", "OK");

                                exi.Existence = exi.Existence - item.Quantity;
                                await this.inventoriesManager.EditAsync(exi);
                                //    CrossToastPopUp.Current.ShowToastMessage($"Existencia venta {exi.Existence} - {item.Quantity}", ToastLength.Long);
                                System.Diagnostics.Debug.WriteLine($"Existencia edit Ventas-{item.IdProduct.Value}");
                            }
                        }
                        else
                        {
                            if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
                            {
                                // await this.dialogService.DisplayAlertAsync("MSG", $"Existencia compra:{exi.IdProduct} - Q{exi.Existence} + P{item.Quantity}", "OK");

                                //CrossToastPopUp.Current.ShowToastMessage($"Existencia compra 0 + {item.Quantity}", ToastLength.Long);
                                var p = new Inventories();
                                p.Existence = item.Quantity;
                                p.IdProduct = item.IdProduct.Value;
                                await this.inventoriesManager.CreateAsync(p);
                                System.Diagnostics.Debug.WriteLine($"Exi create Compras- {item.IdProduct.Value}");
                            }
                            if (this.TypeInvoice == ConstantName.ConstantName.Sales)
                            {
                                //  await this.dialogService.DisplayAlertAsync("MSG", $"Existencia venta:{exi.IdProduct} - Q{exi.Existence} + P{item.Quantity}", "OK");

                                var p = new Inventories();
                                p.Existence = (-1 * item.Quantity);
                                p.IdProduct = item.IdProduct.Value;
                                await this.inventoriesManager.CreateAsync(p);
                                //  CrossToastPopUp.Current.ShowToastMessage($"Existencia ventas 0 + {item.Quantity}", ToastLength.Long);
                                System.Diagnostics.Debug.WriteLine($"Exi create Ventas-{item.IdProduct.Value}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error Receipt{e.Message}", ToastLength.Long);
            }
            try
            {
                this.Receipt = await this.ReceiptGenerateAsyc();
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"Error Receipt{e.Message}", ToastLength.Long);
            }

            CrossToastPopUp.Current.ShowToastSuccess($"Se guardo en: {this.Title}", ToastLength.Long);
            this.Total = 0;
            this.TotalItem = 0;
            this.NameProduct = string.Empty;
            this.Description = string.Empty;
            this.Quantity = null;
            this.Price = null;
            this.Cash = null;
            this.NumberQuantity = "";
            this.NumberPrice = null;
            this.NumberCash = string.Empty;
            this.Exchange = null;
            this.DetailInvoices.Clear();

            if (!string.IsNullOrEmpty(this.Receipt))
            {
                var param = new NavigationParameters();
                param.Add("Receipt", this.Receipt);
                param.Add("Title", this.Title);

                param.Add("NumberInvoice", numberinvoice);
                await this.NavigationService.NavigateAsync("ReceiptPage", param);
            }
        }
        public int? Quantity
        {
            get => this.quantity;
            set
            {
                this.quantity = value;
                this.RaisePropertyChanged();
            }
        }

        public string NumberQuantity
        {
            get
            {
                if (this.Quantity == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Quantity.ToString();
                }
            }

            set
            {
                try
                {
                    this.Quantity = int.Parse(value);
                    decimal price = 0;
                    if (this.Price != null) { price = this.Price.Value; }
                    if (value != null)
                    {
                        this.TotalItem = this.Quantity.Value * price;
                    }

                }
                catch
                {
                    this.Quantity = null;
                    this.TotalItem = 0;
                }
                this.RaisePropertyChanged();
            }
        }
        public decimal? Price
        {
            get => this.price;
            set
            {
                this.price = value;
                this.RaisePropertyChanged();
            }
        }

        public string NumberPrice
        {
            get
            {
                if (this.Price == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Price.ToString();
                }
            }

            set
            {
                try
                {
                    this.Price = decimal.Parse(value);
                    decimal quantity = 0;
                    if (this.Quantity != null) { quantity = this.Quantity.Value; }
                    if (value != null)
                    {
                        this.TotalItem = this.Price.Value * quantity;
                    }

                }
                catch
                {
                    this.Price = null;
                    this.TotalItem = 0;
                }
                this.RaisePropertyChanged();
            }
        }

        public decimal? Exchange
        {
            get => this.exchange;
            set
            {
                this.exchange = value;
                this.RaisePropertyChanged();
            }
        }
        public decimal? Cash
        {
            get => this.cash;
            set
            {
                this.cash = value;
                this.RaisePropertyChanged();
            }
        }

        public string NumberCash
        {
            get
            {
                if (this.Cash == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Cash.ToString();
                }
            }

            set
            {
                try
                {
                    this.Cash = int.Parse(value);
                    decimal total = 0;
                    if (this.Total != null) { total = this.Total.Value; }
                    if (value != null)
                    {
                        this.Exchange = total - this.Cash.Value;
                    }

                }
                catch
                {
                    this.Cash = null;
                    this.Exchange = 0;
                }
                this.RaisePropertyChanged();
            }
        }
        public decimal? Total
        {
            get => this.total;
            set
            {
                this.total = value;
                this.RaisePropertyChanged();
            }
        }

        public decimal TotalItem
        {
            get => this.totalitem;
            set
            {
                this.totalitem = value;
                this.RaisePropertyChanged();
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.RaisePropertyChanged();
            }
        }

        public string NameProduct
        {
            get => this.nameProduct;
            set
            {
                this.nameProduct = value;
                this.RaisePropertyChanged();
            }
        }

        public int? IdProduct
        {
            get => this.idProduct;
            set
            {
                this.idProduct = value;
                this.RaisePropertyChanged();
            }
        }
        public async Task<string> ReceiptGenerateAsyc()
        {
            string reci = string.Empty;
            try
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
            var exchangenewx = this.Exchange != null ? this.Exchange.Value.ToString("C2") : value.ToString("C2");
            var cashnew = this.Cash != null ? this.Cash.Value.ToString("C2") : value.ToString("C2");
             reci = @" <!DOCTYPE html>
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
            foreach (var item in this.DetailInvoices)
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
<td style = 'padding-right:4px;text-align:center;color:black;font-weight: bold;'> Total:</td><td style = 'padding-right:4px;text-align:center;'>" + this.Total.Value.ToString("C2") + @" </td>
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
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError($"{e.Message} ", ToastLength.Long); 
            }
            return reci;
        }

        public string Receipt
        {
            get => this.receipt;
            set
            {
                this.receipt = value;
                this.RaisePropertyChanged();
            }
        }

        public string TypeInvoice
        {
            get => this.typeInvoice;
            set
            {
                this.typeInvoice = value;
                this.RaisePropertyChanged();
            }
        }

        public string TypeDecription
        {
            get => this.typeDecription;
            set
            {
                this.typeDecription = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }

        public CustomersModel Customer
        {
            get => this.customer;
            set
            {
                this.customer = value;
                this.RaisePropertyChanged();
            }
        }

        public ProvidersModel Provider
        {
            get => this.provider;
            set
            {
                this.provider = value;
                this.RaisePropertyChanged();
            }
        }

        public ProductsModel Product
        {
            get => this.product;
            set
            {
                this.product = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
