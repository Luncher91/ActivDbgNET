using ActivDbg;
using System;

namespace ActivDbgNET
{
    // ToDo: Ich habe vergessen, dass IDebugDocument kann nochmal gecastet werden; IDebugDocumentText -> IDebugDocumentTextAuthor; IDebugDocumentProvider;

    // Note: Wenn man IDebugDocumentHost implementiert, kann ein solcher Host in IDebugDocumentHelper32 oder IDebugDocumentHelper64 gesetzt werden

    // Start to implement the debugger in VSCode to get anything; From there we can evolve!
    // MVP Version 0.0.1:
    // - attach to script process
    // - stop on breakpoint and error
    // - show the current script document
    // - show the next executed line highlighted and center to that
    // - continue with the execution (F5)
    // - disconnect the debugger at any time

    // 1.0.0
    // - step over
    // - step in
    // - step out
    // - show all the other loaded documents
    // - "Watch" functionality
    // - watch code completion
    // - mouseover exploration (if vscode supports that as well)
    // - VSCode will be added to the MDM
    // - list context/global variables
    // - ??? Map the already open documents to the documents which the ScriptEngine provides ???

    public class DebugDocument
    {
        private IDebugDocument doc;

        internal DebugDocument(IDebugDocument doc)
        {
            this.doc = doc;
        }

        private string GetName(tagDOCUMENTNAMETYPE type)
        {
            string name;
            doc.GetName(type, out name);
            return name;
        }

        public string GetAppNodeName()
        {
            return GetName(tagDOCUMENTNAMETYPE.DOCUMENTNAMETYPE_APPNODE);
        }

        public string GetTitle()
        {
            return GetName(tagDOCUMENTNAMETYPE.DOCUMENTNAMETYPE_TITLE);
        }

        public string GetFilename()
        {
            return GetName(tagDOCUMENTNAMETYPE.DOCUMENTNAMETYPE_FILE_TAIL);
        }

        public string GetURL()
        {
            return GetName(tagDOCUMENTNAMETYPE.DOCUMENTNAMETYPE_URL);
        }

        public string GetId()
        {
            return GetName(tagDOCUMENTNAMETYPE.DOCUMENTNAMETYPE_UNIQUE_TITLE);
        }

        private Guid GetClassId()
        {
            Guid classId;
            doc.GetDocumentClassId(out classId);
            return classId;
        }
    }
}