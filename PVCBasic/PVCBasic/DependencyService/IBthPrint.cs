using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.DependencyService
{
  public interface IBthPrint
    {
        string NamePrint { get; set; }

        Task WriteAsync(string content);

        Task PrintImage(byte[] ValueImage);

        Task ConnectAsync();

        Task DisconnectAsync();

        ObservableCollection<string> PairedDevices();
    }
}
