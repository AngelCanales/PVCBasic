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

            var myCurrency = new CultureInfo("es-HN");
            CultureInfo.DefaultThreadCurrentCulture = myCurrency;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(myCurrency.IetfLanguageTag);
            ResourceGlobal.Culture = new CultureInfo(myCurrency.IetfLanguageTag);

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
            containerRegistry.RegisterForNavigation<TransactionDetailsPage, TransactionDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<MonthlyReportPage, MonthlyReportViewModel>();
            containerRegistry.RegisterForNavigation<AnnualReportPage, AnnualReportPageViewModel>();

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
