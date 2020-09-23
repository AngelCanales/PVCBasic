using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.IO;
using PVCBasic.Models;
using Xamarin.Forms;

namespace PVCBasic.Droid.DependencyService
{
    class WebViewCallBack : WebViewClient
    {
       

        string fileNameWithPath = null;

        public WebViewCallBack(string path)
        {
            this.fileNameWithPath = path;
        }


        public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
        {
            PdfDocument document = new PdfDocument();
            PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(850, 3000, 1).Create());

            myWebview.Draw(page.Canvas);
            document.FinishPage(page);
            Stream filestream = new MemoryStream();
            FileOutputStream fos = new Java.IO.FileOutputStream(fileNameWithPath, false); ;
            try
            {
                document.WriteTo(filestream);
                fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
                fos.Close();
            }
            catch
            {
            }
        }
    }
}