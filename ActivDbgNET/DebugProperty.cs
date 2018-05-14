using ActivDbg;

namespace ActivDbgNET
{
    public class DebugProperty
    {
        private const uint RADIX = 10;
        private IDebugProperty prop;

        internal DebugProperty(IDebugProperty property)
        {
            this.prop = property;
        }

        public DebugPropertyInfo GetInfo()
        {
            // TODO: not implemented
            return null;
        }
    }
}