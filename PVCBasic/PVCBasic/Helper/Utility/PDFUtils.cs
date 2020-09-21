using PVCBasic.DependencyService;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PVCBasic.Helper.Utility
{
    public static class PDFUtils
    {
        private static string GetBaseUrl()
        {
            var fileHelper = Xamarin.Forms.DependencyService.Get<IFileHelper>();
            return fileHelper.ResourcesBaseUrl + "pdfjs/";
        }

        public static string PdfJsViewerUri => GetBaseUrl() + "web/viewer.html";
    }
}
