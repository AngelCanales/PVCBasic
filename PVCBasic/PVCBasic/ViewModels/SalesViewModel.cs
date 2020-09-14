using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PVCBasic.Database;
using PVCBasic.Database.Models;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        private int? quantity;
        private decimal total;
        private string description;
        private string nameProduct;
        private string typeInvoice;
        private decimal? price;
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private decimal totalitem;
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
        }

        public DetailInvoicesViewModel SelectedItemDetails { get; set; }

        public ICommand AddItemCommand { get; set; }

        public ICommand ClearItemsCommand { get; set; }

        private async Task ExecuteClearItemsCommand()
        {
            this.DetailInvoices.Clear();
        }

        private async Task ExecuteAddItemCommand()
        { 
            if(this.Quantity== null) { return; };
            if (this.Price == null) { return; };

            var des = $"{this.NameProduct} -> {this.Quantity.Value} x {this.Price.Value} = {this.TotalItem}";
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
        }

        public ICommand UpdateItemCommand { get; set; }

        private async Task ExecuteUpdateItemCommand()
        {
            foreach (var item in this.DetailInvoices)
            {
                if( item.Id == this.SelectedItemDetails.Id) 
                {
                    var des = $"{this.NameProduct} -> {this.Quantity} x {this.Price} = {this.TotalItem}";

                    item.Description = des;
                    item.ProductName = this.NameProduct;
                    item.Quantity = this.Quantity.Value;
                    item.Price = this.Price.Value;
                }
            }
        }

        public ICommand DeleteItemCommand { get; set; }

        private async Task ExecuteDeleteItemCommand()
        {
            this.DetailInvoices.Remove(this.SelectedItemDetails);
        }

        public ICommand SalvarCommand { get; set; }

        private async Task ExecuteSalvarCommand()
        {
            await this.SalvarAsync();
        }

        public async Task SalvarAsync()
        {
            var invoice = new Invoices();
            var detail = this.DetailInvoices.Select(s => new DetailInvoices { Description = s.Description, TotalItem = s.TotalItem}).ToList();
            invoice.Date = DateTime.Now;
            invoice.Description = this.Description;
            invoice.InvoicesTypes = this.TypeInvoice;
            invoice.Total = this.DetailInvoices.Sum(t => t.TotalItem);
            invoice.DetailInvoices = detail;
           await this.invoicesManager.CreateAsync(invoice);
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

        public decimal? Price
        {
            get => this.price;
            set
            {
                this.price = value;
                this.RaisePropertyChanged();
            }
        }
        public decimal Total
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
