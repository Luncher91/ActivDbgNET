using ActivDbg;
using ActivDbgNET.PlatformDependentClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET
{
    public abstract class ProcessDebugManager
    {
        public static ProcessDebugManager Create()
        {
            // running in x86 mode
            if (IntPtr.Size == 4)
                return new ProcessDebugManager32();

            // running in x64 mode
            if(IntPtr.Size == 8)
                return new ProcessDebugManager64();

            return null;
        }

        public abstract DebugApplication GetDefaultDebugApplication();
    }
}
