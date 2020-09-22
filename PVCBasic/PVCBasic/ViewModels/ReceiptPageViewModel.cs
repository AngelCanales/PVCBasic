
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

namespace PVCBasic.ViewModels
{
    public class ReceiptPageViewModel : BaseViewModel
    {
        private ObservableCollection<DetailInvoicesViewModel> detailInvoices;
        private string typeInvoice;
        private string titleInvoice;
        private string receipt;
        

        public ReceiptPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.PrintItemCommand = new DelegateCommand(async () => await this.ExecutePrintItemCommand());
            this.ShareItemCommand = new DelegateCommand(async () => await this.ExecuteShareItemCommand());
            this.CloseItemsCommand = new DelegateCommand(async () => await this.ExecuteCloseItemsCommand());
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
                
              var path =  Xamarin.Forms.DependencyService.Get<IFileHelper>().StrartConverting(this.Receipt, "Receipt");

                CrossToastPopUp.Current.ShowToastSuccess($"{path}", ToastLength.Long);

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
            return;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Receipt"))
            {
                this.Receipt = parameters["Receipt"] as string;
            }
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add("DetailInvoices", this.DetailInvoices);
            parameters.Add("Title", this.TitleInvoice);
            parameters.Add("TypeInvoice", this.TypeInvoice);
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

        public string Receipt
        {
            get => this.receipt;
            set
            {
                this.receipt = value;
                this.RaisePropertyChanged();
            }
        }

        


        public string TitleInvoice
        {
            get => this.titleInvoice;
            set
            {
                this.titleInvoice = value;
                this.RaisePropertyChanged();
            }
        }
        public DetailInvoicesViewModel SelectedItemDetails { get; set; }

        public ObservableCollection<DetailInvoicesViewModel> DetailInvoices
        {
            get => this.detailInvoices;
            set => this.SetProperty(ref this.detailInvoices, value);
        }

        public ICommand PrintItemCommand { get; set; }

        public ICommand ShareItemCommand { get; set; }

        public ICommand CloseItemsCommand { get; set; }



        public ICommand DeleteItemCommand { get; set; }



    }
}
