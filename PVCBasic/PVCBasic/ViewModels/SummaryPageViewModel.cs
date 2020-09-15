using Prism.Navigation;
using Prism.Services;
using PVCBasic.PVCBCore.Invoices;
using PVCBasic.ConstantName;
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
        private decimal box;
        private string boxTextColor;
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
            var purchase = data.Where(w => w.InvoicesTypes == ConstantName.ConstantName.Purchases);
            var sales = data.Where(w => w.InvoicesTypes == ConstantName.ConstantName.Sales);
            this.Date = DateTime.Now;
            this.PurchaseQuantity = purchase.Count();
            this.TotalPurchases = purchase.Sum(s => s.Total);
            this.SalesQuantity = sales.Count();
            this.TotalSales = sales.Sum(s => s.Total);
            this.Box = this.TotalSales - this.TotalPurchases;
            if (this.Box < 0)
            {
                this.BoxTextColor = "#FC0505";
            }
            else
            {
                this.BoxTextColor = "#0561FC";
            }
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

        public decimal Box
        {
            get => this.box;
            set
            {
                this.box = value;
                this.RaisePropertyChanged();
            }
        }

        public string BoxTextColor
        {
            get => this.boxTextColor;
            set
            {
                this.boxTextColor = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
