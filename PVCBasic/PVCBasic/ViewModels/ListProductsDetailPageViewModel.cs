using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Database.Models;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class ListProductsDetailPageViewModel : BaseViewModel
    {
        private ProductsModel product;
        private bool isError;
        private readonly IProductsManager productsManager;
        public ListProductsDetailPageViewModel(INavigationService navigationService, IProductsManager productsManager) : base(navigationService)
        {
            this.productsManager = productsManager;
            this.SaveCommand = new DelegateCommand(async () => await this.ExecuteSaveCommand());
            this.IsError = false;
            this.DeleteCommand = new DelegateCommand(async () => await this.ExecuteDeleteCommand());
            this.Product = new ProductsModel();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("SelectedProduct"))
            {
                this.Product = parameters["SelectedProduct"] as ProductsModel;
                this.NumberCost = this.Product.Cost.Value.ToString();
                this.NumberPrice = this.Product.Price.Value.ToString();
            }
            this.Title = "Editar/Eliminar Producto";

        }

        public ICommand DeleteCommand { get; set; }
        private async Task ExecuteDeleteCommand()
        {

            var product = await productsManager.FindByIdAsync(this.Product.Id);
            await productsManager.DeleteAsync(product);
            await this.NavigationService.GoBackAsync();
        }

        private async Task ExecuteSaveCommand()
        {
            this.IsError = true;
            var product = await productsManager.FindByIdAsync(this.Product.Id);
            product.Code = this.Product.Code;
            product.Cost = this.Product.Cost.Value;
            product.Price = this.Product.Price.Value;
            product.Name = this.Product.Name;
            product.ShortName = this.Product.ShortName;

            await productsManager.EditAsync(product);

            CrossToastPopUp.Current.ShowToastSuccess($"Producto Editado", ToastLength.Long);
            this.IsError = false;
            await this.NavigationService.GoBackAsync();
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

        public ICommand SaveCommand { get; set; }

        public ProductsModel Product
        {
            get => this.product;
            set
            {
                this.SetProperty(ref this.product, value);
            }
        }

        public string NumberPrice
        {
            get
            {
                if (this.Product.Price == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Product.Price.ToString();
                }
            }

            set
            {
                try
                {
                    this.Product.Price = decimal.Parse(value);


                }
                catch
                {
                    this.Product.Price = null;

                }
                this.RaisePropertyChanged();
            }
        }

        public string NumberCost
        {
            get
            {
                if (this.Product.Cost == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Product.Cost.ToString();
                }
            }

            set
            {
                try
                {
                    this.Product.Cost = decimal.Parse(value);
                }
                catch
                {
                    this.Product.Cost = null;

                }
                this.RaisePropertyChanged();
            }
        }
    }
}
