using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET.PlatformDependentClasses
{
    internal class ProcessDebugManager32 : ProcessDebugManager
    {
        private IProcessDebugManager32 pdm;

        public ProcessDebugManager32()
        {
            Guid _pdmCLSID = new Guid("78a51822-51f4-11d0-8f20-00805f2cd064");
            Type pdmType = Type.GetTypeFromCLSID(_pdmCLSID, false);
            pdm = (IProcessDebugManager32)Activator.CreateInstance(pdmType);
        }

        public override DebugApplication GetDefaultDebugApplication()
        {
            IDebugApplication32 da32 = null;
            pdm.GetDefaultApplication(out da32);

            if (da32 == null)
                return null;

            return new DebugApplication32(da32);
        }
    }
}
