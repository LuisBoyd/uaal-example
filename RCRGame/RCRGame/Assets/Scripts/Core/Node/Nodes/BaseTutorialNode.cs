using System;
using System.Linq;
using RCRCoreLib.Core.Node.Graphs;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;
using RCRCoreLib.TutorialEvents;
using UnityEngine;
using XNode;

namespace RCRCoreLib.Core.Node.Nodes
{
    public abstract class BaseTutorialNode : XNode.Node
    {
        [Input] public int entry;
        [Output] public int exit;

        //Initialization
        protected override void Init()
        {
            base.Init();
        }
        public virtual void Execute()
        {
            NextNode("exit");
        }

        public abstract void Update();
        public void NextNode(string _exit)
        {
            BaseTutorialNode b = null;
            foreach (NodePort port in this.Ports)
            {
                if (port.fieldName == _exit)
                {
                    //This is the one we are after
                    if (port.ConnectionCount <= 0)
                    {
                        EventManager.Instance.QueueEvent(new HideTutorialinterface());
                        EventManager.Instance.QueueEvent(new EndTutorialEvent());
                        return;
                    }

                    if (port.Connection.node == null)
                    {
                        EventManager.Instance.QueueEvent(new HideTutorialinterface());
                        EventManager.Instance.QueueEvent(new EndTutorialEvent());
                        return;
                    }
                    //There is no node connected to the main connection??
                    b = port.Connection.node as BaseTutorialNode;
                    break;
                }
            }

            if (b != null)
            {
                TutorialGraph _graph = this.graph as TutorialGraph;
                _graph.currentNode = b;
                _graph.Execute();
            }
            //TODO if no more in chain fire off event to say end and then enable all input again
        }
        
        
    }
}