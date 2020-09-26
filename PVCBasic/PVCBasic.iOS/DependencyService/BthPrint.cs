using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using PVCBasic.DependencyService;
using PVCBasic.iOS;
using WebKit;
using CoreGraphics;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;
using ExternalAccessory;

[assembly: Dependency(typeof(BthPrint))]
namespace PVCBasic.iOS
{
    public class BthPrint : IBthPrint
    {
        //private BluetoothDevice device = null;
        //private BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
        //private BluetoothSocket BthSocket = null;
        private NSOutputStream outStream = null;
        private CancellationTokenSource _ct { get; set; }

        public string NamePrint { get; set; }


        const int RequestResolveError = 1000;
        const int sleepTime = 200;

       
        public async Task ConnectAsync()
        {
            EAAccessoryManager manager = EAAccessoryManager.SharedAccessoryManager;
            var allaccessorries = manager.ConnectedAccessories;
            foreach (var accessory in allaccessorries)
            {
                //yourlable.Text = "find accessory";
                Console.WriteLine(accessory.ToString());
                Console.WriteLine(accessory.Name);
                var protocol = "com.Yourprotocol.name";

                if (accessory.ProtocolStrings.Where(s => s == protocol).Any())
                {
                    //yourlable.Text = "Accessory  found";
                    //start session
                    var session = new EASession(accessory, protocol);
                    outStream = session.OutputStream;
                    // outputStream.Delegate = new MyOutputStreamDelegate(yourlable);
                    outStream.Schedule(NSRunLoop.Current, "kCFRunLoopDefaultMode");
                    outStream.Open();
                }
            }
        }

        public async Task DisconnectAsync()
        {
            outStream.Dispose();
            outStream.Close();
        }

        public ObservableCollection<string> PairedDevices()
        {

            EAAccessoryManager manager = EAAccessoryManager.SharedAccessoryManager;
            var allaccessorries = manager.ConnectedAccessories;
            foreach (var accessory in allaccessorries)
            {
                //yourlable.Text = "find accessory";
                Console.WriteLine(accessory.ToString());
                Console.WriteLine(accessory.Name);
                var protocol = "com.Yourprotocol.name";

                if (accessory.ProtocolStrings.Where(s => s == protocol).Any())
                {
                    //yourlable.Text = "Accessory  found";
                    //start session
                    var session = new EASession(accessory, protocol);
                    outStream = session.OutputStream;
                   // outputStream.Delegate = new MyOutputStreamDelegate(yourlable);
                    outStream.Schedule(NSRunLoop.Current, "kCFRunLoopDefaultMode");
                    outStream.Open();
                }
            }

            return new ObservableCollection<string>() { "AAA", "BBB" };
        }

        public async Task WriteAsync(string content)
        {

            try
            {
              
                System.Diagnostics.Debug.WriteLine("Connected!");
                byte[] bytes = Encoding.ASCII.GetBytes(content);
                outStream.Write(bytes);
                 System.Diagnostics.Debug.WriteLine("Exit the inner loop");
            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }

        public Task PrintImage(byte[] ValueImage)
        {
            throw new NotImplementedException();
        }
    }
}