using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Util;
using PVCBasic.DependencyService;
using PVCBasic.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(BthPrint))]
namespace PVCBasic.Droid.DependencyService
{
    public class BthPrint : IBthPrint
    {
		//github.com/acaliaro/TestBth

		private BluetoothDevice device = null;
		private BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
		private BluetoothSocket BthSocket = null;
		private StreamWriter outStream = null;
		private CancellationTokenSource _ct { get; set; }

        const int RequestResolveError = 1000;
		const int sleepTime = 200;

		public string NamePrint { get; set; }

        public async Task ConnectAsync()
        {
			//Thread.Sleep(1000);
			_ct = new CancellationTokenSource();
			while (_ct.IsCancellationRequested == false)
			{

				try
				{
					Thread.Sleep(sleepTime);

					adapter = BluetoothAdapter.DefaultAdapter;

					if (adapter == null)
						System.Diagnostics.Debug.WriteLine("No Bluetooth adapter found.");
					else
						System.Diagnostics.Debug.WriteLine("Adapter found!!");

					if (!adapter.IsEnabled)
						System.Diagnostics.Debug.WriteLine("Bluetooth adapter is not enabled.");
					else
						System.Diagnostics.Debug.WriteLine("Adapter enabled!");

					System.Diagnostics.Debug.WriteLine("Try to connect to " + this.NamePrint);

					foreach (var bd in adapter.BondedDevices)
					{
						System.Diagnostics.Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
						if (bd.Name.ToUpper().IndexOf(this.NamePrint.ToUpper()) >= 0)
						{

							System.Diagnostics.Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
							device = bd;
							break;
						}
					}

					if (device == null)
						System.Diagnostics.Debug.WriteLine("Named device not found.");
					else
					{
						UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
						if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
							BthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
						else
							BthSocket = device.CreateRfcommSocketToServiceRecord(uuid);

						if (BthSocket != null)
						{


							
							await BthSocket.ConnectAsync();
							await Task.Delay(500);
							
							if (BthSocket.IsConnected)
							{
								System.Diagnostics.Debug.WriteLine("Connected!");
								Thread.Sleep(200);
									try
									{
									this.outStream = new StreamWriter(this.BthSocket.OutputStream);
									}
									catch (Exception e)
									{
									System.Diagnostics.Debug.WriteLine(e.Message);
									}
								}
								
								System.Diagnostics.Debug.WriteLine("Exit the inner loop");

							
						}
						else
							System.Diagnostics.Debug.WriteLine("BthSocket = null");

					}


				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
				}
			}

			System.Diagnostics.Debug.WriteLine("Exit the external loop");
		}

        public async Task DisconnectAsync()
        {
			await outStream.DisposeAsync();
			this.BthSocket?.Close();
			device = null;
			adapter = null;
		}

        public ObservableCollection<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }

        public async Task WriteAsync(string content)
        {
			if (this.BthSocket != null)
			{
				try
				{
					if (this.BthSocket.IsConnected)
					{
						System.Diagnostics.Debug.WriteLine("Connected!");
						
						await outStream.WriteLineAsync(content);
						await outStream.FlushAsync();

						System.Diagnostics.Debug.WriteLine("Exit the inner loop");

					}
				}
				catch (Exception )
				{
				
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("BthSocket = null");
			}
		}

        public async Task PrintImage(byte[] ValueImage)
        {
			try
			{
				await this.BthSocket.OutputStream.WriteAsync(ValueImage, 0, ValueImage.Length);
				// Java.Lang.Thread.Sleep(2000);

				//END IMAGE
				Java.Lang.Thread.Sleep(2000);
			}
			catch (Exception)
			{
				throw new Exception("Unable to print. Please re-configure the printer and try again!");
			}
		}

		
	}
}