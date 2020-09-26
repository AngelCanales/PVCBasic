using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Database.Models;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Products;
using PVCBasic.PVCBCore.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class AddProviderPageViewModel : BaseViewModel
    {
      
        private ProvidersModel provider;
        private bool isError;
        private readonly IProvidersManager providersManager;
        public AddProviderPageViewModel(INavigationService navigationService, IProvidersManager providersManager) : base(navigationService)
        {
            this.providersManager = providersManager;
            this.SaveCommand = new DelegateCommand(async () => await this.ExecuteSaveCommand());
            this.IsError = false;
            this.Provider = new ProvidersModel();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            this.Title = "Agregar Proveedor";

        }

        private async Task ExecuteSaveCommand()
        {
            this.IsError = true;
            var providernew = new Providers();
            providernew.Code = this.Provider.Code;
            providernew.Location = this.Provider.Location;
            providernew.Address = this.Provider.Address;
            providernew.Name = this.Provider.Name;
            providernew.ShortName = this.Provider.ShortName;
            providernew.DateCreate = DateTime.Now;
            providernew.ContactName = this.Provider.ContactName;
            providernew.ContactPhoneNumber = this.Provider.ContactPhoneNumber;
            providernew.City = this.Provider.City;
            providernew.PhoneNumber = this.Provider.PhoneNumber;


            await providersManager.CreateAsync(providernew);

            CrossToastPopUp.Current.ShowToastSuccess($"Proveedor creado", ToastLength.Long);
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

        public ProvidersModel Provider
        {
            get => this.provider;
            set
            {
                this.SetProperty(ref this.provider, value);
            }
        }

     
    }
}
