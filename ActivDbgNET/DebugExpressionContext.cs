using ActivDbg;
using System;

namespace ActivDbgNET
{
    public class DebugExpressionContext
    {
        private const uint RADIX = 10;
        private const uint EVAL_FLAGS = 
              (uint)DebugTextFlags.DEBUG_TEXT_ISEXPRESSION 
            | (uint)DebugTextFlags.DEBUG_TEXT_RETURNVALUE
            | (uint)DebugTextFlags.DEBUG_TEXT_NOSIDEEFFECTS
            | (uint)DebugTextFlags.DEBUG_TEXT_ALLOWBREAKPOINTS
            | (uint)DebugTextFlags.DEBUG_TEXT_ALLOWERRORREPORT;

        private IDebugExpressionContext dec;

        public DebugExpressionContext(IDebugExpressionContext debugExpressionContext)
        {
            this.dec = debugExpressionContext;
        }

        public LanguageInfo GetLanguageInfo()
        {
            LanguageInfo li = new LanguageInfo();
            dec.GetLanguageInfo(out li.Name, out li.LanguageID);
            return li;
        }

        public DebugExpression EvaluateExpression(string txt)
        {
            IDebugExpression dexp;
            dec.ParseLanguageText(txt, RADIX, null, EVAL_FLAGS, out dexp);

            return new DebugExpression(dexp);
        }

        internal enum DebugTextFlags
        {
            DEBUG_TEXT_ISEXPRESSION = 0x01,
            DEBUG_TEXT_RETURNVALUE = 0x02,
            DEBUG_TEXT_NOSIDEEFFECTS = 0x04,
            DEBUG_TEXT_ALLOWBREAKPOINTS = 0x08,
            DEBUG_TEXT_ALLOWERRORREPORT = 0x10,
            DEBUG_TEXT_EVALUATETOCODECONTEXT = 0x20,
        }


        public struct LanguageInfo
        {
            public string Name;
            public Guid LanguageID;
        }
    }
}