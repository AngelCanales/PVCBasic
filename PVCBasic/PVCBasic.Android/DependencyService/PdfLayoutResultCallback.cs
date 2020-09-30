using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Lang;
using PVCBasic.Models;

namespace PVCBasic.Droid.DependencyService
{
    [Register("android/print/PdfLayoutResultCallback")]
    public class PdfLayoutResultCallback : PrintDocumentAdapter.LayoutResultCallback
    {
        public PrintDocumentAdapter Adapter { get; set; }

        public PDFToHtml PDFToHtml { get; set; }

        public PdfLayoutResultCallback(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public PdfLayoutResultCallback() : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (!(Handle != IntPtr.Zero))
            {
                unsafe
                {
                    JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                    SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                    JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                }
            }

        }

        public override void OnLayoutFinished(PrintDocumentInfo info, bool changed)
        {
            try
            {
                var file = new Java.IO.File(PDFToHtml.FilePath);
                var fileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadWrite);
                var writeResultCallback = new PdfWriteResultCallback(PDFToHtml);
                Adapter.OnWrite(new PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(), writeResultCallback);
            }
            catch
            {
                PDFToHtml.StatusFailed = true;
            }

            base.OnLayoutFinished(info, changed);
        }

        public override void OnLayoutCancelled()
        {
            base.OnLayoutCancelled();
            PDFToHtml.StatusFailed = true;
        }

        public override void OnLayoutFailed(ICharSequence error)
        {
            base.OnLayoutFailed(error);
            PDFToHtml.StatusFailed = true;
        }
    }

    [Register("android/print/PdfWriteResult")]
    public class PdfWriteResultCallback : PrintDocumentAdapter.WriteResultCallback
    {
        readonly PDFToHtml pDFToHtml;

        public PdfWriteResultCallback(PDFToHtml _pDFToHtml, IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            pDFToHtml = _pDFToHtml;
        }

        public PdfWriteResultCallback(PDFToHtml _pDFToHtml) : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (!(Handle != IntPtr.Zero))
            {
                unsafe
                {
                    JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                    SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                    JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                }
            }

            pDFToHtml = _pDFToHtml;
        }


        public override void OnWriteFinished(PageRange[] pages)
        {
            base.OnWriteFinished(pages);
            pDFToHtml.StatusFailed = false;
        }

        public override void OnWriteCancelled()
        {
            base.OnWriteCancelled();
            pDFToHtml.StatusFailed = true;
        }

        public override void OnWriteFailed(ICharSequence error)
        {
            base.OnWriteFailed(error);
            pDFToHtml.StatusFailed = true;
        }
    }
}