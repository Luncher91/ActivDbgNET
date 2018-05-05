using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET
{
    public class RemoteDebugApplication
    {
        private IRemoteDebugApplication remoteDebugApplication;

        internal RemoteDebugApplication(IRemoteDebugApplication rda)
        {
            remoteDebugApplication = rda;
        }

        public string GetName()
        {
            string name = "";
            remoteDebugApplication.GetName(out name);
            return name;
        }

        public void ConnectDebugger(ApplicationDebugger debugger)
        {
            remoteDebugApplication.ConnectDebugger(debugger.GetIApplicationDebugger());
        }

        public void DisconnectDebugger()
        {
            remoteDebugApplication.DisconnectDebugger();
        }
    }
}
