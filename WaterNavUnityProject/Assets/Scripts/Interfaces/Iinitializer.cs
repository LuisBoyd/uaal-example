using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;
using RCR.DataStructures;

namespace Interfaces
{
    public interface Iinitializer
    {
        public IEnumerator Process_Init(CoroutineToken tkn);
        
        public void Init_CleanUp();
    }
}