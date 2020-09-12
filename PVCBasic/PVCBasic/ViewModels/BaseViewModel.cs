using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible
    {
        private string title;

        public BaseViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        protected INavigationService NavigationService { get; private set; }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }
    }
}
