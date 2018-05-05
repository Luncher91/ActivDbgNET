using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivDbgNET
{
    public abstract class ApplicationDebugger
    {
        private ApplicationDebuggerImplementation appDebugger;

        public delegate void BreakPointHandler(RemoteDebugApplicationThread debugAppThread, BreakReason reason, ActiveScriptErrorDebug error);
        public event BreakPointHandler BreakPoint;

        public delegate void CloseHandler();
        public event CloseHandler Close;

        public delegate void DebugOutputHandler(string output);
        public event DebugOutputHandler DebugOutput;

        public delegate void DebuggerEventHandler(Guid interfaceId, object punk);
        public event DebuggerEventHandler DebugEvent;

        protected ApplicationDebugger()
        {
            appDebugger = new ApplicationDebuggerImplementation();
            appDebugger.BreakPoint += AppDebugger_BreakPoint;
            appDebugger.Close += AppDebugger_Close;
            appDebugger.DebugOutput += AppDebugger_DebugOutput;
            appDebugger.DebuggerEvent += AppDebugger_DebuggerEvent;
        }

        private void AppDebugger_DebuggerEvent(Guid riid, object punk)
        {
            DebugEvent?.Invoke(riid, punk);
        }

        private void AppDebugger_DebugOutput(string output)
        {
            DebugOutput?.Invoke(output);
        }

        private void AppDebugger_Close()
        {
            Close?.Invoke();
        }

        private void AppDebugger_BreakPoint(IRemoteDebugApplicationThread prpt, tagBREAKREASON br, IActiveScriptErrorDebug pError)
        {
            RemoteDebugApplicationThread rdat = null;

            if (prpt != null)
                rdat = new RemoteDebugApplicationThread(prpt);

            ActiveScriptErrorDebug ased = null;

            if(pError != null)
                ased = new ActiveScriptErrorDebug(pError);

            BreakPoint?.Invoke(rdat, br.ToBreakReason(), ased);
        }

        internal IApplicationDebugger GetIApplicationDebugger()
        {
            return appDebugger;
        }        
    }

    public enum BreakReason
    {
        BreakPoint,
        DebuggerBlock,
        DebuggerHalt,
        Error,
        HostInitiated,
        JIT,
        LanguageInitiated,
        MutationBreakpoint,
        Step
    }

    internal static class tagBreakReasonMethods
    {
        internal static BreakReason ToBreakReason(this tagBREAKREASON br)
        {
            switch (br)
            {
                case tagBREAKREASON.BREAKREASON_STEP:
                    return BreakReason.Step;
                case tagBREAKREASON.BREAKREASON_BREAKPOINT:
                    return BreakReason.BreakPoint;
                case tagBREAKREASON.BREAKREASON_DEBUGGER_BLOCK:
                    return BreakReason.DebuggerBlock;
                case tagBREAKREASON.BREAKREASON_HOST_INITIATED:
                    return BreakReason.HostInitiated;
                case tagBREAKREASON.BREAKREASON_LANGUAGE_INITIATED:
                    return BreakReason.LanguageInitiated;
                case tagBREAKREASON.BREAKREASON_DEBUGGER_HALT:
                    return BreakReason.DebuggerHalt;
                case tagBREAKREASON.BREAKREASON_ERROR:
                    return BreakReason.Error;
                case tagBREAKREASON.BREAKREASON_JIT:
                    return BreakReason.JIT;
                case tagBREAKREASON.BREAKREASON_MUTATION_BREAKPOINT:
                    return BreakReason.MutationBreakpoint;
                default:
                    return BreakReason.Step;
            }
        }
    }

    internal class ApplicationDebuggerImplementation : IApplicationDebugger
    {
        internal delegate void CreateInstanceAtDebuggerHandler(Guid rclsid, object pUnkOuter, uint dwClsContext, Guid riid);
        internal event CreateInstanceAtDebuggerHandler CreateInstanceAtDebuggerEvent;

        internal delegate void CloseHandler();
        internal event CloseHandler Close;

        internal delegate void DebuggerEventHandler(Guid riid, object punk);
        internal event DebuggerEventHandler DebuggerEvent;

        internal delegate void DebugOutputHandler(string output);
        internal event DebugOutputHandler DebugOutput;

        internal delegate void BreakPointHandler(IRemoteDebugApplicationThread prpt, tagBREAKREASON br, IActiveScriptErrorDebug pError);
        internal event BreakPointHandler BreakPoint;

        internal delegate void QueryAliveHandler();
        internal event QueryAliveHandler AliveInquiry;

        public void CreateInstanceAtDebugger(ref Guid rclsid, object pUnkOuter, uint dwClsContext, ref Guid riid, out object ppvObject)
        {
            ppvObject = this;
            CreateInstanceAtDebuggerEvent?.Invoke(rclsid, pUnkOuter, dwClsContext, riid);
        }

        public void onClose()
        {
            Close?.Invoke();
        }

        public void onDebuggerEvent(ref Guid riid, object punk)
        {
            DebuggerEvent?.Invoke(riid, punk);
        }

        public void onDebugOutput(string pstr)
        {
            DebugOutput?.Invoke(pstr);
        }

        public void onHandleBreakPoint(IRemoteDebugApplicationThread prpt, tagBREAKREASON br, IActiveScriptErrorDebug pError)
        {
            BreakPoint?.Invoke(prpt, br, pError);
        }

        public void QueryAlive()
        {
            AliveInquiry?.Invoke();
        }
    }
}
