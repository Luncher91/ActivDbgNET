using ActivDbg;
using System.Collections.Generic;

namespace ActivDbgNET
{
    public class DebugDocumentContext
    {
        private IDebugDocumentContext docCont;

        internal DebugDocumentContext(IDebugDocumentContext docCont)
        {
            this.docCont = docCont;
        }

        public DebugCodeContext[] GetCodeContexts()
        {
            List<DebugCodeContext> contexts = new List<DebugCodeContext>();

            IEnumDebugCodeContexts codeContexts;
            docCont.EnumCodeContexts(out codeContexts);
            codeContexts.Reset();

            IDebugCodeContext ctx;
            uint fetched = 0;

            do
            {
                fetched = 0;
                codeContexts.RemoteNext(1, out ctx, out fetched);

                if (ctx != null && fetched > 0)
                    contexts.Add(new DebugCodeContext(ctx));
            } while (fetched > 0);

            return contexts.ToArray();
        }

        public DebugDocument GetDocument()
        {
            IDebugDocument doc;
            docCont.GetDocument(out doc);
            return new DebugDocument(doc);
        }
    }
}