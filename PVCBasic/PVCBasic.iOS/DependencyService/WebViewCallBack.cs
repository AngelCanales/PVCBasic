using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using PVCBasic.Models;
using UIKit;
using WebKit;

namespace PVCBasic.iOS
{
    class WebViewCallBack : WKNavigationDelegate
    {
        string filename = null;
        public WebViewCallBack(string path)
        {
            this.filename = path;
        }
        override public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            double height, width;
            int header, sidespace;
            width = 595.2;
            height = 841.8;
            header = 10;
            sidespace = 10;
            UIEdgeInsets pageMargins = new UIEdgeInsets(header, sidespace, header, sidespace);
            webView.ViewPrintFormatter.ContentInsets = pageMargins;
            UIPrintPageRenderer renderer = new UIPrintPageRenderer();
            renderer.AddPrintFormatter(webView.ViewPrintFormatter, 0);
            CGSize pageSize = new CGSize(width, height);
            CGRect printableRect = new CGRect(sidespace,
                              header,
                              pageSize.Width - (sidespace * 2),
                              pageSize.Height - (header * 2));
            CGRect paperRect = new CGRect(0, 0, width, height);
            renderer.SetValueForKey(NSValue.FromObject(paperRect), (NSString)"paperRect");
            renderer.SetValueForKey(NSValue.FromObject(printableRect), (NSString)"printableRect");
            NSData file = PrintToPDFWithRenderer(renderer, paperRect);
            File.WriteAllBytes(filename, file.ToArray());
        }

        private NSData PrintToPDFWithRenderer(UIPrintPageRenderer renderer, CGRect paperRect)
        {
            NSMutableData pdfData = new NSMutableData();
            UIGraphics.BeginPDFContext(pdfData, paperRect, null);
            renderer.PrepareForDrawingPages(new NSRange(0, renderer.NumberOfPages));
            CGRect bounds = UIGraphics.PDFContextBounds;
            for (int i = 0; i < renderer.NumberOfPages; i++)
            {
                UIGraphics.BeginPDFPage();
                renderer.DrawPage(i, paperRect);
            }
            UIGraphics.EndPDFContent();
            return pdfData;
        }
    }
}