using ActivDbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSDebugger
{
    public class ApplicationDebugger : ScriptDebugger
    {
        public delegate void BreakPointHandler(DebugApplication app, BreakReason reason, ScriptError error);

        public event CloseHandler Close;
        public event BreakPointHandler BreakPoint;

        void IApplicationDebugger.CreateInstanceAtDebugger(ref Guid rclsid, object pUnkOuter, uint dwClsContext, ref Guid riid, out object ppvObject)
        {
            Console.WriteLine("== CreateInstanceAtDebugger ==");
            Console.WriteLine("GUID rclsid: " + rclsid.ToString(""));
            Console.WriteLine("GUID riid: " + riid.ToString(""));
            ppvObject = this;
            Console.WriteLine("-- CreateInstanceAtDebugger --");
        }

        void IApplicationDebugger.onClose()
        {
            Close?.Invoke();
            Console.WriteLine("onClose");
        }

        void IApplicationDebugger.onDebuggerEvent(ref Guid riid, object punk)
        {
            Console.WriteLine("== onDebuggerEvent ==");
            Console.WriteLine("-- onDebuggerEvent --");
        }

        void IApplicationDebugger.onDebugOutput(string pstr)
        {
            Console.WriteLine("DEBUG OUT: " + pstr);
        }

        void IApplicationDebugger.onHandleBreakPoint(IRemoteDebugApplicationThread prpt, tagBREAKREASON br, IActiveScriptErrorDebug pError)
        {
            var thread = new DebugApplication(prpt);
            ScriptError error = null;

            if (pError != null)
                error = new ScriptError(pError);

            BreakPoint?.Invoke(thread, br.ToBreakReason(), error);
        }

        void IApplicationDebugger.QueryAlive()
        {
            Console.WriteLine("Query Alive!");
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
}
