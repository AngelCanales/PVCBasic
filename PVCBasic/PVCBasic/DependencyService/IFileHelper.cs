using System;
using System.Collections.Generic;
using System.Text;

namespace PVCBasic.DependencyService
{
    public interface IFileHelper
    {
        string DocumentFilePath { get; }

        string ResourcesBaseUrl { get; }
    }
}
