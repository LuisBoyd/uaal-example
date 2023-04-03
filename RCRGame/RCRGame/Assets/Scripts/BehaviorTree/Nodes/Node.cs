using UnityEngine;

namespace BehaviorTree.Nodes
{
    public abstract class Node : ScriptableObject
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
        public State state = State.Running;

        /// <summary>
        /// Flag to indicate if the node has ran before.
        /// </summary>
        public bool started = false;

        public string guid;

        public Vector2 Position;
        
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
    }
}