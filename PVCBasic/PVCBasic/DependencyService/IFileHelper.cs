using PVCBasic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PVCBasic.DependencyService
{
    public interface IFileHelper
    {
     
        Task<string> StrartConverting(string html, string namefile);

        string DocumentFilePath { get; }

        void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml);
    }
}
