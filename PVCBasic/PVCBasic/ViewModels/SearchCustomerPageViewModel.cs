using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Customers;
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
    public class SearchCustomerPageViewModel : BaseViewModel
    {
     
        private readonly ICustomersManager customersManager;
        private ObservableCollection<CustomersModel> listOfCustomers;
        private List<CustomersModel> listOfCustomersbackup;
        private List<CustomersModel> listCustomers;
        private string searchText;
        private bool isBusySearchBar;
        private bool isBusy;
        private bool isError;
        private ProductsModel product;
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private string typeInvoice;

        public SearchCustomerPageViewModel(INavigationService navigationService, ICustomersManager customersManager) : base(navigationService)
        {
            this.customersManager = customersManager;
            this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
            this.DetailCustomersCommand = new DelegateCommand<CustomersModel>(async (c) => await this.ExecuteDetailCustomersCommand(c));
            this.SearchCommand = new DelegateCommand(async () => await this.PerformSearch());
            this.AddCommand = new DelegateCommand(async () => await this.ExecuteAddCommand());

            this.ListOfCustomers = new ObservableCollection<CustomersModel>();
            this.ListOfCustomersbackup = new List<CustomersModel>();
            this.ListCustomers = new List<CustomersModel>();
            this.IsBusySearchBar = false;
            this.IsError = false;
            this.Title = "Lista de Clientes";
        }

        private async Task ExecuteAddCommand()
        {
            var param = new NavigationParameters();
            await this.NavigationService.NavigateAsync("AddCustomersPage");
        }

        public ICommand DetailCustomersCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ICommand AddCommand { get; set; }

        private async Task ExecuteDetailCustomersCommand(CustomersModel selectedProduct)
        {
            var param = new NavigationParameters();
            param.Add("SelectedCustomers", selectedProduct);
            param.Add("SelectedProduct", this.Product);
            param.Add("DetailInvoices", this.DetailInvoices);
            param.Add("TypeInvoice", this.TypeInvoice);
            
            await this.NavigationService.GoBackAsync(param);
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
        public async Task PerformSearch()
        {
            await this.FilterDataAsync();
        }

        private async Task FilterDataAsync()
        {
            if (!string.IsNullOrEmpty(this.SearchText))
            {
                CrossToastPopUp.Current.ShowToastSuccess($"Buscando :{this.SearchText}", ToastLength.Long);
                var data = this.ListCustomers.ToList()
                    .Where(c => c.Code.ToUpper().Contains(this.SearchText.ToUpper()) ||
                    c.Name.ToUpper().Contains(this.SearchText.ToUpper()) ||
                    c.ShortName.ToUpper().Contains(this.SearchText.ToUpper()))
                    .Select(s => new CustomersModel
                    {
                        Id = s.Id,
                        Address = s.Address,
                        Name = s.Name,
                        ShortName = s.ShortName,
                        Code = s.Code,
                        City = s.City,
                        DateCreate = s.DateCreate,
                        Location = s.Location,
                        PhoneNumber = s.PhoneNumber
                    });

                if (data.Any())
                {

                    this.ListOfCustomers.Clear();
                    foreach (var item in data.OrderBy(c => c.Name))
                    {
                        this.ListOfCustomers.Add(item);
                    }
                }
                else
                {
                    this.ListOfCustomers.Clear();
                    foreach (var item in this.listOfCustomersbackup.OrderBy(c => c.Name))
                    {
                        this.ListOfCustomers.Add(item);
                    }
                }
            }
            else
            {
                this.ListOfCustomers.Clear();
                foreach (var item in this.listOfCustomersbackup.OrderBy(c => c.Name))
                {
                    this.ListOfCustomers.Add(item);
                }
            }
        }

        private async Task LoadAsync()
        {
            this.ListOfCustomers.Clear();
            this.ListCustomers.Clear();
            this.ListOfCustomersbackup.Clear();
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

        public async Task GetProductsListAsync()
        {
            this.ListOfCustomers = new ObservableCollection<CustomersModel>();

            await Task.Yield();
            await Task.Delay(500);
            this.IsBusy = true;


            var productsList = new ObservableCollection<CustomersModel>();
            try
            {
                var plist = await customersManager.GetAllAsync();
                var data = plist.Select(s => new CustomersModel
                {
                    Id = s.Id,
                    Address = s.Address,
                    Name = s.Name,
                    ShortName = s.ShortName,
                    Code = s.Code,
                    City = s.City,
                    DateCreate = s.DateCreate,
                    Location = s.Location,
                    PhoneNumber = s.PhoneNumber
                });

                foreach (var item in data)
                {
                    productsList.Add(item);
                }

                await Task.Yield();

                this.ListOfCustomers =
                   new ObservableCollection<CustomersModel>(
                       productsList
                           .OrderBy(c => c.Name));

                this.ListCustomers =
                      new List<CustomersModel>(
                          productsList
                              .OrderBy(c => c.Name));

                this.ListOfCustomersbackup =
                       new List<CustomersModel>(
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

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
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

        public string TypeInvoice
        {
            get => this.typeInvoice;
            set
            {
                this.typeInvoice = value;
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

        public ObservableCollection<CustomersModel> ListOfCustomers
        {
            get => this.listOfCustomers;
            set
            {
                this.SetProperty(ref this.listOfCustomers, value);
            }
        }

        public List<CustomersModel> ListOfCustomersbackup
        {
            get => this.listOfCustomersbackup;
            set
            {
                this.SetProperty(ref this.listOfCustomersbackup, value);
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

        public List<CustomersModel> ListCustomers
        {
            get => this.listCustomers;
            set
            {
                this.SetProperty(ref this.listCustomers, value);
            }
        }
    }
}
