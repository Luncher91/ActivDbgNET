using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSDebugger
{
    public class VBSDebugger
    {
        private IMachineDebugManager mdm;
        private IEnumRemoteDebugApplications erda;

        public VBSDebugger()
        {
            mdm = GetMDM();
            erda = null;
        }

        public DebugProcess[] GetProcesses()
        {
            List<DebugProcess> procs = new List<DebugProcess>();
            IRemoteDebugApplication rda = null;
            uint pceltFetched = 1;

            mdm.EnumApplications(out erda);
            erda.Reset();

            while (pceltFetched > 0)
            {
                erda.RemoteNext(1, out rda, out pceltFetched);

                if (rda != null)
                {
                    DebugProcess dproc = new DebugProcess(rda);
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
