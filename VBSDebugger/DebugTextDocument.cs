using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VBSDebugger
{
    public class DebugTextDocument
    {
        private IDebugDocumentText document;

        public DebugTextDocument(IDebugDocumentText doc)
        {
            document = doc;
        }

        public DocumentSize GetSize()
        {
            DocumentSize size = new DocumentSize();
            document.GetSize(out size.Lines, out size.Characters);
            return size;
        }

        public enum SOURCE_TEXT_ATTR
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

        public struct DocumentText
        {
            public string Text;
            public SOURCE_TEXT_ATTR[] Attributes;
        }

        public DocumentText GetText()
        {
            var size = GetSize();
            int bufferSize = (int)size.Characters;
            var tBuffer = new ushort[bufferSize];
            var aBuffer = new ushort[bufferSize];
            uint numChars = 0;

            document.GetText(0, ref tBuffer[0], ref aBuffer[0], ref numChars, size.Characters);

            DocumentText result = new DocumentText();
            result.Text = StringFromBuffer(tBuffer);
            result.Attributes = aBuffer.Select(v => (SOURCE_TEXT_ATTR)v).ToArray();

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

        public string GetName()
        {
            string title = "";
            document.GetName(tagDOCUMENTNAMETYPE.DOCUMENTNAMETYPE_TITLE, out title);
            return title;
        }

        private string GetStringFromRefUShort(ushort ptr)
        {
            return Marshal.PtrToStringUni((IntPtr)ptr);
        }

        public struct DocumentSize
        {
            public uint Lines;
            public uint Characters;
        }

        public override int GetHashCode()
        {
            return document.GetHashCode();
        }

        public override bool Equals(object a)
        {
            DebugTextDocument aDoc = a as DebugTextDocument;

            if (aDoc == null)
                return false;

            return document.Equals(aDoc.document);
        }

        public static bool operator ==(DebugTextDocument a, DebugTextDocument b)
        {
            object aObj = a;
            object bObj = b;

            if (aObj == null)
                if (bObj == null)
                    return true;
                else
                    return false;


            return a.Equals(b);
        }

        public static bool operator !=(DebugTextDocument a, DebugTextDocument b)
        {
            object aObj = a;
            object bObj = b;

            if (aObj == null)
                if (bObj == null)
                    return true;
                else
                    return false;

            return !a.Equals(b);
        }
    }
}
