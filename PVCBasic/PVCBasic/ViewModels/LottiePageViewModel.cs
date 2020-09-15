using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class LottiePageViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        private string nameFile;
        private string route;
        private LottieItem lottieItem;

        public LottiePageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            this.dialogService = dialogService;
            //this.NameFile = "dataCashBack.json";
            this.FinishedCommand = new DelegateCommand(async () => await this.ExecuteFinishedCommand());
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("LottieItem"))
            {
                this.LottieItem = parameters["LottieItem"] as LottieItem;
                this.NameFile = this.LottieItem.NameFile;
                this.Route = this.LottieItem.Route;
            }
        }

        public ICommand FinishedCommand { get; set; }

        private async Task ExecuteFinishedCommand()
        {
            if (this.Route != null)
            {
                await this.NavigationService.NavigateAsync(this.Route);
            }
        }

        public LottieItem LottieItem
        {
            get => this.lottieItem;
            set
            {
                this.lottieItem = value;
                this.RaisePropertyChanged();
            }
        }

        public string NameFile
        {
            get => this.nameFile;
            set
            {
                this.nameFile = value;
                this.RaisePropertyChanged();
            }
        }
        public string Route
        {
            get => this.route;
            set
            {
                this.route = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
