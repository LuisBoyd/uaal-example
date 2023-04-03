using System;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviorTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviorTree>();
        }
    }
}