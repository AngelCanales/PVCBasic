using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class SearchProductPageViewModel : BaseViewModel
    {
        private readonly IProductsManager productsManager;
        private ObservableCollection<ProductsModel> listOfProducts;
        private List<ProductsModel> listOfProductsbackup;
        private List<ProductsModel> listProducts;
        private string searchText;
        private bool isBusySearchBar;
        private bool isBusy;
        private bool isError;
        private string description;
        private CustomersModel customer;
        private ProvidersModel provider;
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;

        public SearchProductPageViewModel(INavigationService navigationService, IProductsManager productsManager) : base(navigationService)
        {
            this.productsManager = productsManager;
            this.DetailInvoices = new ObservableCollection<DetailInvoicesViewModel>();
            this.DetailProductCommand = new DelegateCommand<ProductsModel>(async (c) => await this.ExecuteDetailProductCommand(c));
            this.SearchCommand = new DelegateCommand(async () => await this.PerformSearch());
            this.AddCommand = new DelegateCommand(async () => await this.ExecuteAddCommand());

            this.ListOfProducts = new ObservableCollection<ProductsModel>();
            this.ListOfProductsbackup = new List<ProductsModel>();
            this.ListProducts = new List<ProductsModel>();
            this.Title = "Seleccionar Productos";
            this.IsBusySearchBar = false;
            this.IsError = false;
        }


        private async Task ExecuteAddCommand()
        {
            var param = new NavigationParameters();
            await this.NavigationService.NavigateAsync("AddProductPage");
        }

        public ICommand DetailProductCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ICommand AddCommand { get; set; }

        private async Task ExecuteDetailProductCommand(ProductsModel selectedProduct)
        {
            var param = new NavigationParameters();
            param.Add("SelectedProduct", selectedProduct);
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
                var data = this.ListProducts.ToList()
                    .Where(c => c.Code.ToUpper().Contains(this.SearchText.ToUpper()) ||
                    c.Name.ToUpper().Contains(this.SearchText.ToUpper()) ||
                    c.ShortName.ToUpper().Contains(this.SearchText.ToUpper()))
                    .Select(s => new ProductsModel
                    {
                        Id = s.Id,
                        Cost = s.Cost.Value,
                        Name = s.Name,
                        ShortName = s.ShortName,
                        Code = s.Code,
                        Tax = s.Tax,
                        Price = s.Price.Value,
                        Image = s.Image,
                        Discount = s.Discount,
                        Date = s.Date
                    });

                if (data.Any())
                {

                    this.ListOfProducts.Clear();
                    foreach (var item in data.OrderBy(c => c.Name))
                    {
                        this.ListOfProducts.Add(item);
                    }
                }
                else
                {
                    this.ListOfProducts.Clear();
                    foreach (var item in this.listOfProductsbackup.OrderBy(c => c.Name))
                    {
                        this.ListOfProducts.Add(item);
                    }
                }
            }
            else
            {
                this.ListOfProducts.Clear();
                foreach (var item in this.listOfProductsbackup.OrderBy(c => c.Name))
                {
                    this.ListOfProducts.Add(item);
                }
            }
        }

        private async Task LoadAsync()
        {
            this.ListOfProducts.Clear();
            this.ListProducts.Clear();
            this.ListOfProductsbackup.Clear();
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
            this.ListOfProducts = new ObservableCollection<ProductsModel>();

            await Task.Yield();
            await Task.Delay(500);
            this.IsBusy = true;


            var productsList = new ObservableCollection<ProductsModel>();
            try
            {
                var plist = await productsManager.GetAllAsync();
                var data = plist.Select(s => new ProductsModel
                {
                    Id = s.Id,
                    Cost = s.Cost,
                    Name = s.Name,
                    ShortName = s.ShortName,
                    Code = s.Code,
                    Tax = s.Tax,
                    Price = s.Price,
                    Image = s.Image,
                    Discount = s.Discount,
                    Date = s.Date
                });

                foreach (var item in data)
                {
                    productsList.Add(item);
                }

                await Task.Yield();

                this.ListOfProducts =
                   new ObservableCollection<ProductsModel>(
                       productsList
                           .OrderBy(c => c.Name));

                this.ListProducts =
                      new List<ProductsModel>(
                          productsList
                              .OrderBy(c => c.Name));

                this.ListOfProductsbackup =
                       new List<ProductsModel>(
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
            try
            {

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
            catch (Exception)
            {

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

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
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

        public ObservableCollection<ProductsModel> ListOfProducts
        {
            get => this.listOfProducts;
            set
            {
                this.SetProperty(ref this.listOfProducts, value);
            }
        }

        public List<ProductsModel> ListOfProductsbackup
        {
            get => this.listOfProductsbackup;
            set
            {
                this.SetProperty(ref this.listOfProductsbackup, value);
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

        public List<ProductsModel> ListProducts
        {
            get => this.listProducts;
            set
            {
                this.SetProperty(ref this.listProducts, value);
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

    }
}
