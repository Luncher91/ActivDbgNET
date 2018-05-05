using ActivDbg;

namespace ActivDbgNET
{
    public class ActiveScriptErrorDebug
    {
        private IActiveScriptErrorDebug pError;

        public ActiveScriptErrorDebug(IActiveScriptErrorDebug pError)
        {
            this.pError = pError;
        }
    }
}