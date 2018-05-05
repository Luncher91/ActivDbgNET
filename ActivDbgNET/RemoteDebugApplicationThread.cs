using ActivDbg;
using System.Collections.Generic;

namespace ActivDbgNET
{
    public class RemoteDebugApplicationThread
    {
        private IRemoteDebugApplicationThread prpt;

        internal RemoteDebugApplicationThread(IRemoteDebugApplicationThread prpt)
        {
            this.prpt = prpt;
        }

        private DebugStackFrameDescriptor[] GetDebugStackFrameDescriptors()
        {
            List<DebugStackFrameDescriptor> frames = new List<DebugStackFrameDescriptor>();

            // get enum
            IEnumDebugStackFrames stackFrames = null;
            prpt.EnumStackFrames(out stackFrames);

            // temporary variables
            tagDebugStackFrameDescriptor frame;
            uint fetched = 0;

            do
            {
                fetched = 0;
                stackFrames.RemoteNext(1, out frame, out fetched);
                frames.Add(new DebugStackFrameDescriptor(frame));
            } while (fetched > 0);

            return frames.ToArray();
        }
    }
}