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

    public class ListProductsPageViewModel : BaseViewModel
    {
        private readonly IProductsManager productsManager;
        private ObservableCollection<ProductsModel> listOfProducts;
        private List<ProductsModel> listOfProductsbackup;
        private List<ProductsModel> listProducts;
        private string searchText;
        private bool isBusySearchBar;
        private bool isBusy;

        public ListProductsPageViewModel(INavigationService navigationService, IProductsManager productsManager) : base(navigationService)
        {
            this.productsManager = productsManager;

            this.DetailProductCommand = new DelegateCommand<ProductsModel>(async (c) => await this.ExecuteDetailProductCommand(c));
            this.SearchCommand = new DelegateCommand(async () => await this.PerformSearch());

            this.ListOfProducts = new ObservableCollection<ProductsModel>();
            this.ListOfProductsbackup = new List<ProductsModel>();
            this.ListProducts = new List<ProductsModel>();
            this.IsBusySearchBar = false;
        }

        public ICommand DetailProductCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private async Task ExecuteDetailProductCommand(ProductsModel selectedProduct)
        {
            var param = new NavigationParameters();
            param.Add("SelectedProduct", selectedProduct);
            await this.NavigationService.NavigateAsync("ListProductsDetailPage", param);
        }

        public async Task PerformSearch()
        {
            await this.FilterDataAsync();
        }

        private async Task FilterDataAsync()
        {
            if (!string.IsNullOrEmpty(this.SearchText))
            {
                var data = this.ListProducts.ToList()
                    .Where(c => c.Code.ToUpper().Contains(this.SearchText.ToUpper()) || c.Name.ToUpper().Contains(this.SearchText.ToUpper()) || c.ShortName.ToUpper().Contains(this.SearchText.ToUpper()) || c.Code.ToUpper().Contains(this.SearchText.ToUpper()))
                    .Select(s => new ProductsModel
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
                var data  = plist.Select(s => new ProductsModel
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
            OnPropertyChanged("SearchText");
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
