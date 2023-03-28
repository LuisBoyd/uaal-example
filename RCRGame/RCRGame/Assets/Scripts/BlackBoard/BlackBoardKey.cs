using UnityEngine;

namespace BlackBoard
{
    public class BlackBoardKey<T> : ScriptableObject
    {
        public string EntryName;
        [TextArea]
        public string EntryDescription;
        public BlackBoardKeyType KeyType;
        public bool InstanceSynced; //means for all other blackboard keys of the same it will be synced. static seperateValue??
    }
}