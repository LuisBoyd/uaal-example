using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCR.Tiled
{
    [System.Serializable]
    public class GIDsDatabase
    {
#if UNITY_EDITOR
        
        /// <summary>
        /// Called on Loaded or when a value of this object changes in the Inspector
        /// </summary>
        private void OnValidate()
        {
           //TODO implement Validation Checks Editor Side
        }
#endif
        
        //TODO this can only store 8,191 tiles
    }
}
