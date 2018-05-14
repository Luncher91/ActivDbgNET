using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET
{
    public class DebugHelper
    {
        private IDebugHelper helper;

        internal DebugHelper(IDebugHelper helper)
        {
            this.helper = helper;            
        }
    }
}
