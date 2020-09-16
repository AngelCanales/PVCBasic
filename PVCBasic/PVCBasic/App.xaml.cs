using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PVCBasic.Services;
using PVCBasic.Views;
using Prism.DryIoc;
using System.Globalization;
using Prism;
using Prism.Ioc;
using PVCBasic.ViewModels;
using System.Threading;
using PVCBasic.Resource;
using PVCBasic.Database;
using PVCBasic.PVCBCore.Invoices;
using PVCBasic.Database.Repositories;
using Prism.Navigation;
using PVCBasic.Models;

namespace PVCBasic
{
    public partial class App : PrismApplication
    {

        public App()
          : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }


        protected override async void OnInitialized()
        {
            this.InitializeComponent();


            var culture = CultureInfo.InstalledUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture.IetfLanguageTag);
            ResourceGlobal.Culture = new CultureInfo(culture.IetfLanguageTag);

            var param = new NavigationParameters();
            var lottieItem = new LottieItem();
            lottieItem.NameFile = "dataCashBack.json";
            lottieItem.Route = "MasterDetailPage/NavigationPage/SummaryPage";
            param.Add("LottieItem", lottieItem);

            await this.NavigationService.NavigateAsync("LottiePage", param);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.MasterDetailPage, MasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<SalesPage, SalesViewModel>();
            containerRegistry.RegisterForNavigation<SummaryPage, SummaryPageViewModel>();
            containerRegistry.RegisterForNavigation<LottiePage, LottiePageViewModel>();
            containerRegistry.RegisterForNavigation<TransactionsPage, TransactionsPageViewModel>();

            containerRegistry.RegisterSingleton<IRepository<Database.Models.Invoices>, InvoicesRepository>();
            containerRegistry.RegisterSingleton<IRepository<Database.Models.DetailInvoices>, DetailInvoicesRepository>();
            containerRegistry.RegisterSingleton<IInvoicesManager, InvoicesManager>();

            if (containerRegistry.IsRegistered<IInvoicesManager>())
            {
                // Do something...
            }

        }
    }
}
