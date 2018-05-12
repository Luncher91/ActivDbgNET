using System;
using System.Linq;
using System.Text;
using ActivDbg;

namespace ActivDbgNET
{
    public class DebugDocumentText : DebugDocument
    {
        private IDebugDocumentText doc;

        internal DebugDocumentText(IDebugDocumentText doc) : base(doc)
        {
            this.doc = doc as IDebugDocumentText;
        }

        public DocumentText GetText()
        {
            var size = GetSize();
            int bufferSize = (int)size.Characters;
            var tBuffer = new ushort[bufferSize];
            var aBuffer = new ushort[bufferSize];
            uint numChars = 0;

            doc.GetText(0, ref tBuffer[0], ref aBuffer[0], ref numChars, size.Characters);

            DocumentText result = new DocumentText();
            result.Text = StringFromBuffer(tBuffer);
            result.Flags = aBuffer.Select(v => (SourceTextType)v).ToArray();

            return result;
        }

        private static string StringFromBuffer(ushort[] buffer)
        {
            StringBuilder txt = new StringBuilder();
            foreach (ushort c in buffer)
            {
                if (c == 0)
                    break;

                txt.Append(Convert.ToChar(c));
            }

            return txt.ToString();
        }

        public DocumentSize GetSize()
        {
            DocumentSize size = new DocumentSize();
            doc.GetSize(out size.Lines, out size.Characters);
            return size;
        }

        public SourcePosition GetPosition(DebugDocumentContext docCont)
        {
            SourcePosition p = new SourcePosition();
            uint characterPosition = 0, numChars = 0;
            doc.GetPositionOfContext(docCont.GetDebugDocumentContext(), out characterPosition, out numChars);
            doc.GetLineOfPosition(characterPosition, out p.Line, out p.Character);
            return p;
        }

        public static DebugDocumentText FromDebugDocument(DebugDocument d)
        {
            if (d == null)
                return null;

            IDebugDocumentText tdoc = d.GetDoc() as IDebugDocumentText;

            if (tdoc == null)
                return null;

            return new DebugDocumentText(tdoc);
        }

        public struct DocumentSize
        {
            public uint Lines;
            public uint Characters;
        }

        public struct DocumentText
        {
            public string Text;
            public SourceTextType[] Flags;
        }

        public enum SourceTextType
        {
            Keyword = 0x0001,
            Comment = 0x0002,
            NonSource = 0x0004,
            Operator = 0x0008,
            Number = 0x0010,
            String = 0x0020,
            FunctionStart = 0x0040,
            Identifier = 0x0100
        }
    }
}
