using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.Events;

namespace Utility
{
    public static class UniTaskHelper
    {
        public static Action<T> Action<T>(Func<T, UniTaskVoid> asyncAction)
        {
            return (t1) => asyncAction(t1).Forget();
        }
        public static Action<T0,T1> Action<T0,T1>(Func<T0,T1, UniTaskVoid> asyncAction)
        {
            return (t0,t1) => asyncAction(t0,t1).Forget();
        }
        public static Action<T0,T1,T2> Action<T0,T1,T2>(Func<T0,T1,T2, UniTaskVoid> asyncAction)
        {
            return (t0,t1,t2) => asyncAction(t0,t1,t2).Forget();
        }
        public static Action<T0,T1,T2,T3> Action<T0,T1,T2,T3>(Func<T0,T1,T2,T3, UniTaskVoid> asyncAction)
        {
            return (t0,t1,t2,t3) => asyncAction(t0,t1,t2,t3).Forget();
        }
        public static Action<T0,T1,T2,T3,T4> Action<T0,T1,T2,T3,T4>(Func<T0,T1,T2,T3,T4, UniTaskVoid> asyncAction)
        {
            return (t0,t1,t2,t3,t4) => asyncAction(t0,t1,t2,t3,t4).Forget();
        }
        
        public static UnityAction UnityAction(Func<UniTaskVoid> asyncAction)
        {
            return () => asyncAction().Forget();
        }
        public static UnityAction<T> UnityAction<T>(Func<T, UniTaskVoid> asyncAction)
        {
            return (t1) => asyncAction(t1).Forget();
        }
        public static UnityAction<T0,T1> UnityAction<T0,T1>(Func<T0,T1, UniTaskVoid> asyncAction)
        {
            return (t0,t1) => asyncAction(t0,t1).Forget();
        }
        public static UnityAction<T0,T1,T2> UnityAction<T0,T1,T2>(Func<T0,T1,T2, UniTaskVoid> asyncAction)
        {
            return (t0,t1,t2) => asyncAction(t0,t1,t2).Forget();
        }
        public static UnityAction<T0,T1,T2,T3> UnityAction<T0,T1,T2,T3>(Func<T0,T1,T2,T3, UniTaskVoid> asyncAction)
        {
            return (t0,t1,t2,t3) => asyncAction(t0,t1,t2,t3).Forget();
        }
        
        //
        public static UnityAction UnityAction(Func<UniTask> asyncAction)
        {
            return () => asyncAction().Forget();
        }
        public static UnityAction<T> UnityAction<T>(Func<T, UniTask> asyncAction)
        {
            return (t1) => asyncAction(t1).Forget();
        }
        public static UnityAction<T0,T1> UnityAction<T0,T1>(Func<T0,T1, UniTask> asyncAction)
        {
            return (t0,t1) => asyncAction(t0,t1).Forget();
        }
        public static UnityAction<T0,T1,T2> UnityAction<T0,T1,T2>(Func<T0,T1,T2, UniTask> asyncAction)
        {
            return (t0,t1,t2) => asyncAction(t0,t1,t2).Forget();
        }
        public static UnityAction<T0,T1,T2,T3> UnityAction<T0,T1,T2,T3>(Func<T0,T1,T2,T3, UniTask> asyncAction)
        {
            return (t0,t1,t2,t3) => asyncAction(t0,t1,t2,t3).Forget();
        }
    }
}