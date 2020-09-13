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

            await this.NavigationService.NavigateAsync("MasterDetailPage/NavigationPage/SummaryPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.MasterDetailPage, MasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<SalesPage, SalesViewModel>();
            containerRegistry.RegisterForNavigation<SummaryPage, SummaryPageViewModel>();
            
            containerRegistry.RegisterSingleton<IRepository<Database.Models.Invoices>, InvoicesRepository>();
            containerRegistry.RegisterSingleton<IInvoicesManager, InvoicesManager>();

            if (containerRegistry.IsRegistered<IInvoicesManager>())
            {
                // Do something...
            }

        }
    }
}
