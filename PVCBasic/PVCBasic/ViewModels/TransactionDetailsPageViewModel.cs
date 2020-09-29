using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
   public class TransactionDetailsPageViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        private decimal total;
        private InvoicesViewModel invoices;
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private readonly IInvoicesManager invoicesManager;
        public TransactionDetailsPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            this.Title = "Detalle";

            this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
            var myCurrency = new CultureInfo("es-HN");
            CultureInfo.DefaultThreadCurrentCulture = myCurrency;

            this.DeleteCommand = new DelegateCommand(async () => await this.ExecuteDeleteCommand());
            this.ReceiptCommand = new DelegateCommand(async () => await this.ExecuteReceiptCommand());
        }

        private async Task ExecuteReceiptCommand()
        {
            var param = new NavigationParameters();
            // param.Add("Receipt", this.Receipt);
            string title = string.Empty;
            if (this.Invoices.InvoicesTypes == ConstantName.ConstantName.Purchases)
            {
                title = "Compras";
            }

            if (this.Invoices.InvoicesTypes == ConstantName.ConstantName.Sales)
            {
                title = "Ventas";
            }
            param.Add("Title", title );

            param.Add("NumberInvoice", this.Invoices.NumFactura);
            await this.NavigationService.NavigateAsync("ReceiptPage", param);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Invoice"))
            {
                this.Invoices = parameters["Invoice"] as InvoicesViewModel;
            }
            
            await GetDataAsync();
        }


        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }
        public ICommand DeleteCommand { get; set; }

        public ICommand ReceiptCommand { get; set; }

        private async Task ExecuteDeleteCommand()
        {
            if (this.Invoices == null) { return; };
            var invoicedelete = await invoicesManager.FindByIdAsync(this.Invoices.Id);
            await invoicesManager.DeleteAsync(invoicedelete);

            await this.NavigationService.GoBackAsync();
        }

        private async Task GetDataAsync()
        {
            var data = await invoicesManager.FindByIdAsync(this.Invoices.Id);

           
            var collection = data.DetailInvoices.Select(s => new DetailInvoicesViewModel
            {
                Description = s.Description,
                IdD = s.Id,
                TotalItem = s.TotalItem,
            }).ToList();

            this.DetailInvoices.Clear();
            foreach (var item in collection)
            {
                this.DetailInvoices.Add(item);
            }

            this.SumTotal = this.DetailInvoices.Sum(s => s.TotalItem);
        }


        public InvoicesViewModel Invoices
        {
            get => this.invoices;
            set => this.SetProperty(ref this.invoices, value);
        }
        public decimal SumTotal
        {
            get => this.total;
            set
            {
                this.total = value;
                this.RaisePropertyChanged();
            }
        }

    }
}
