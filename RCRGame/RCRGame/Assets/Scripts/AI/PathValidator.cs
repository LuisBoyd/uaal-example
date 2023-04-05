using System;
using UnityEngine;
using WQS;

namespace AI
{
    [System.Serializable]
    public class PathValidator
    {
        private float interval = 0.1f;
        public float intervalRemaining = 0f;
        public float Interval
        {
            get => interval;
        } //Interval of time before we check the current path validity.
        private bool ShouldValidate { get; set; }


        //TODO: add differen't time options such as time.deltatime, time.scaled time .... etc
        
        public PathValidator(float interval) => this.interval = interval;

        public bool ValidatePath(Transform agent, Vector3 endpoint, PathFindingMode mode)
        {
            return PathEngine.Instance.FindPath(agent, endpoint, mode).Count != 0;
            //If the path here is not 0 then we are good to say the path is still valid.
        }

        public void StartValidating()
        {
            ShouldValidate = true;
            intervalRemaining = Interval;
        }

        public void StopValidating()
        {
            ShouldValidate = false;
            intervalRemaining = Interval;
        }

        public void PauseValidating() => ShouldValidate = false;
        public void ResumeValidating() => ShouldValidate = true;

        public bool Tick(Transform agent, Vector3 endPoint, PathFindingMode mode)
        {
            if(!ShouldValidate)
                return true;
            if (intervalRemaining < 0f)
            {
                //Time is Up we should validate the path.
                bool pathvalid = ValidatePath(agent, endPoint, mode); //if path is not valid we fail the AISteering.
                if (!pathvalid)
                    return false;
                intervalRemaining = Interval;
            }
            else
            {
                intervalRemaining -= Time.deltaTime; //TODO: add differen't time options such as time.deltatime, time.scaled time .... etc
                return true;
            }
            return true;
        }
        
    }
}