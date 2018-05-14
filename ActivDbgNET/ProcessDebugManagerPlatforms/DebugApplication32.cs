using ActivDbg;

namespace ActivDbgNET.PlatformDependentClasses
{
    internal class DebugApplication32 : DebugApplication
    {
        private IDebugApplication32 da32;

        public DebugApplication32(IDebugApplication32 da32)
        {
            this.da32 = da32;
        }

        public override string GetName()
        {
            string name = "";
            da32.GetName(out name);
            return name;
        }
    }
}