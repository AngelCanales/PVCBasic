using Android.Graphics.Pdf;
using Java.IO;
using PVCBasic.DependencyService;
using PVCBasic.Droid.DependencyService;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(FileHelper))]
namespace PVCBasic.Droid.DependencyService
{
    public class FileHelper : IFileHelper
    {
        public string StrartConverting(string html, string namefile)
        {
            
            var webpage = new Android.Webkit.WebView(Android.App.Application.Context);
            var dir = new Java.IO.File(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath);
            var path = dir + "/" + namefile + ".pdf";
            var file = new Java.IO.File(path);

            if (!dir.Exists())
                dir.Mkdirs();


            //int x = 0;
            //while (file.Exists())
            //{
            //    x++;
            //    path = dir + "/" + namefile + "( " + x + " )" + ".pdf";
            //    file = new Java.IO.File(path);
            //}

            int width = 2102;
            int height = 2973;

            webpage.Layout(0, 0, width, height);
            webpage.SetWebViewClient(new WebViewCallBack(file.ToString()));
            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);

            webpage?.Dispose();
            webpage = null;


            return path;
        }

        public string ResourcesBaseUrl => "file:///android_asset/";
    }
}