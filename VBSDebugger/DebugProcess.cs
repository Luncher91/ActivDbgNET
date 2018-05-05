using System;
using ActivDbg;

namespace VBSDebugger
{
    public class DebugProcess
    {
        private IRemoteDebugApplication rda;
        private string name = null;

        public string Name
        {
            get
            {
                if (name == null)
                    rda.GetName(out name);
                return name;
            }
        }

        public DebugProcess(IRemoteDebugApplication rda)
        {
            this.rda = rda;
        }

        public void ConnectDebugger(ScriptDebugger debugger)
        {
            debugger.Close += rda.DisconnectDebugger;
            rda.ConnectDebugger(debugger);
        }

        internal void Resume(DebugApplication debugApp)
        {
            rda.ResumeFromBreakPoint(debugApp.RemoteDebugApplicationThread, tagBREAKRESUME_ACTION.BREAKRESUMEACTION_IGNORE, tagERRORRESUMEACTION.ERRORRESUMEACTION_AbortCallAndReturnErrorToCaller);
        }
    }

    public delegate void CloseHandler();

    public interface ScriptDebugger : IApplicationDebugger
    {
        event CloseHandler Close;
    }
}