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
    public class MonthlyReportViewModel : BaseViewModel
    {
        private DateTime date;
        private decimal box;
        private string boxTextColor;
        private IPageDialogService dialogService;
        private IInvoicesManager invoicesManager;
        private ObservableCollection<MonthlyReportModel> detailInvoices;
        private decimal total;
        private bool isVisibleContent;
        private bool isVisibleAnimation;

        public MonthlyReportViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            this.Title = "Reporte Mensual";
            this.DetailInvoices = new ObservableCollection<MonthlyReportModel>();
            this.DateSelectedCommand = new DelegateCommand(async () => await this.ExecuteDateSelectedCommand());

            this.FinishedCommand = new DelegateCommand(async () => await this.ExecuteFinishedCommand());
            this.IsVisibleAnimation = true;
            this.IsVisibleContent = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.Date = DateTime.Now;
            await GetDataAsync();
        }



        public ObservableCollection<MonthlyReportModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }

        public ICommand DateSelectedCommand { get; set; }
        private async Task ExecuteDateSelectedCommand()
        {
            await GetDataAsync();
        }

        public ICommand FinishedCommand { get; set; }

        public bool IsVisibleAnimation
        {
            get => this.isVisibleAnimation;
            set
            {
                this.isVisibleAnimation = value;
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

        private async Task GetDataAsync()
        {

            var salesList = new List<MonthlyReportModel>();
            var purchasesList = new List<MonthlyReportModel>();
            for (int i = 1; i < 13; i++)
            {
                var item = new MonthlyReportModel() { mes = i , Date = new DateTime(this.Date.Year, i, 1) };
                salesList.Add(item);
                purchasesList.Add(item);
            }

            var data = await invoicesManager.GetAllByDateYearAsync(new DateTime(this.Date.Year, 1, 1), new DateTime(this.Date.Year, 12, 31));

            var groupSales = data.Where(c => c.InvoicesTypes == ConstantName.ConstantName.Sales)
                                  .GroupBy(g => g.Date.Month)
                                  .Select(s => new MonthlyReportModel
                                  {
                                      mes = s.Key,
                                      TotalSales = s.Sum(d => d.Total),
                                      TypeInvoice = ConstantName.ConstantName.Sales,
                                      Date =  s.FirstOrDefault().Date,
                                      CountSales = s.Count()
                                  });

            foreach (var item in salesList)
            {
                foreach (var g in groupSales)
                {
                    if(item.mes == g.mes)
                    {
                        item.TotalSales = g.TotalSales;
                        item.TypeInvoice = g.TypeInvoice;
                       // item.Date = g.Date;
                        item.CountSales = g.CountSales;
                    }
                }
            }

            var groupPurchase = data.Where(c => c.InvoicesTypes == ConstantName.ConstantName.Purchases)
                                  .GroupBy(g => g.Date.Month)
                                  .Select(s => new MonthlyReportModel
                                  {
                                      mes = s.Key,
                                      TotalPurchase = s.Sum(d => d.Total),
                                      TypeInvoice = ConstantName.ConstantName.Purchases,
                                      Date = s.FirstOrDefault().Date,
                                      CountPurchase = s.Count(),
                                  });

            foreach (var item in purchasesList)
            {
                foreach (var g in groupPurchase)
                {
                    if (item.mes == g.mes)
                    {
                        item.TotalPurchase = g.TotalPurchase;
                        item.TypeInvoice = g.TypeInvoice;
                      //  item.Date = g.Date;
                        item.CountPurchase = g.CountPurchase;
                    }
                }
            }
            var result = salesList.Join(purchasesList,
                sales => sales.mes,
                purchase => purchase.mes,
                (sales, purchase) => new MonthlyReportModel
                {
                    mes = sales.mes,
                    TotalPurchase = purchase.TotalPurchase,
                    TotalSales = sales.TotalSales,
                    Difference = sales.TotalSales - purchase.TotalPurchase,
                    DifferenceTextColor = (sales.TotalSales - purchase.TotalPurchase) < 0 ? "#FC0505" : "#0561FC",
                    MonthName = sales.Date.ToString("MMMM", CultureInfo.InvariantCulture),
                    CountPurchase = sales.CountPurchase,
                    CountSales = sales.CountSales
                }).ToList();

            this.DetailInvoices.Clear();
            foreach (var item in result)
            {
                this.DetailInvoices.Add(item);
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

        public decimal SumTotal
        {
            get => this.total;
            set
            {
                this.total = value;
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
