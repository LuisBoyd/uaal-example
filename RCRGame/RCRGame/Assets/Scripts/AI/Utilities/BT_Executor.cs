using System;
using AI.behavior_tree;
using UnityEngine;

namespace AI.Utilities
{
    public class BT_Executor : MonoBehaviour
    {
        [SerializeField] 
        private BehaviorTree BT;

        private void Awake()
        {
            if (BT != null)
            {
                BT = BT.Copy() as BehaviorTree; 
                if(BT == null)
                    throw new Exception("Behavior Tree is Empty");
            }
        }
        private void OnDestroy()
        {
            BT.Clear();
            BT = null;
        }
        private void Update() => BT.Tick();
    }
}