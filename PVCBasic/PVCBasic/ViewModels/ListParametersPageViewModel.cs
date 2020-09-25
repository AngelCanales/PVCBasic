using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Parameters;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PVCBasic.ViewModels
{
    public class ListParametersPageViewModel : BaseViewModel
    {
        private readonly IParametersManager parametersManager;
        private ObservableCollection<ParametersModel> listparameters;
   
        private bool isBusy;
        private bool isError;
        public ListParametersPageViewModel(INavigationService navigationService, IParametersManager parametersManager) : base(navigationService)
        {
            this.parametersManager = parametersManager;

            this.DetailParameterCommand = new DelegateCommand<ParametersModel>(async (c) => await this.ExecuteDetailParameterCommand(c));
            this.ListOfParameters = new ObservableCollection<ParametersModel>();

            this.IsError = false;
            this.Title = "Parámetros";
        }


        private async Task ExecuteDetailParameterCommand(ParametersModel selectedParameter)
        {
            var param = new NavigationParameters();
            param.Add("SelectedParameter", selectedParameter);
            await this.NavigationService.NavigateAsync("ParametersDetailPage", param);
        }

        public ICommand DetailParameterCommand { get; set; }

       

        private async Task LoadAsync()
        {
            this.ListOfParameters.Clear();
            await this.GetProductsListAsync();
        }

        public bool IsError
        {
            get => this.isError;
            set
            {
                this.isError = value;
                this.RaisePropertyChanged();
            }
        }

        public async Task GetProductsListAsync()
        {
            await Task.Yield();
            await Task.Delay(500);
            this.IsBusy = true;
            var parametersList = new ObservableCollection<ParametersModel>();
            try
            {
                var plist = await parametersManager.GetAllAsync();

                var data = plist.Select(s => new ParametersModel
                {
                    Id = s.Id,
                    Key = s.Key,
                    Value = s.Value,
                    ValueBool = s.ValueBool,
                    ValueDate = s.ValueDate,
                    ValueDecimal = s.ValueDecimal,
                    ValueImage = s.ValueImage,
                    ValueInt = s.ValueInt,
                });

                foreach (var item in data)
                {
                    parametersList.Add(item);
                }

                await Task.Yield();

                this.ListOfParameters =
                   new ObservableCollection<ParametersModel>(
                       parametersList
                           .OrderBy(c => c.Key));

               



            }
            catch (Exception e)
            {
                var x = e.Message;
            }


            this.IsBusy = false;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Title"))
            {
                this.Title = parameters["Title"] as string;
            }

            await LoadAsync();
        }

       

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<ParametersModel> ListOfParameters
        {
            get => this.listparameters;
            set
            {
                this.SetProperty(ref this.listparameters, value);
            }
        }

    }
}
