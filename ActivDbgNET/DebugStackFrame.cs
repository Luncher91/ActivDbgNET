using ActivDbg;

namespace ActivDbgNET
{
    public class DebugStackFrame
    {
        private IDebugStackFrame pdsf;

        internal DebugStackFrame(IDebugStackFrame pdsf)
        {
            this.pdsf = pdsf;
        }

        public DebugCodeContext GetCodeContext()
        {
            IDebugCodeContext codeContext = null;
            pdsf.GetCodeContext(out codeContext);
            return new DebugCodeContext(codeContext);
        }

        public DebugApplicationThread GetThread()
        {
            IDebugApplicationThread thread = null;
            pdsf.GetThread(out thread);
            return new DebugApplicationThread(thread);
        }

        public DebugProperty GetDebugProperty()
        {
            IDebugProperty property = null;
            pdsf.GetDebugProperty(out property);
            return new DebugProperty(property);
        }

        public string GetLongDescription()
        {
            return GetDescription(StringType.Long);
        }

        public string GetShortDescription()
        {
            return GetDescription(StringType.Short);
        }

        private string GetDescription(StringType type)
        {
            string descr = "";
            pdsf.GetDescriptionString((int)type, out descr);
            return descr;
        }

        public string GetLongLanguage()
        {
            return GetLanguage(StringType.Long);
        }

        public string GetShortLanguage()
        {
            return GetLanguage(StringType.Short);
        }

        private string GetLanguage(StringType type)
        {
            string language = "";
            pdsf.GetLanguageString((int)type, out language);
            return language;
        }

        private enum StringType
        {
            Short = 0,
            Long = 1
        }
    }
}