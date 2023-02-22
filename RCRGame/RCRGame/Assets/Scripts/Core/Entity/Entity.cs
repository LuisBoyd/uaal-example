using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.AI;
using RCRCoreLib.Core.AI.TaskSystem;
using RCRCoreLib.Core.AI.TaskSystem.Tasks;
using RCRCoreLib.Core.Animation;
using UnityEngine;

namespace RCRCoreLib.Core.Entity
{
    [RequireComponent(typeof(IsometricAnimatorController))]
    public class Entity : MonoBehaviour, IEntity
    {
        public Transform Transform { get; }
        private EntityType m_entityType;
        private IsometricAnimatorController IsometricAnimatorController;

        private bool PathSet
        {
            get
            {
                return Path != null;
            }
        }
        private IEnumerable<Vector3> Path;

        private void Awake()
        {
            IsometricAnimatorController = GetComponent<IsometricAnimatorController>();
        }

        public void Initialize(EntityType typeData)
        {
            m_entityType = typeData;
        }

        public void InterruptTask(TaskBase task)
        {
            throw new NotImplementedException();
        }
        public void MoveTo(Vector3 position, Action onArrivedAtPosition = null)
        {
            Path = PathFindingSystem.Instance.FindPath(transform, position, m_entityType.PathFindingMode);
            LTDescr d = LeanTween.moveSpline(gameObject, Path.ToArray(),  Path.Count() / m_entityType.MovementSpeed);
            d.setOnUpdate((Action<Vector3>) IsometricAnimatorController.SetDirection); //TODO Check Casting
            if (onArrivedAtPosition != null)
                d.setOnComplete(onArrivedAtPosition);
            //TODO more consistant speed modifier calculations
            // StartCoroutine(MovePathOverFrames(
            //     m_entityType.MovementSpeed * 60, onArrivedAtPosition));
            //TODO might need to get rid of LTdescr when changing paths
        }

        private IEnumerator MovePathOverFrames(float secondsTocomplete,Action OnCompleteCallback = null)
        {
            if(!PathSet)
                yield break;
            
            using (IEnumerator<Vector3> pathEnumerator = Path.GetEnumerator())
            {
                float elapsedTime = 0;
                secondsTocomplete = secondsTocomplete / Path.Count();
                while (pathEnumerator.MoveNext())
                {
                    while (elapsedTime < secondsTocomplete)
                    {
                        transform.position = Vector3.Lerp(transform.position, pathEnumerator.Current,
                            (elapsedTime / secondsTocomplete));
                        elapsedTime += Time.deltaTime;
                        //yield return new WaitForEndOfFrame();
                        //yield return new wa
                    }
                    transform.position = pathEnumerator.Current;
                    
                }
            }

            if (OnCompleteCallback != null)
                OnCompleteCallback();
        }
    }
}