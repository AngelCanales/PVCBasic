﻿using System;
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
using PVCBasic.PVCBCore.Products;
using PVCBasic.PVCBCore.Parameters;
using PVCBasic.PVCBCore.Inventories;
using Microsoft.EntityFrameworkCore;

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

           
           
                try
                {
                if (!DesignMode.IsDesignModeEnabled)
                {
                    var context = new PVCBContext();
                    await PVCBasic.PVCBasicSeedData.PVCBasicSeedData.EnsurePVCBasicSeedData(context);
                }
            }
                catch (Exception e)
                {
                }
           

         

            var param = new NavigationParameters();
            param.Add("NameFile", "dataCashBack.json");
            await this.NavigationService.NavigateAsync("MasterDetailPage/NavigationPage/SummaryPage", param);
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
            containerRegistry.RegisterForNavigation<ListProductsPage,ListProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddProductPage, AddProductPageViewModel>();
            containerRegistry.RegisterForNavigation<ListProductsDetailPage, ListProductsDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchProductPage, SearchProductPageViewModel>();
            containerRegistry.RegisterForNavigation<InvoiceTabbedPage, InvoiceTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<InvoiceDetailPage, InvoiceDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ReceiptPage, ReceiptPageViewModel>();
            containerRegistry.RegisterForNavigation<InventoriesPage, InventoriesPageViewModel>();
            containerRegistry.RegisterForNavigation<ListParametersPage, ListParametersPageViewModel>();
            containerRegistry.RegisterForNavigation<ParametersDetailPage, ParametersDetailPageViewModel>();

            // Data access
            containerRegistry.RegisterSingleton<IRepository<Database.Models.Invoices>, InvoicesRepository>();
            containerRegistry.RegisterSingleton<IRepository<Database.Models.DetailInvoices>, DetailInvoicesRepository>();
            containerRegistry.RegisterSingleton<IRepository<Database.Models.Products>, ProductRepository>();
            containerRegistry.RegisterSingleton<IRepository<Database.Models.Parameters>, ParametersRepository>();
            containerRegistry.RegisterSingleton<IInvoicesManager, InvoicesManager>();
            containerRegistry.RegisterSingleton<IProductsManager, ProductsManager>();
            containerRegistry.RegisterSingleton<IParametersManager, ParametersManager>();
            containerRegistry.RegisterSingleton<IRepository<Database.Models.Inventories>, InventoriesRepository>();
            containerRegistry.RegisterSingleton<IInventoriesManager, InventoriesManager>();

            containerRegistry.RegisterSingleton<PrintInvoice.PrintInvoice>();
        }
    }
}