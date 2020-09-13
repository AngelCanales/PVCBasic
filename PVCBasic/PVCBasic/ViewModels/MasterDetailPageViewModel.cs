﻿using Prism.Commands;
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
                    Name = "Ventas",
                    Route = "MasterDetailPage/NavigationPage/SalesPage",
                },
                new MenuItem
                {
                    Name = "Compras",
                    Route = "MasterDetailPage/NavigationPage/Compras",
                },
                new MenuItem
                {
                    Name = "Lista de Ventas",
                    Route = "MasterDetailPage/NavigationPage/",
                },
                new MenuItem
                {
                    Name = "Lista de Compras",
                    Route = "MasterDetailPage/NavigationPage/",
                },
                new MenuItem
                {
                    Name = "Reporte Diario",
                    Route = "MasterDetailPage/NavigationPage/SummaryPage",
                },
                 new MenuItem
                {
                    Name = "Reporte Mensual",
                    Route = "MasterDetailPage/NavigationPage/",
                },
                  new MenuItem
                {
                    Name = "Reporte Anual",
                    Route = "MasterDetailPage/NavigationPage/",
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
                await this.NavigationService.NavigateAsync(item.Route);
            }
        }
    }
}

