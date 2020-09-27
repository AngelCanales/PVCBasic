using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Inventories;
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
    public class InventoriesPageViewModel : BaseViewModel
    {
    
        private readonly IProductsManager productsManager;
        private ObservableCollection<ProductsModel> listOfProducts;
        private List<ProductsModel> listOfProductsbackup;
        private List<ProductsModel> listProducts;
        private string searchText;
        private bool isBusySearchBar;
        private bool isBusy;
        private bool isError;
        private readonly IInventoriesManager inventoriesManager;
        public InventoriesPageViewModel(INavigationService navigationService, IProductsManager productsManager, IInventoriesManager inventoriesManager) : base(navigationService)
        {
            this.productsManager = productsManager;
            this.inventoriesManager = inventoriesManager;
            this.SearchCommand = new DelegateCommand(async () => await this.PerformSearch());

            this.ListOfProducts = new ObservableCollection<ProductsModel>();
            this.ListOfProductsbackup = new List<ProductsModel>();
            this.ListProducts = new List<ProductsModel>();
            this.IsBusySearchBar = false;
            this.IsError = false;
            this.Title = "Inventario";
        }

        public ICommand SearchCommand { get; set; }

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
                        Date = s.Date,
                        Existence = s.Existence,
                        ExistenceText = s.ExistenceText,
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
                var exit = await inventoriesManager.GetAllAsync();

                var plistInven = plist.Join(exit, p => p.Id, e => e.IdProduct, (p, e) => new { p, e });
                var data = plistInven.Select(s => new ProductsModel
                {
                    Id = s.p.Id,
                    Cost = s.p.Cost,
                    Name = s.p.Name,
                    ShortName = s.p.ShortName,
                    Code = s.p.Code,
                    Tax = s.p.Tax,
                    Price = s.p.Price,
                    Image = s.p.Image,
                    Discount = s.p.Discount,
                    Date = s.p.Date,
                    Existence = s.e.Existence,
                    ExistenceText = $"Existencia: {s.e.Existence}",

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

            if (parameters.ContainsKey("Title"))
            {
                this.Title = parameters["Title"] as string;
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

    }
}
