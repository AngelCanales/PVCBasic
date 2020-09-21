using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using PVCBasic.DependencyService;
using PVCBasic.iOS;

[assembly: Dependency(typeof(FileHelper))]
namespace PVCBasic.iOS
{
    public class FileHelper : IFileHelper
    {
        public string DocumentFilePath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public string ResourcesBaseUrl
        {
            get
            {
                string path = NSBundle.MainBundle.BundlePath;
                if (!path.EndsWith("/")) path += "/";
                return path;
            }
        }
    }
}