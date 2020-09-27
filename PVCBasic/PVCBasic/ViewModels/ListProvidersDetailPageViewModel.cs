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
    public class ListProvidersDetailPageViewModel : BaseViewModel
    {
       

        private ProvidersModel provider;
        private bool isError;
        private readonly IProvidersManager providersManager;
        public ListProvidersDetailPageViewModel(INavigationService navigationService, IProvidersManager providersManager) : base(navigationService)
        {
            this.providersManager = providersManager;
            this.SaveCommand = new DelegateCommand(async () => await this.ExecuteSaveCommand());
            this.IsError = false;
            this.DeleteCommand = new DelegateCommand(async () => await this.ExecuteDeleteCommand());
            this.Provider = new ProvidersModel();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("SelectedProvider"))
            {
                this.Provider = parameters["SelectedProvider"] as ProvidersModel;
                
            }
            this.Title = "Editar/Eliminar Proveedor";

        }

        public ICommand DeleteCommand { get; set; }
        private async Task ExecuteDeleteCommand()
        {

            var provider = await providersManager.FindByIdAsync(this.Provider.Id);
            await providersManager.DeleteAsync(provider);
            await this.NavigationService.GoBackAsync();
        }

        private async Task ExecuteSaveCommand()
        {
            this.IsError = true;
            try
            {
                var providernew = await providersManager.FindByIdAsync(this.Provider.Id);
                providernew.Code = this.Provider.Code;
                providernew.Location = this.Provider.Location;
                providernew.Address = this.Provider.Address;
                providernew.Name = this.Provider.Name;
                providernew.ShortName = this.Provider.ShortName;
                providernew.ContactName = this.Provider.ContactName;
                providernew.ContactPhoneNumber = this.Provider.ContactPhoneNumber;
                providernew.City = this.Provider.City;
                providernew.PhoneNumber = this.Provider.PhoneNumber;


                await providersManager.EditAsync(providernew);
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastError(e.Message, ToastLength.Long);
            }
           

            CrossToastPopUp.Current.ShowToastSuccess($"Proveedor Editado", ToastLength.Long);
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
                this.provider = value;
                this.RaisePropertyChanged();
            }
        }
     
    }
}
