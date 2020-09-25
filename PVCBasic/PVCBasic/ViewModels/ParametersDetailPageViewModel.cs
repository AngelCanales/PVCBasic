using Plugin.Media;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PVCBasic.Models;
using PVCBasic.PVCBCore.Parameters;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PVCBasic.ViewModels
{
    public class ParametersDetailPageViewModel : BaseViewModel
    {
        private readonly IParametersManager parametersManager;
        private ParametersModel parameters;

        private bool isBusy;
        private ImageSource imageSource;

        public ParametersDetailPageViewModel(INavigationService navigationService, IParametersManager parametersManager) : base(navigationService)
        {
            this.parametersManager = parametersManager;
            this.SaveCommand = new DelegateCommand(async () => await this.ExecuteSaveCommand());
            this.SelectedImageCommand = new DelegateCommand(async () => await this.ExecuteSelectedImageCommand());
            this.Title = "Parámetro";
        }

        private async Task ExecuteSelectedImageCommand()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                CrossToastPopUp.Current.ShowToastError($"Foto no soportada:( Permiso denegado para acceder a las fotos.)", ToastLength.Long);
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                MaxWidthHeight = 200,
            });

            if (file == null)
            {
                return;
            }

            this.ImageSources = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                Parameter.ValueImage = memoryStream.ToArray();
                file.Dispose();
            }


        }

        private async Task ExecuteSaveCommand()
        {
            this.IsError = true;
            var parameters = await parametersManager.FindByIdAsync(this.Parameter.Key);
            parameters.Value = this.Parameter.Value;
            parameters.ValueDate = this.Parameter.ValueDate;
            parameters.ValueImage = this.Parameter.ValueImage;
            parameters.ValueBool = this.Parameter.ValueBool;
            parameters.ValueInt = this.Parameter.ValueInt;
            parameters.ValueDecimal = this.Parameter.ValueDecimal;
            await parametersManager.EditAsync(parameters);

            CrossToastPopUp.Current.ShowToastSuccess($"Parameters Editado", ToastLength.Long);
            this.IsError = false;
            await this.NavigationService.GoBackAsync();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("SelectedParameter"))
            {
                this.Parameter = parameters["SelectedParameter"] as ParametersModel;
            }

            
        }

        public ICommand SaveCommand { get; set; }

        public ICommand SelectedImageCommand { get; set; }

        
        public bool IsError
        {
            get => this.isBusy;
            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged();
            }
        }

        public ImageSource ImageSources
        {
            get => this.imageSource;
            set
            {
                this.imageSource = value;
                this.RaisePropertyChanged();
            }
        }

        public ParametersModel Parameter
        {
            get => this.parameters;
            set
            {
                this.SetProperty(ref this.parameters, value);
            }
        }

    }
}
