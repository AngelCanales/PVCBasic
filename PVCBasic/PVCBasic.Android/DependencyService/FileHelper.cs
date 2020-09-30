using Android.Graphics.Pdf;
using Android.Views;
using Java.IO;
using PVCBasic.DependencyService;
using PVCBasic.Droid.DependencyService;
using PVCBasic.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(FileHelper))]
namespace PVCBasic.Droid.DependencyService
{
    public class FileHelper : IFileHelper
    {
        public async Task<string> StrartConverting(string html, string namefile)
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
            Task.Delay(100);
            webpage.SetWebViewClient(new WebViewCallBack(file.ToString()));
            webpage.LoadData( html, "text/html", "UTF-8");

            webpage?.Dispose();
            webpage = null;


            return path;
        }


        public void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml)
        {
            try
            {
                var webpage = new Android.Webkit.WebView(Android.App.Application.Context);
                webpage.Settings.JavaScriptEnabled = true;

#pragma warning disable CS0618 // Type or member is obsolete
                webpage.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete

                webpage.SetLayerType(LayerType.Software, null);


                int width = 2102;
                int height = 2973;


                webpage.Layout(0, 0, width, height);
                webpage.LoadData(_PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8");
                webpage.SetWebViewClient(new WebViewCallBackTwo(_PDFToHtml));
            }
            catch
            {
                _PDFToHtml.StatusFailed = true ;
            }
        }
        public string DocumentFilePath => GetLocalFilePath();

        private string GetLocalFilePath()
        {
            //For dummy file path creation.
            //return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
        }
        public string ResourcesBaseUrl => "file:///android_asset/";
    }
}