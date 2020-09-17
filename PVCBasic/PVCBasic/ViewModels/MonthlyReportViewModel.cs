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

        public MonthlyReportViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            this.Title = "Reporte Mensuak";
            this.DetailInvoices = new ObservableCollection<MonthlyReportModel>();
            this.DateSelectedCommand = new DelegateCommand(async () => await this.ExecuteDateSelectedCommand());

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.Date = DateTime.Now;
            await GetDataAsync();
        }

        public ICommand FinishedCommand { get; set; }


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

        private async Task GetDataAsync()
        {
            var data = await invoicesManager.GetAllByDateYearAsync(new DateTime(this.Date.Year, 1, 1), new DateTime(this.Date.Year, 12, 31));

            var groupSales = data.Where(c => c.InvoicesTypes == "V")
                                  .GroupBy(g => g.Date.Month)
                                  .Select(s => new MonthlyReportModel
                                  {
                                      mes = s.Key,
                                      TotalSales = s.Sum(d => d.Total),
                                      TypeInvoice = "V",
                                      Date = s.FirstOrDefault().Date,
                                  });

            var groupPurchase = data.Where(c => c.InvoicesTypes == "C")
                                  .GroupBy(g => g.Date.Month)
                                  .Select(s => new MonthlyReportModel
                                  {
                                      mes = s.Key,
                                      TotalPurchase = s.Sum(d => d.Total),
                                      TypeInvoice = "C",
                                      Date = s.FirstOrDefault().Date,
                                  });

            var result = groupSales.Join(groupPurchase,
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
