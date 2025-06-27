using System;

namespace Terra.FSM {
    public class FuncPredicate : IPredicate {
        readonly Func<bool> _func;
        
        public FuncPredicate(Func<bool> func) {
            this._func = func;
        }
        
        public bool Evaluate() => _func.Invoke();
    }
}