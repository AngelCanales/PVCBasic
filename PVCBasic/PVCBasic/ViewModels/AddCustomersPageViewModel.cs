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
    public class AddCustomersPageViewModel : BaseViewModel
    {
       

        private CustomersModel customer;
        private bool isError;
        private readonly ICustomersManager customersManager;
        public AddCustomersPageViewModel(INavigationService navigationService, ICustomersManager customersManager) : base(navigationService)
        {
            this.customersManager = customersManager;
            this.SaveCommand = new DelegateCommand(async () => await this.ExecuteSaveCommand());
            this.IsError = false;
            this.Customer = new CustomersModel();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            this.Title = "Agregar Cliente";

        }

        private async Task ExecuteSaveCommand()
        {
            this.IsError = true;
            var clientnew = new Customers();
            clientnew.Code = this.Customer.Code;
            clientnew.Location = this.Customer.Location;
            clientnew.Address = this.Customer.Address;
            clientnew.Name = this.Customer.Name;
            clientnew.ShortName = this.Customer.ShortName;
            clientnew.DateCreate = DateTime.Now;
            clientnew.City = this.Customer.City;
            clientnew.PhoneNumber = this.Customer.PhoneNumber;


            await customersManager.CreateAsync(clientnew);

            CrossToastPopUp.Current.ShowToastSuccess($"Cliente creado", ToastLength.Long);
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
