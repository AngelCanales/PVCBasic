using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.DependencyService
{
  public interface IBthPrint
    {
        string NamePrint { get; set; }

        Task WriteAsync(string content);

        Task ConnectAsync();

        Task DisconnectAsync();

        ObservableCollection<string> PairedDevices();
    }
}
