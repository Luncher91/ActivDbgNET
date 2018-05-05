using ActivDbg;

namespace ActivDbgNET
{
    public class DebugProperty
    {
        private IDebugProperty property;

        internal DebugProperty(IDebugProperty property)
        {
            this.property = property;
        }
    }
}