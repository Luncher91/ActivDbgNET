using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSDebugger
{
    public class ScriptError
    {
        private IActiveScriptErrorDebug ActiveScriptErrorDebug;

        internal ScriptError(IActiveScriptErrorDebug error)
        {
            ActiveScriptErrorDebug = error;
        }

        public DebugTextDocument GetTextDocument()
        {
            IDebugDocumentContext docCon = null;
            ActiveScriptErrorDebug.GetDocumentContext(out docCon);
            IDebugDocument doc = null;
            docCon.GetDocument(out doc);

            IDebugDocumentText txtDoc = doc as IDebugDocumentText;
            if (txtDoc == null)
                return null;

            return new DebugTextDocument(txtDoc);
        }

        public DocumentPosition GetDebugPosition()
        {
            DocumentPosition pos = new DocumentPosition();
            ActiveScriptErrorDebug.GetSourcePosition(out pos.ContextCookie, out pos.Line, out pos.Character);
            return pos;
        }

        public struct DocumentPosition
        {
            public uint Line;
            public int Character;
            public uint ContextCookie;
        }
    }
}
