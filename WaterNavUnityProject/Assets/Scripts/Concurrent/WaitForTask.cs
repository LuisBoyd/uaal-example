using System.Threading.Tasks;
using UnityEngine;

namespace Concurrent
{
    public class WaitForTask : CustomYieldInstruction
    {
        private Task m_t;

        public override bool keepWaiting
        {
            get => m_t.IsCompleted;
        }

        public WaitForTask(Task t)
        {
            m_t = t;
        }
    }
}