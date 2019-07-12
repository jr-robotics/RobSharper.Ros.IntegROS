using System;

namespace RosComponentTesting
{
    public class Match<T>
    {
        private Func<T, bool> _f;
        
        public Match(Func<T, bool> evaluationFunc)
        {
            _f = evaluationFunc;
        }

        public bool Evaluate(T item)
        {
            return _f(item);
        }
    }
}