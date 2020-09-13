using Prism.Navigation;
using Prism.Services;
using PVCBasic.Database;
using PVCBasic.PVCBCore.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;

        private readonly IInvoicesManager invoicesManager;
        public SalesViewModel(INavigationService navigationService, IPageDialogService dialogService, IInvoicesManager invoicesManager) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.invoicesManager = invoicesManager;
        }
    }
}
