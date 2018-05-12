using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET
{
    public class RemoteDebugApplication
    {
        private IRemoteDebugApplication remoteDebugApplication;

        internal RemoteDebugApplication(IRemoteDebugApplication rda)
        {
            remoteDebugApplication = rda;
        }

        public string GetName()
        {
            string name = "";
            remoteDebugApplication.GetName(out name);
            return name;
        }

        public void ConnectDebugger(ApplicationDebugger debugger)
        {
            remoteDebugApplication.ConnectDebugger(debugger.GetIApplicationDebugger());
        }

        public void DisconnectDebugger()
        {
            remoteDebugApplication.DisconnectDebugger();
        }

        private enum ResumeAction
        {
            Continue,
            StepIn,
            StepOut,
            StepOver,
            Abort
        }

        private void Resume(RemoteDebugApplicationThread t, ResumeAction r)
        {
            tagBREAKRESUME_ACTION action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_IGNORE;

            switch (r)
            {
                case ResumeAction.Continue:
                    action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_CONTINUE;
                    break;
                case ResumeAction.StepIn:
                    action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_STEP_INTO;
                    break;
                case ResumeAction.StepOut:
                    action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_STEP_OUT;
                    break;
                case ResumeAction.StepOver:
                    action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_STEP_OVER;
                    break;
                case ResumeAction.Abort:
                    action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_ABORT;
                    break;
                default:
                    action = tagBREAKRESUME_ACTION.BREAKRESUMEACTION_CONTINUE;
                    break;
            }

            remoteDebugApplication.ResumeFromBreakPoint(t.GetRemoteDebugApplicationThread(), action, tagERRORRESUMEACTION.ERRORRESUMEACTION_AbortCallAndReturnErrorToCaller);
        }

        public void Continue(RemoteDebugApplicationThread t)
        {
            Resume(t, ResumeAction.Continue);
        }

        public void StepIn(RemoteDebugApplicationThread t)
        {
            Resume(t, ResumeAction.StepIn);
        }

        public void StepOver(RemoteDebugApplicationThread t)
        {
            Resume(t, ResumeAction.StepOver);
        }

        public void StepOut(RemoteDebugApplicationThread t)
        {
            Resume(t, ResumeAction.StepOut);
        }

        public void Abort(RemoteDebugApplicationThread t)
        {
            Resume(t, ResumeAction.Abort);
        }
    }
}
