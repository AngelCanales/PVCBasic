using Prism.Navigation;
using Prism.Services;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PVCBasic.ViewModels
{
   public class SummaryPageViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        private DateTime date;
        private decimal totalPurchases;
        private int purchaseQuantity;
        private decimal totalSales;
        private int salesQuantity;
        private readonly IInvoicesManager invoicesManager;
        public SummaryPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            this.Title = "Reporte Diario";

            var myCurrency = new CultureInfo("es-HN");
            CultureInfo.DefaultThreadCurrentCulture = myCurrency;

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var data = await invoicesManager.GetAllByDateAsync(DateTime.Now);
            var purchase = data.Where(w => w.InvoicesTypes == "C");
            var sales = data.Where(w => w.InvoicesTypes == "V");
            this.Date = DateTime.Now;
            this.PurchaseQuantity = purchase.Count();
            this.TotalPurchases = purchase.Sum(s => s.Total);
            this.SalesQuantity = sales.Count();
            this.TotalSales = sales.Sum(s => s.Total);

        }
        public int SalesQuantity
        {
            get => this.salesQuantity;
            set
            {
                this.salesQuantity = value;
                this.RaisePropertyChanged();
            }
        }

        public decimal TotalSales
        {
            get => this.totalSales;
            set
            {
                this.totalSales = value;
                this.RaisePropertyChanged();
            }
        }

        public int PurchaseQuantity
        {
            get => this.purchaseQuantity;
            set
            {
                this.purchaseQuantity = value;
                this.RaisePropertyChanged();
            }
        }

        public decimal TotalPurchases
        {
            get => this.totalPurchases;
            set
            {
                this.totalPurchases = value;
                this.RaisePropertyChanged();
            }
        }

        public DateTime Date
        {
            get => this.date;
            set
            {
                this.date = value;
                this.RaisePropertyChanged();
            }
        }

    }
}
