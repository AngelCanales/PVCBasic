using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        private IPageDialogService dialogService;
        public SalesViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            this.dialogService = dialogService;
        }
    }
}
