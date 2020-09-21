﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PVCBasic.Droid;
using Xamarin.Forms;
using System.Threading.Tasks;
using PVCBasic.DependencyService;
using PVCBasic.Droid.DependencyService;
using PVCBasic.Helper.Enum;
using PVCBasic.Models;

[assembly: Dependency(typeof(ConvertToPDF))]
namespace PVCBasic.Droid.DependencyService
{
    
    public class ConvertToPDF : IConvertToPDF
    {
        
       
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
                webpage.Layout(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight);
                webpage.LoadData(_PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8");
                webpage.SetWebViewClient(new WebViewCallBack(_PDFToHtml));
            }
            catch
            {
                _PDFToHtml.Status = PDFEnum.Failed;
            }
        }
    }
}