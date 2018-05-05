using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSDebugger
{
    public class DebugApplication
    {
        private IRemoteDebugApplicationThread rdat;

        internal DebugApplication(IRemoteDebugApplicationThread remoteDebugThread)
        {
            rdat = remoteDebugThread;
        }

        public List<DebugTextDocument> GetDocuments()
        {
            List<tagDebugStackFrameDescriptor> frames;
            frames = GetFrameDescriptors();

            var stackFrames = GetAllStackFrames(frames);
            foreach (var frame in stackFrames)
            {
                string description = "";
                frame.GetDescriptionString(1, out description);
                Console.WriteLine("Description: " + description);
            }

            List<IDebugCodeContext> codeContexts = GetAllCodeContexts(frames);
            List<IDebugDocumentContext> documentContexts = GetAllDocumentContexts(codeContexts);
            List<IDebugDocument> documents = GetAllDocuments(documentContexts);
            List<IDebugDocumentText> textDocs = documents.Select(d => d as IDebugDocumentText).Where(d => d != null).ToList();

            return textDocs.Select(d => new DebugTextDocument(d)).ToList();
        }

        private List<IDebugStackFrame> GetAllStackFrames(List<tagDebugStackFrameDescriptor> frames)
        {
            return frames.Select(frame => GetStackFrame(frame)).Where(sf => sf != null).ToList();
        }

        private IDebugStackFrame GetStackFrame(tagDebugStackFrameDescriptor frame)
        {
            return frame.pdsf;
        }

        private List<IDebugDocument> GetAllDocuments(List<IDebugDocumentContext> documentContexts)
        {
            return documentContexts.Select(dc => GetDocument(dc)).ToList();
        }

        private IDebugDocument GetDocument(IDebugDocumentContext dc)
        {
            IDebugDocument doc = null;
            dc.GetDocument(out doc);
            return doc;
        }

        private List<IDebugDocumentContext> GetAllDocumentContexts(List<IDebugCodeContext> codeContexts)
        {
            return codeContexts.Select(cc => GetDocumentContexts(cc)).ToList();
        }

        private IDebugDocumentContext GetDocumentContexts(IDebugCodeContext cc)
        {
            IDebugDocumentContext docCon = null;
            cc.GetDocumentContext(out docCon);
            return docCon;
        }

        private List<IDebugCodeContext> GetAllCodeContexts(List<tagDebugStackFrameDescriptor> frames)
        {
            return frames.Select(f => GetCodeContexts(f)).Where(cc => cc != null).ToList();
        }

        private IDebugCodeContext GetCodeContexts(tagDebugStackFrameDescriptor f)
        {
            if (f.pdsf == null)
                return null;

            IDebugCodeContext codeContext = null;
            f.pdsf.GetCodeContext(out codeContext);
            return codeContext;
        }

        private List<tagDebugStackFrameDescriptor> GetFrameDescriptors()
        {
            List<tagDebugStackFrameDescriptor> frames = new List<tagDebugStackFrameDescriptor>();

            // get enum
            IEnumDebugStackFrames stackFrames = null;
            rdat.EnumStackFrames(out stackFrames);

            // temporary variables
            tagDebugStackFrameDescriptor frame;
            uint fetched = 0;

            do
            {
                fetched = 0;
                stackFrames.RemoteNext(1, out frame, out fetched);
                frames.Add(frame);
            } while (fetched > 0);

            return frames;
        }

        internal IRemoteDebugApplicationThread RemoteDebugApplicationThread
        {
            get
            {
                return rdat;
            }
        }
    }
}
