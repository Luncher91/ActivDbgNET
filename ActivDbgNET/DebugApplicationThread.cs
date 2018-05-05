using ActivDbg;

namespace ActivDbgNET
{
    public class DebugApplicationThread
    {
        private IDebugApplicationThread thread;

        internal DebugApplicationThread(IDebugApplicationThread thread)
        {
            this.thread = thread;
        }
    }
}