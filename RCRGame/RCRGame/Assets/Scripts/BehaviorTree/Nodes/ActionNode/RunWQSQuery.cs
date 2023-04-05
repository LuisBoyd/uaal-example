using System;
using UnityEngine;
using WQS;

namespace BehaviorTree.Nodes.ActionNode
{
    public class RunWQSQuery : ActionNode
    {
        [SerializeField] 
        private GeneratorType GeneratorType;
        [SerializeField] 
        private float radius;
        

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
           
        }

        protected override State OnUpdate()
        {
            switch (GeneratorType)
            {
                case GeneratorType.Generate_SubjectPoint:
                    state = GeneratorBuilder.Generate_SubjectPoint(agent.gameObject,
                        "CurrentLocation", ref blackboard)
                        ? State.Success
                        : State.Failure;
                    break;
                case GeneratorType.Generate_WQSObjs_RadiusPoint:
                    state = GeneratorBuilder.Generate_WQSObjs_RadiusPoint(agent.gameObject, radius,
                        "WQSMembersInRange", ref blackboard)
                        ? State.Success
                        : State.Failure;
                    break;
                default:
                    state = State.Failure;
                    break;
            }
            return state;
        }
    }
}