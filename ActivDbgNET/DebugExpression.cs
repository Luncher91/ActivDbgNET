using System;
using ActivDbg;

namespace ActivDbgNET
{
    public class DebugExpression
    {
        private IDebugExpression dexp;

        public DebugExpression(IDebugExpression dexp)
        {
            this.dexp = dexp;
        }

        public void Start(Action<DebugExpression> onComplete)
        {
            DebugExpressionCallback callbackObj = new DebugExpressionCallback(this, onComplete);
            dexp.Start(callbackObj);
        }

        public void Abort()
        {
            dexp.Abort();
        }

        public StringResultStruct GetResultAsString()
        {
            var resultStruct = new StringResultStruct();

            dexp.GetResultAsString(out resultStruct.Status, out resultStruct.Result);

            return resultStruct;
        }

        public PropertyResultStruct GetResultAsDebugProperty()
        {
            var resultStruct = new PropertyResultStruct();
            IDebugProperty debugProp;

            dexp.GetResultAsDebugProperty(out resultStruct.Status, out debugProp);

            resultStruct.Result = new DebugProperty(debugProp);
            return resultStruct;
        }

        public struct StringResultStruct
        {
            public int Status;
            public string Result;
        }

        public struct PropertyResultStruct
        {
            public int Status;
            public DebugProperty Result;
        }

        internal class DebugExpressionCallback : IDebugExpressionCallBack
        {
            private Action<DebugExpression> onCompleteCallback;
            private DebugExpression debugExpr;

            public DebugExpressionCallback(DebugExpression debugExpr, Action<DebugExpression> onCompleteCallback)
            {
                this.onCompleteCallback = onCompleteCallback;
                this.debugExpr = debugExpr;
            }

            public void onComplete()
            {
                onCompleteCallback?.Invoke(debugExpr);
            }
        }
    }
}