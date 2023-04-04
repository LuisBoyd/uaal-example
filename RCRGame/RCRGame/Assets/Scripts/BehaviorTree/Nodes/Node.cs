using System;
using AI;
using BlackBoard;
using UnityEngine;
using Utilities;

namespace BehaviorTree.Nodes
{
    public abstract class Node : ScriptableObject, ICopy<Node>
    {
        public enum State
        {
            Running,
            Failure,
            Success,
            Aborted
        }

        /// <summary>
        /// The state the node is in currently
        /// </summary>
        [HideInInspector]public State state = State.Running;

        /// <summary>
        /// Flag to indicate if the node has ran before.
        /// </summary>
        [HideInInspector]public bool started = false;

        [HideInInspector]public string guid;

        [HideInInspector]public Vector2 Position;

        [HideInInspector] public Blackboard blackboard;

        [HideInInspector] public AiAgent agent;

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();
            if (state == State.Failure || state == State.Success)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        public abstract Node DeepCopy();

        public virtual object Clone() => DeepCopy();

    }
}