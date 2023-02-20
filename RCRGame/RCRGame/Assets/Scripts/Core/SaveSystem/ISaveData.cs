using Unity.Collections;
using UnityEngine;

namespace RCRCoreLib.Core.SaveSystem
{
    public interface ISaveData<U,T> where T : Data where U : ScriptableObject
    {
        public void Initialize(U item);
        public void Initialize(U item, T data);
        
        /// <summary>
        /// After all data has been loaded. any after private functionality
        /// </summary>
        public void Load();
        
    }
}