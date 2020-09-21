using PVCBasic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.DependencyService
{
  public  interface IConvertToPDF
    {

        void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml);
    }
}
