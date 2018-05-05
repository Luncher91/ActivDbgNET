using ActivDbg;

namespace ActivDbgNET
{
    public class DebugCodeContext
    {
        private IDebugCodeContext codeContext;

        internal DebugCodeContext(IDebugCodeContext codeContext)
        {
            this.codeContext = codeContext;
        }

        public DebugDocumentContext GetDebugDocumentContext()
        {
            IDebugDocumentContext docCont;
            codeContext.GetDocumentContext(out docCont);
            return new DebugDocumentContext(docCont);
        }

        public void SetBreakpoint(BreakPointState state)
        {
            codeContext.SetBreakPoint(state.GetTagBreakPointState());
        }

        public enum BreakPointState
        {
            Deleted,
            Disabled,
            Enabled
        }
    }

    internal static class BreakPointStateConversion
    {
        internal static tagBREAKPOINT_STATE GetTagBreakPointState(this DebugCodeContext.BreakPointState newState)
        {
            switch (newState)
            {
                case DebugCodeContext.BreakPointState.Deleted:
                    return tagBREAKPOINT_STATE.BREAKPOINT_DELETED;
                case DebugCodeContext.BreakPointState.Disabled:
                    return tagBREAKPOINT_STATE.BREAKPOINT_DISABLED;
                case DebugCodeContext.BreakPointState.Enabled:
                    return tagBREAKPOINT_STATE.BREAKPOINT_ENABLED;
                default:
                    return tagBREAKPOINT_STATE.BREAKPOINT_DELETED;
            }
        }

        internal static DebugCodeContext.BreakPointState GetBreakPointState(this tagBREAKPOINT_STATE oldState)
        {
            switch (oldState)
            {
                case tagBREAKPOINT_STATE.BREAKPOINT_DELETED:
                    return DebugCodeContext.BreakPointState.Deleted;
                case tagBREAKPOINT_STATE.BREAKPOINT_DISABLED:
                    return DebugCodeContext.BreakPointState.Disabled;
                case tagBREAKPOINT_STATE.BREAKPOINT_ENABLED:
                    return DebugCodeContext.BreakPointState.Enabled;
                default:
                    return DebugCodeContext.BreakPointState.Deleted;
            }
        }
    }
}