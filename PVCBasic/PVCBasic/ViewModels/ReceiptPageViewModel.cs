
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.DependencyService;
using PVCBasic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using PVCBasic.PrintInvoice;

namespace PVCBasic.ViewModels
{
    public class ReceiptPageViewModel : BaseViewModel
    {
        
        private string receipt;
        private string numberInvoice;
        private InvoicesViewModel invoices;
        private readonly PrintInvoice.PrintInvoice printInvoice;
        public ReceiptPageViewModel(INavigationService navigationService, PrintInvoice.PrintInvoice printInvoice) : base(navigationService)
        {
            this.PrintItemCommand = new DelegateCommand(async () => await this.ExecutePrintItemCommand());
            this.ShareItemCommand = new DelegateCommand(async () => await this.ExecuteShareItemCommand());
            this.CloseItemsCommand = new DelegateCommand(async () => await this.ExecuteCloseItemsCommand());
            this.printInvoice = printInvoice;
            this.Title = "Recibo";

            
        }

        private async Task ExecuteCloseItemsCommand()
        {
            await this.NavigationService.GoBackAsync();
        }

        private async Task ExecuteShareItemCommand()
        {

            try
            {
              await Task.Delay(500);

                string path = string.Empty;

                var t = $"Receipt-{Guid.NewGuid().ToString()}";
                path = Xamarin.Forms.DependencyService.Get<IFileHelper>().StrartConverting(this.Receipt, t);
                // CrossToastPopUp.Current.ShowToastSuccess($"{path}", ToastLength.Long);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Title,
                    File = new ShareFile(path)
                });

            }
            catch (Exception e)
            {
                CrossToastPopUp.Current.ShowToastError($"{e.Message}", ToastLength.Long);
            }
        }


        private async Task ExecutePrintItemCommand()
        {
            try
            {
                printInvoice.Title = this.Title;
               await printInvoice.PrintInvoices(this.NumberInvoice);
            }
            catch (Exception e)
            {
                CrossToastPopUp.Current.ShowToastError($"Error: {e.Message}", ToastLength.Long);
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
         //   CrossToastPopUp.Current.ShowToastSuccess($"{this.NumberInvoice} ", ToastLength.Long);
            if (parameters.ContainsKey("Receipt"))
            {
                this.Receipt = parameters["Receipt"] as string;
            }

            if (parameters.ContainsKey("Title"))
            {
                this.Title = parameters["Title"] as string;
            }

            if (parameters.ContainsKey("Invoice"))
            {
                if (this.Invoices != null)
                {
                    this.Invoices = parameters["Invoice"] as InvoicesViewModel;
                }
            }


            if (parameters.ContainsKey("NumberInvoice"))
            {
                this.NumberInvoice = parameters["NumberInvoice"] as string;
            }

            if (string.IsNullOrEmpty(this.Receipt))
            {
            //   CrossToastPopUp.Current.ShowToastSuccess($"n{this.NumberInvoice} ", ToastLength.Long);
                this.Receipt = await printInvoice.GetHtmlInvoices(this.NumberInvoice);
            }
        }

        public InvoicesViewModel Invoices
        {
            get => this.invoices;
            set => this.SetProperty(ref this.invoices, value);
        }

        public string Receipt
        {
            get => this.receipt;
            set
            {
                this.receipt = value;
                this.RaisePropertyChanged();
            }
        }

        public string NumberInvoice
        {
            get => this.numberInvoice;
            set
            {
                this.numberInvoice = value;
                this.RaisePropertyChanged();
            }
        }


        public ICommand PrintItemCommand { get; set; }

        public ICommand ShareItemCommand { get; set; }

        public ICommand CloseItemsCommand { get; set; }

    }
}
