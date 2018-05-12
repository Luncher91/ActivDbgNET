using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET
{
    public class MachineDebugManager
    {
        private IMachineDebugManager machineDebugManager = null;

        public MachineDebugManager()
        {
            machineDebugManager = GetMDM();
        }

        public RemoteDebugApplication[] GetRemoteDebugApplications()
        {
            List<RemoteDebugApplication> procs = new List<RemoteDebugApplication>();
            IEnumRemoteDebugApplications erda;
            IRemoteDebugApplication rda = null;
            uint pceltFetched = 1;

            machineDebugManager.EnumApplications(out erda);
            erda.Reset();

            while (pceltFetched > 0)
            {
                erda.RemoteNext(1, out rda, out pceltFetched);

                if (rda != null)
                {
                    RemoteDebugApplication dproc = new RemoteDebugApplication(rda);
                    procs.Add(dproc);
                }
            }

            return procs.ToArray();
        }

        private static IMachineDebugManager GetMDM()
        {
            Type t = Type.GetTypeFromProgID("MDM.AD1");
            object obj = Activator.CreateInstance(t);
            return obj as IMachineDebugManager;
        }
    }
}
