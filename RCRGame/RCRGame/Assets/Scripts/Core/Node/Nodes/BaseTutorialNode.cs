using System;
using System.Linq;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.TutorialEvents;
using RCRCoreLib.Core.Node.Graphs;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;
using UnityEngine;
using XNode;

namespace RCRCoreLib.Core.Node.Nodes
{
    public abstract class BaseTutorialNode : XNode.Node
    {
        [Input] public int entry;
        [Output] public int exit;
        
        [TextArea]
        public string Message; //The Text that you want displayed in the text box.
        public bool HorizontalFlipped = false; // Should we reverse the order so that the avatar and text box appear in reverse order.
        public Vector2 LocationOnScreen; //Where should the UI pop up.

        //Initialization
        protected override void Init()
        {
            base.Init();
        }
        public virtual void Execute()
        {
            NextNode("exit");
        }
        public virtual void NextNode(string _exit)
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