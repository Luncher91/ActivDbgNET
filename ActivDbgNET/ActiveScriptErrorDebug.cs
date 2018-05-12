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

        public DebugDocumentContext GetDocumentContext()
        {
            IDebugDocumentContext debugCtxt = null;
            pError.GetDocumentContext(out debugCtxt);

            if (debugCtxt != null)
                return new DebugDocumentContext(debugCtxt);

            return null;
        }

        public SourcePosition GetSourcePosition()
        {
            SourcePosition pos = new SourcePosition();
            int charPos = 0;

            pError.GetSourcePosition(out pos.SourceContext, out pos.Line, out charPos);

            pos.Character = (uint)charPos;

            return pos;
        }
    }
}