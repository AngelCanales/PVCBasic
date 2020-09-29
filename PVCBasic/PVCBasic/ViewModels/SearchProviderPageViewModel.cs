using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Products;
using PVCBasic.PVCBCore.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class SearchProviderPageViewModel : BaseViewModel
    {

        private readonly IProvidersManager providersManager;
        private ObservableCollection<ProvidersModel> listOfProviders;
        private List<ProvidersModel> listOfProvidersbackup;
        private List<ProvidersModel> listProviders;
        private string searchText;
        private bool isBusySearchBar;
        private bool isBusy;
        private bool isError;
        private ProductsModel product;
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private string typeInvoice;

        public SearchProviderPageViewModel(INavigationService navigationService, IProvidersManager providersManager) : base(navigationService)
        {
            this.providersManager = providersManager;
            this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
            this.DetailProvidersCommand = new DelegateCommand<ProvidersModel>(async (c) => await this.ExecuteDetailProviderCommand(c));
            this.SearchCommand = new DelegateCommand(async () => await this.PerformSearch());
            this.AddCommand = new DelegateCommand(async () => await this.ExecuteAddCommand());

            this.ListOfProviders = new ObservableCollection<ProvidersModel>();
            this.ListOfProvidersbackup = new List<ProvidersModel>();
            this.ListProviders = new List<ProvidersModel>();
            this.IsBusySearchBar = false;
            this.IsError = false;
            this.Title = "Lista de proveedores";
        }

        private async Task ExecuteAddCommand()
        {
            var param = new NavigationParameters();
            await this.NavigationService.NavigateAsync("AddProviderPage");
        }

        public ICommand DetailProvidersCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ICommand AddCommand { get; set; }

        private async Task ExecuteDetailProviderCommand(ProvidersModel selectedProvider)
        {
            var param = new NavigationParameters();
            param.Add("SelectedProvider", selectedProvider);
            param.Add("SelectedProduct", this.Product);
            param.Add("TypeInvoice", this.TypeInvoice);
            param.Add("DetailInvoices", this.DetailInvoices);
            await this.NavigationService.GoBackAsync(param);
        }

        public async Task PerformSearch()
        {
            await this.FilterDataAsync();
        }

        private async Task FilterDataAsync()
        {
            if (!string.IsNullOrEmpty(this.SearchText))
            {
                CrossToastPopUp.Current.ShowToastSuccess($"Buscando :{this.SearchText}", ToastLength.Long);
                var data = this.ListProviders.ToList()
                    .Where(c => c.Code.ToUpper().Contains(this.SearchText.ToUpper()) ||
                    c.Name.ToUpper().Contains(this.SearchText.ToUpper()) ||
                    c.ShortName.ToUpper().Contains(this.SearchText.ToUpper()))
                    .Select(s => new ProvidersModel
                    {
                        Id = s.Id,
                        Address = s.Address,
                        Name = s.Name,
                        ShortName = s.ShortName,
                        Code = s.Code,
                        City = s.City,
                        ContactName = s.ContactName,
                        ContactPhoneNumber = s.ContactPhoneNumber,
                        DateCreate = s.DateCreate,
                        Location = s.Location,
                        PhoneNumber = s.PhoneNumber
                    });

                if (data.Any())
                {

                    this.ListOfProviders.Clear();
                    foreach (var item in data.OrderBy(c => c.Name))
                    {
                        this.ListOfProviders.Add(item);
                    }
                }
                else
                {
                    this.ListOfProviders.Clear();
                    foreach (var item in this.listOfProvidersbackup.OrderBy(c => c.Name))
                    {
                        this.ListOfProviders.Add(item);
                    }
                }
            }
            else
            {
                this.ListOfProviders.Clear();
                foreach (var item in this.listOfProvidersbackup.OrderBy(c => c.Name))
                {
                    this.ListOfProviders.Add(item);
                }
            }
        }

        private async Task LoadAsync()
        {
            this.ListOfProviders.Clear();
            this.ListProviders.Clear();
            this.ListOfProvidersbackup.Clear();
            await this.GetProductsListAsync();
        }

        public bool IsError
        {
            get => this.isError;
            set
            {
                this.isError = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }

        public async Task GetProductsListAsync()
        {
            this.ListOfProviders = new ObservableCollection<ProvidersModel>();

            await Task.Yield();
            await Task.Delay(500);
            this.IsBusy = true;


            var productsList = new ObservableCollection<ProvidersModel>();
            try
            {
                var plist = await providersManager.GetAllAsync();
                var data = plist.Select(s => new ProvidersModel
                {
                    Id = s.Id,
                    Address = s.Address,
                    Name = s.Name,
                    ShortName = s.ShortName,
                    Code = s.Code,
                    City = s.City,
                    ContactName = s.ContactName,
                    ContactPhoneNumber = s.ContactPhoneNumber,
                    DateCreate = s.DateCreate,
                    Location = s.Location,
                    PhoneNumber = s.PhoneNumber
                });

                foreach (var item in data)
                {
                    productsList.Add(item);
                }

                await Task.Yield();

                this.ListOfProviders =
                   new ObservableCollection<ProvidersModel>(
                       productsList
                           .OrderBy(c => c.Name));

                this.ListProviders =
                      new List<ProvidersModel>(
                          productsList
                              .OrderBy(c => c.Name));

                this.ListOfProvidersbackup =
                       new List<ProvidersModel>(
                           productsList
                               .OrderBy(c => c.Name));



            }
            catch (Exception e)
            {
                var x = e.Message;
            }


            this.IsBusy = false;
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
                this.Product = parameters["SelectedProduct"] as ProductsModel;
            }

            if (parameters.ContainsKey("DetailInvoices"))
            {
                this.DetailInvoices = parameters["DetailInvoices"] as ObservableCollection<DetailInvoicesViewModel>;
            }
            await LoadAsync();
        }

        public bool IsBusySearchBar
        {
            get => this.isBusySearchBar;
            set
            {
                this.isBusySearchBar = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<ProvidersModel> ListOfProviders
        {
            get => this.listOfProviders;
            set
            {
                this.SetProperty(ref this.listOfProviders, value);
            }
        }

        public List<ProvidersModel> ListOfProvidersbackup
        {
            get => this.listOfProvidersbackup;
            set
            {
                this.SetProperty(ref this.listOfProvidersbackup, value);
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChanged("SearchText");
            }
        }

        public ProductsModel Product
        {
            get => this.product;
            set
            {
                this.product = value;
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
        public List<ProvidersModel> ListProviders
        {
            get => this.listProviders;
            set
            {
                this.SetProperty(ref this.listProviders, value);
            }
        }

    }
}
