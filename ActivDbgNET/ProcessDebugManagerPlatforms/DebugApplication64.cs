using ActivDbg;

namespace ActivDbgNET.PlatformDependentClasses
{
    internal class DebugApplication64 : DebugApplication
    {
        private IDebugApplication64 da64;

        public DebugApplication64(IDebugApplication64 da64)
        {
            this.da64 = da64;
        }

        public override string GetName()
        {
            string name = "";
            da64.GetName(out name);
            return name;
        }
    }
}