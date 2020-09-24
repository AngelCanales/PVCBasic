using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PVCBasic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class MasterDetailPageViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;

        public MasterDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
             : base(navigationService)
        {
            this.dialogService = dialogService;
            this.Title = "Pagina principal";

            this.Menu = new ObservableCollection<MenuItem>
            {
                   new MenuItem
                {
                    Name = "Inventario",
                    Route = "MasterDetailPage/NavigationPage/InventoriesPage",
                    IconValue = "\uf474",
                },
                  new MenuItem
                {
                    Name = "Productos",
                    Route = "MasterDetailPage/NavigationPage/ListProductsPage",
                    IconValue = "\uf468",
                },
                new MenuItem
                {
                    Name = "Ventas",
                    Route = "MasterDetailPage/NavigationPage/InvoiceTabbedPage",
                    IconValue = "\uf07a",
                },
                new MenuItem
                {
                    Name = "Compras",
                    Route = "MasterDetailPage/NavigationPage/InvoiceTabbedPage",
                    IconValue = "\uf3d1",
                },
                new MenuItem
                {
                    Name = "Lista de Ventas",
                    Route = "MasterDetailPage/NavigationPage/TransactionsPage",
                    IconValue = "\uf46d",
                },
                new MenuItem
                {
                    Name = "Lista de Compras",
                    Route = "MasterDetailPage/NavigationPage/TransactionsPage",
                    IconValue = "\uf46d",
                },
                new MenuItem
                {
                    Name = "Reporte Diario",
                    Route = "MasterDetailPage/NavigationPage/SummaryPage",
                    IconValue = "\uf1fe",
                },
                 new MenuItem
                {
                    Name = "Reporte Mensual",
                    Route = "MasterDetailPage/NavigationPage/MonthlyReportPage",
                    IconValue = "\uf1fe",
                },
                  new MenuItem
                {
                    Name = "Reporte Anual",
                    Route = "MasterDetailPage/NavigationPage/AnnualReportPage",
                    IconValue = "\uf1fe",
                },
                    new MenuItem
                {
                    Name = "Configuración",
                    Route = "MasterDetailPage/NavigationPage/ListParametersPage",
                    IconValue = "\uf085",
                },
            };

            this.NavigateCommand = new DelegateCommand<MenuItem>(async (item) => await this.ExecuteNavigate(item));
        }

        public ICommand NavigateCommand { get; set; }

        public ObservableCollection<MenuItem> Menu { get; set; }

        private async Task ExecuteNavigate(MenuItem item)
        {
            if (item.Route != null)
            {
                if(item.Name == "Ventas") 
                {
                    var param = new NavigationParameters();
                    
                    param.Add("TypeInvoice", ConstantName.ConstantName.Sales);
                    param.Add("Title", "Ventas");
                    await this.NavigationService.NavigateAsync(item.Route, param);
                }

                if (item.Name == "Compras")
                {
                    var param = new NavigationParameters();

                    param.Add("TypeInvoice", ConstantName.ConstantName.Purchases);
                    param.Add("Title", "Compras");
                    await this.NavigationService.NavigateAsync(item.Route, param);
                }

                if (item.Name == "Lista de Ventas")
                {
                    var param = new NavigationParameters();

                    param.Add("TypeInvoice", ConstantName.ConstantName.Sales);
                    param.Add("Title", "Lista de Ventas");
                    await this.NavigationService.NavigateAsync(item.Route, param);
                }

                if (item.Name == "Lista de Compras")
                {
                    var param = new NavigationParameters();

                    param.Add("TypeInvoice", ConstantName.ConstantName.Purchases);
                    param.Add("Title", "Lista de Compras");
                    await this.NavigationService.NavigateAsync(item.Route, param);
                }

                if (item.Name == "Reporte Diario")
                {
                    var param = new NavigationParameters();
                    param.Add("NameFile", "dataReport.json");
                    await this.NavigationService.NavigateAsync(item.Route, param);
                }
                

                await this.NavigationService.NavigateAsync(item.Route);
            }
        }
    }
}

