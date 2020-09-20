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
        private readonly IInvoicesManager invoicesManager;
        public SalesViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
            this.SalvarCommand = new DelegateCommand(async () => await this.ExecuteSalvarCommand());
            this.AddItemCommand = new DelegateCommand(async () => await this.ExecuteAddItemCommand());
            this.UpdateItemCommand = new DelegateCommand(async () => await this.ExecuteUpdateItemCommand());
            this.DeleteItemCommand = new DelegateCommand(async () => await this.ExecuteDeleteItemCommand());
            this.ClearItemsCommand = new DelegateCommand(async () => await this.ExecuteClearItemsCommand());
            this.SearchProductCommand = new DelegateCommand(async () => await this.ExecuteSearchProductCommand());

            
        }

        private async Task ExecuteSearchProductCommand()
        {
            await this.NavigationService.NavigateAsync("SearchProductPage");
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("TypeInvoice"))
            {
                this.TypeInvoice = parameters["TypeInvoice"] as string;
            }

            if (parameters.ContainsKey("Title"))
            {
                this.Title = parameters["Title"] as string;
            }

            if (parameters.ContainsKey("SelectedProduct"))
            {
                var product = parameters["SelectedProduct"] as ProductsModel;
              //  CrossToastPopUp.Current.ShowToastSuccess($"price :{product.Price.Value.ToString()}", ToastLength.Long);
              if (this.TypeInvoice == ConstantName.ConstantName.Sales) 
                { 
                this.NumberPrice = product.Price.Value.ToString();
                this.Price = product.Price.Value;
                this.NameProduct = product.ShortName;
                }
                if (this.TypeInvoice == ConstantName.ConstantName.Purchases)
                {
                    this.NumberPrice = product.Cost.Value.ToString();
                    this.Price = product.Cost.Value;
                    this.NameProduct = product.ShortName;
                }
            }

            if (parameters.ContainsKey("DetailInvoices"))
            {
                this.DetailInvoices = parameters["DetailInvoices"] as ObservableCollection<DetailInvoicesViewModel>;
                this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
            }

        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add("DetailInvoices", this.DetailInvoices);
            parameters.Add("Title", this.Title);
            parameters.Add("TypeInvoice", this.TypeInvoice);
        }

        public DetailInvoicesViewModel SelectedItemDetails { get; set; }

        public ICommand AddItemCommand { get; set; }

        public ICommand ClearItemsCommand { get; set; }

        public ICommand SearchProductCommand { get; set; }

        private async Task ExecuteClearItemsCommand()
        {
            this.DetailInvoices.Clear();
            this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
        }

        private async Task ExecuteAddItemCommand()
        { 
            if(this.Quantity== null) { return; };
            if (this.Price == null) { return; };

            var des = $"{this.NameProduct} => {this.Quantity.Value} x {this.Price.Value} = {this.TotalItem.ToString("C2")}";
            var item = new DetailInvoicesViewModel
            {
                Id =  Guid.NewGuid(),
                TotalItem = this.Quantity.Value * this.Price.Value,
                Description =  des,
                ProductName = this.NameProduct,
                Price = this.Price.Value,
                Quantity = this.Quantity.Value,
            };
            this.DetailInvoices.Add(item);
            this.Total = this.DetailInvoices.Sum(s => s.TotalItem);
            this.NameProduct = "";
            this.TotalItem = 0;
            this.NumberQuantity = string.Empty;
            this.NumberPrice = string.Empty; ;
            this.NumberCash = string.Empty; ;
            this.Exchange = null;
        }

        public ICommand UpdateItemCommand { get; set; }

        private async Task ExecuteUpdateItemCommand()
        {
            foreach (var item in this.DetailInvoices)
            {
                if( item.Id == this.SelectedItemDetails.Id) 
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
            var invoice = new Invoices();
            var detail = this.DetailInvoices.Select(s => new DetailInvoices { IdInvoices = invoice.Id, Invoices = invoice, Description = s.Description, TotalItem = s.TotalItem}).ToList();
            invoice.Date = DateTime.Now;
            invoice.Description = this.Description;
            invoice.InvoicesTypes = this.TypeInvoice;
            invoice.Total = this.DetailInvoices.Sum(t => t.TotalItem);
            invoice.DetailInvoices = detail;
         //   invoice.InvoicesTypes = "V";
           await this.invoicesManager.CreateAsync(invoice);

            
            CrossToastPopUp.Current.ShowToastSuccess($"Se guardo en: {this.Title}", ToastLength.Long);
            this.Total = 0;
            this.TotalItem = 0;
            this.NameProduct = string.Empty;
            this.Description = string.Empty;
            this.Quantity =  null;
            this.Price = null;
            this.Cash = null;
            this.NumberQuantity = "";
            this.NumberPrice = null; 
            this.NumberCash = string.Empty; 
            this.Exchange = null;
            this.DetailInvoices.Clear();
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

        public string TypeInvoice
        {
            get => this.typeInvoice;
            set
            {
                this.typeInvoice = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }
    }
}
