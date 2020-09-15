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
    public class TransactionsPageViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        private DateTime date;
        private decimal total;
        private string typeInvoice;
        private ObservableCollection<InvoicesViewModel> invoices;
        private readonly IInvoicesManager invoicesManager;
        public TransactionsPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
            
            this.ListInvoices = new ObservableCollection<InvoicesViewModel>();
            var myCurrency = new CultureInfo("es-HN");
            CultureInfo.DefaultThreadCurrentCulture = myCurrency;

            this.DateSelectedCommand = new DelegateCommand(async () => await this.ExecuteDateSelectedCommand());

        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
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

            this.Date = DateTime.Now;
            await GetDataAsync();
        }

        public ICommand DateSelectedCommand { get; set; }
        private async Task ExecuteDateSelectedCommand()
        {
            await GetDataAsync();
        }

        private async Task GetDataAsync()
        {
            var data = await invoicesManager.GetAllByDateAsync(this.Date);

            var list = data.Where(w => w.InvoicesTypes == TypeInvoice);
            var collection = list.Select(s => new InvoicesViewModel
            {
                Date = s.Date,
                Description = s.Description,
                Id = s.Id,
                InvoicesTypes = s.InvoicesTypes,
                Total = s.Total
            }).ToList();

            this.ListInvoices.Clear();
            foreach (var item in collection)
            {
                this.ListInvoices.Add(item);
            }

            this.SumTotal = list.Sum(s => s.Total);
        }

        public InvoicesViewModel SelectedItemInvoice { get; set; }

        public ObservableCollection<InvoicesViewModel> ListInvoices
        {
            get => this.invoices;
            set => this.SetProperty(ref this.invoices, value);
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
