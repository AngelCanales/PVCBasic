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
using PVCBasic.Models;

[assembly: Dependency(typeof(FileHelper))]
namespace PVCBasic.iOS
{
    public class FileHelper : IFileHelper
    {
        public string DocumentFilePath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public async Task<string> StrartConverting(string html, string namefile)
        {
            WKWebViewConfiguration configuration = new WKWebViewConfiguration();
            WKWebView webView = new WKWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72), configuration);
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = Path.Combine(documents, namefile);
            webView.NavigationDelegate = new WebViewCallBack(file);
            webView.UserInteractionEnabled = false;
            webView.BackgroundColor = UIColor.White;
            webView.LoadHtmlString(html, null);
            return file;
        }

        public void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml)
        {
            try
            {
                WKWebView webView = new WKWebView(new CGRect(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight), new WKWebViewConfiguration());
                webView.UserInteractionEnabled = false;
                webView.BackgroundColor = UIColor.White;
                webView.NavigationDelegate = new WebViewCallBackTwo(_PDFToHtml);
                webView.LoadHtmlString(_PDFToHtml.HTMLString, null);
            }
            catch
            {
                _PDFToHtml.StatusFailed = true;
            }
        }
    }
}