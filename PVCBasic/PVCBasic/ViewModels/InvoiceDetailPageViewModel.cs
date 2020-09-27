using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class InvoiceDetailPageViewModel : BaseViewModel
    {
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private string typeInvoice;
        private string titleInvoice;
        private ProvidersModel provider;
        private CustomersModel customer;

        public InvoiceDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
            this.DeleteItemCommand = new DelegateCommand(async () => await this.ExecuteDeleteItemCommand());
            this.ClearItemsCommand = new DelegateCommand(async () => await this.ExecuteClearItemsCommand());

            this.Title = "Detalle";
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
                this.TitleInvoice = parameters["Title"] as string;
            }


            if (parameters.ContainsKey("SelectedCustomers"))
            {
                this.Customer = parameters["SelectedCustomers"] as CustomersModel;
            }

            if (parameters.ContainsKey("SelectedProvider"))
            {
                this.Provider = parameters["SelectedProvider"] as ProvidersModel;
            }
            if (parameters.ContainsKey("DetailInvoices"))
            {
                this.DetailInvoices = parameters["DetailInvoices"] as ObservableCollection<DetailInvoicesViewModel>;
            }

        }


        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add("DetailInvoices", this.DetailInvoices);
            parameters.Add("Title", this.TitleInvoice);
            parameters.Add("TypeInvoice", this.TypeInvoice);
            parameters.Add("SelectedProvider", this.Provider);
            parameters.Add("SelectedCustomers", this.Customer);
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

        public string TitleInvoice
        {
            get => this.titleInvoice;
            set
            {
                this.titleInvoice = value;
                this.RaisePropertyChanged();
            }
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
        public DetailInvoicesViewModel SelectedItemDetails { get; set; }

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }

        public ICommand AddItemCommand { get; set; }

        public ICommand ClearItemsCommand { get; set; }

        public ICommand SearchProductCommand { get; set; }

        private async Task ExecuteClearItemsCommand()
        {
            this.DetailInvoices.Clear();
           
        }

        public ICommand DeleteItemCommand { get; set; }

        private async Task ExecuteDeleteItemCommand()
        {
            this.DetailInvoices.Remove(this.SelectedItemDetails);
        }

    }
}
