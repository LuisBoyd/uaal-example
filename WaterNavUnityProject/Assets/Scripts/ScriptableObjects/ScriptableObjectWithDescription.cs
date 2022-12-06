using UnityEngine;

namespace RCR.ScriptableObjects
{
    public abstract class ScriptableObjectWithDescription : ScriptableObject
    {
        [TextArea] public string Description;
    }
}