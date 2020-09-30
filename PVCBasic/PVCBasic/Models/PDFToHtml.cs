using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PVCBasic.Models
{
  public  class PDFToHtml : INotifyPropertyChanged, IDisposable
    {
        private bool statusFailed;
        private bool ispdfloading;

        public double PageHeight { get; set; } = 3024;

        public double PageWidth { get; set; } = 850;

        public double PageDPI { get; set; } = 300;

        public string HTMLString { get; set; }
        public bool StatusFailed {
            get { return statusFailed; }
            set
            {
                this.statusFailed = value;
                this.UpdatePDFStatus(value);
                OnPropertyChanged("StatusFailed");
            }
        }

        public bool IsPDFGenerating
        {
            get { return ispdfloading; }
            set
            {
                ispdfloading = value;
                OnPropertyChanged("IsPDFGenerating");
            }
        }

        private async void UpdatePDFStatus(bool newValue)
        {
            //if (newValue == PDFEnum.Started)
            //    IsPDFGenerating = true;
           // else
            if (newValue == true)
            {
                IsPDFGenerating = false;
                await App.Current.MainPage.DisplayAlert("ERROR!", "PDF is not generated", "Ok");
            }
            //else if (newValue == false)
            //{
            //    try
            //    {
            //        PDFStreamArray = Device.RuntimePlatform == Device.iOS ? File.ReadAllBytes(FilePath + ".pdf") : new byte[FileStream.Length];

            //        if (Device.RuntimePlatform == Device.Android)
            //            FileStream.Read(PDFStreamArray, 0, (int)FileStream.Length);

            //        await FileStream.WriteAsync(PDFStreamArray, 0, PDFStreamArray.Length);
            //        FileStream.Close();
            //        IsPDFGenerating = false;
            //      //  await App.Current.MainPage.Navigation.PushAsync(new PDFViewer() { Title = FileName, BindingContext = this });
            //    }
            //    catch
            //    {
            //        await App.Current.MainPage.DisplayAlert("ERROR!", "PDF is not generated", "Ok");
            //    }
            //}
        }
        public string FileName { get; set; }
        public FileStream FileStream { get; set; }
        public string FilePath { get; set; }

        public byte[] PDFStreamArray { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void Dispose()
        {
            if (FileStream != null)
            {
                FileStream.Dispose();
                FileStream = null;
            }

            PDFStreamArray = null;
        }
    }
}
