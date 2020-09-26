using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Database.Models;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Customers;
using PVCBasic.PVCBCore.Products;
using PVCBasic.PVCBCore.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class ListCustomersDetailPageViewModel : BaseViewModel
    {
      
        private CustomersModel customer;
        private bool isError;
        private readonly ICustomersManager customersManager;
        public ListCustomersDetailPageViewModel(INavigationService navigationService, ICustomersManager customersManager) : base(navigationService)
        {
            this.customersManager = customersManager;
            this.SaveCommand = new DelegateCommand(async () => await this.ExecuteSaveCommand());
            this.IsError = false;
            this.DeleteCommand = new DelegateCommand(async () => await this.ExecuteDeleteCommand());
            this.Customer = new CustomersModel();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("SelectedCustomer"))
            {
                this.Customer = parameters["SelectedCustomer"] as CustomersModel;

            }
            this.Title = "Editar/Eliminar Cliente";

        }

        public ICommand DeleteCommand { get; set; }
        private async Task ExecuteDeleteCommand()
        {

            var provider = await customersManager.FindByIdAsync(this.Customer.Id);
            await customersManager.DeleteAsync(provider);
            await this.NavigationService.GoBackAsync();
        }

        private async Task ExecuteSaveCommand()
        {
            this.IsError = true;
            try
            {
                var clientnew = await customersManager.FindByIdAsync(this.Customer.Id);

                clientnew.Code = this.Customer.Code;
                clientnew.Location = this.Customer.Location;
                clientnew.Address = this.Customer.Address;
                clientnew.Name = this.Customer.Name;
                clientnew.ShortName = this.Customer.ShortName;
                clientnew.DateCreate = DateTime.Now;
                clientnew.City = this.Customer.City;
                clientnew.PhoneNumber = this.Customer.PhoneNumber;


                await customersManager.EditAsync(clientnew);
            }
            catch (Exception e)
            {

                CrossToastPopUp.Current.ShowToastSuccess(e.Message, ToastLength.Long);
            }
            

            CrossToastPopUp.Current.ShowToastSuccess($"Cliente Editado", ToastLength.Long);
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

        public CustomersModel Customer
        {
            get => this.customer;
            set
            {
                this.SetProperty(ref this.customer, value);
            }
        }
    }
}
