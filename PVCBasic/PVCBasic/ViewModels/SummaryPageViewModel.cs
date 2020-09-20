using Prism.Navigation;
using Prism.Services;
using PVCBasic.PVCBCore.Invoices;
using PVCBasic.ConstantName;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private bool isVisibleAnimation;
        private bool isVisibleContent;
        private bool isVisibleStarAnimation;
        private string nameFile;
        private readonly IInvoicesManager invoicesManager;
        public SummaryPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            this.Title = "Reporte Diario";

            var myCurrency = new CultureInfo("es-HN");
            CultureInfo.DefaultThreadCurrentCulture = myCurrency;
            this.IsVisibleAnimation = true;
            this.DateSelectedCommand = new DelegateCommand(async () => await this.ExecuteDateSelectedCommand());
           this.FinishedCommand = new DelegateCommand(async () => await this.ExecuteFinishedCommand());
           this.FinishedStarCommand = new DelegateCommand(async () => await this.ExecuteFinishedStarCommand());
           this.IsVisibleContent = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("LottieItem"))
            {
                this.IsVisibleStarAnimation = (bool)parameters["LottieItem"];
            }

            if (parameters.ContainsKey("NameFile"))
            {
                this.NameFile = parameters["NameFile"] as string;
            }
            this.Date = DateTime.Now;
            await GetDataAsync();
        }

        public ICommand FinishedCommand { get; set; }

        public ICommand FinishedStarCommand { get; set; }

        public bool IsVisibleAnimation
        {
            get => this.isVisibleAnimation;
            set
            {
                this.isVisibleAnimation = value;
                this.RaisePropertyChanged();
            }
        }

        public string NameFile
        {
            get => this.nameFile;
            set
            {
                this.nameFile = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsVisibleStarAnimation
        {
            get => this.isVisibleStarAnimation;
            set
            {
                this.isVisibleStarAnimation = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsVisibleContent
        {
            get => this.isVisibleContent;
            set
            {
                this.isVisibleContent = value;
                this.RaisePropertyChanged();
            }
        }

        private async Task ExecuteFinishedCommand()
        {
            this.IsVisibleAnimation = false;
            this.IsVisibleContent = true;
        }

        private async Task ExecuteFinishedStarCommand()
        {
            this.IsVisibleStarAnimation = false;
            this.IsVisibleAnimation = true;
            this.IsVisibleContent = false;
        }

        public ICommand DateSelectedCommand { get; set; }
        private async Task ExecuteDateSelectedCommand()
        {
            await GetDataAsync();
        }

        private async Task GetDataAsync()
        {
            var data = await invoicesManager.GetAllByDateAsync(this.Date);
            var purchase = data.Where(w => w.InvoicesTypes == ConstantName.ConstantName.Purchases);
            var sales = data.Where(w => w.InvoicesTypes == ConstantName.ConstantName.Sales);
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
