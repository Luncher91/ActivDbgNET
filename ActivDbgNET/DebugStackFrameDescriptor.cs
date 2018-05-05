using ActivDbg;

namespace ActivDbgNET
{
    public class DebugStackFrameDescriptor
    {
        private tagDebugStackFrameDescriptor frame;

        public object PunkFinal
        {
            get
            {
                return frame.punkFinal;
            }
        }

        public bool IsBeingProcessed
        {
            get
            {
                return frame.fFinal != 0;
            }
        }

        public DebugStackFrame GetStackFrame()
        {
            if (frame.pdsf == null)
                return null;

            return new DebugStackFrame(frame.pdsf);
        }

        internal DebugStackFrameDescriptor(tagDebugStackFrameDescriptor frame)
        {
            this.frame = frame;
        }
    }
}