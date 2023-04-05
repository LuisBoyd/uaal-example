using System;
using UnityEngine;
using WQS;

namespace AI
{
    [RequireComponent(typeof(AiAgent))]
    public class AISteering : MonoBehaviour
    {
        [SerializeField] 
        private PathFindingMode PathFindingMode;
        [SerializeField] 
        private SteeringDetails SteeringDetails; //Options that control how the Agent move's
        public bool HasSetPath { get; private set; } = false; //if are path is currently set.
        public bool CurrentlyFollowingPath { get; private set; } = false; //are we currently following a path.
        private Path _current;
        public Path Current
        {
            get
            {
                if (_current == null)
                    return new Path(this);
                return _current;
            }
            private set
            {
                _current = value;
            }
        } //The current path.

        private PathValidator validator; //How often we should check our current path for.
        private bool ClearPathOnNextFrame = false; //in the Update on the next frame clear the current path.
        public bool IsStopped { get; private set; } = true; //is the AiSteering Agent Currently Stopped.
        public float DistanceToTarget { get; private set; } //The Current Distance we are away from the target.
        public event EventHandler<AgentAIeventArgs> OnCompletePath; //Action to complete if we successfully complete the path.
        public Action OnPathInterrupt; //called if we clear our current path before we reach the target.
        public Action OnPathStopped; //Called when the agent stops on a path but can still resume.
        public Action OnPathResume; //Called when we resume a path.
        public event EventHandler<AgentAIeventArgs> OnPathFailure; //Called when the path is a failure for some reason Note: See A*.
        private LTDescr _tweeningMovement;
        private LTDescr TweeningMovement
        {
            get
            {
                if (_tweeningMovement == null)
                    return new LTDescr();
                return _tweeningMovement;
            }
            set => _tweeningMovement = value;
        }

        private void Awake()
        {
            validator = new PathValidator(SteeringDetails.ValidationInterval);
        }

        private void Update()
        {
            if(!HasSetPath || IsStopped)
                return;
            bool validPath = validator.Tick(transform, Current.CurrentPath[^1], PathFindingMode);
            if (!validPath)
            {
                OnPath_Failed();
                return;
            }
            DistanceToTarget = Vector2.Distance(transform.position, Current.CurrentPath[^1]);
        }

        private void OnPath_Failed()
        {
            if (LeanTween.isTweening(TweeningMovement.id))
            {
                //We would be cancelling the currentMovement.
                LeanTween.cancel(TweeningMovement.id);
                TweeningMovement.reset();
            }
            OnPathFailure?.Invoke(this,new AgentAIeventArgs(GetComponent<AiAgent>()));
        }

        private void OnPath_Complete()
        {
            if (LeanTween.isTweening(TweeningMovement.id))
            {
                //Bug potential bug here..
                LeanTween.cancel(TweeningMovement.id);
                TweeningMovement.reset();
            }
            OnCompletePath?.Invoke(this,new AgentAIeventArgs(GetComponent<AiAgent>()));
        }
        
        private void ClearCurrentPath()
        {
            if (LeanTween.isTweening(TweeningMovement.id))
            {
                //We would be cancelling the currentMovement.
                LeanTween.cancel(TweeningMovement.id);
                TweeningMovement.reset();
                OnPathInterrupt?.Invoke(); // Invoke the Interrupt Path Action.
            }
        }

        public bool SetPath(Transform endLocation)
        {
            var path = PathEngine.Instance.FindPath(transform,endLocation, PathFindingMode).ToArray();
            if (path.Length == 0)
                return false; //The path has a length of 0 meaning something has gone wrong but there is no path.
            ClearCurrentPath();
            Current.SetNewPath(path);
            TweeningMovement =  LeanTween.moveSpline(gameObject, Current.CurrentPath,  Current.PathLength / SteeringDetails.Speed);
            TweeningMovement.setOnComplete(OnPath_Complete);
            HasSetPath = true;
            IsStopped = false;
            return true;
        }

        public bool SetPath(Vector3 endLocation)
        {
            var path = PathEngine.Instance.FindPath(transform,endLocation, PathFindingMode).ToArray();
            if (path.Length == 0)
                return false; //The path has a length of 0 meaning something has gone wrong but there is no path.
            ClearCurrentPath();
            Current.SetNewPath(path);
            TweeningMovement =  LeanTween.moveSpline(gameObject, Current.CurrentPath,  Current.PathLength / SteeringDetails.Speed);
            TweeningMovement.setOnComplete(OnPath_Complete);
            HasSetPath = true;
            IsStopped = false;
            return true;
        }

        public void Stop()
        {
            if (LeanTween.isTweening(TweeningMovement.id))
            {
                LeanTween.pause(TweeningMovement.id);
                IsStopped = true;
                validator.PauseValidating();
                OnPathStopped?.Invoke(); //Invoke stopped action.
            }
        }

        public void Resume()
        {
            if (!LeanTween.isTweening(TweeningMovement.id))
            {
                LeanTween.resume(TweeningMovement.id);
                IsStopped = false;
                validator.ResumeValidating();
                OnPathResume?.Invoke(); //Invoke resume action.
            }
        }
    }
}