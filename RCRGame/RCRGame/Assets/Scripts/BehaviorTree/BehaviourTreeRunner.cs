using System;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField]
        private BehaviorTree tree;

        private void Start()
        {
            Debug.Log($"Before Copy: {tree.GetInstanceID()}");
            tree = tree.DeepCopy();
            Debug.Log($"After Copy: {tree.GetInstanceID()}");
        }

        private void Update()
        {
            tree.Update();
        }
    }
}