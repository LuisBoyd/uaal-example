using RCRCoreLib.Core.Node.Nodes;
using UnityEngine;
using XNode;

namespace RCRCoreLib.Core.Node.Graphs
{
    [CreateAssetMenu]
    public class TutorialGraph : NodeGraph
    {
        public BaseTutorialNode startNode;
        public BaseTutorialNode currentNode;

        public void Start()
        {
            currentNode = startNode;
            Execute();
        }
        public void Execute()
        {
            currentNode.Execute();
        }

        public void Update()
        {
            currentNode.Update();
        }
    }
}