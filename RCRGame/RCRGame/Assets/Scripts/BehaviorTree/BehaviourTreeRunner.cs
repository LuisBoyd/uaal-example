using System;
using AI;
using BlackBoard;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviorTree tree;

        private void Start()
        {
            Debug.Log($"Before Copy: {tree.GetInstanceID()}");
            tree = tree.DeepCopy();
            if (tree.blackboard != null)
                tree = tree.DeepCopy();
            else
            {
                tree.blackboard = ScriptableObject.CreateInstance<Blackboard>();
            }
            tree.Bind(GetComponent<AiAgent>());
            Debug.Log($"After Copy: {tree.GetInstanceID()}");
        }

        private void Update()
        {
            tree.Update();
        }
    }
}