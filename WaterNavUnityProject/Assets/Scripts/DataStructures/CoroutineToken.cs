using System;
using RCR.BaseClasses;

namespace DataStructures
{
    public struct CoroutineToken
    {
        public DelegateNoArg on_cancel;
        
        public void Cancel()
        {
            on_cancel?.Invoke();
        }
    }
}