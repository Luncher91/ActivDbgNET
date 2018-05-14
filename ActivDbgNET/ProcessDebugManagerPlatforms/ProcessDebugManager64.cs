using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET.PlatformDependentClasses
{
    class ProcessDebugManager64 : ProcessDebugManager
    {
        private IProcessDebugManager64 pdm;

        internal ProcessDebugManager64()
        {
            Guid _pdmCLSID = new Guid("78a51822-51f4-11d0-8f20-00805f2cd064");
            Type pdmType = Type.GetTypeFromCLSID(_pdmCLSID, false);
            pdm = (IProcessDebugManager64)Activator.CreateInstance(pdmType);
        }

        public override DebugApplication GetDefaultDebugApplication()
        {
            IDebugApplication64 da64 = null;
            pdm.GetDefaultApplication(out da64);

            if (da64 == null)
                return null;

            return new DebugApplication64(da64);
        }
    }
}
