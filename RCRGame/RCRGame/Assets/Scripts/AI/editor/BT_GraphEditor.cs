using System;
using AI.behavior_tree;
using AI.TaskLibary;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace AI.editor
{
    [CustomNodeGraphEditor(typeof(BehaviorTree))]
    public class BT_GraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(Type type)
        {
            if (type.Namespace.Contains("AI."))
            {
                return base.GetNodeMenuName(type).Replace("AI/","");
            }

            return null;
        }

        public override Gradient GetNoodleGradient(NodePort output, NodePort input)
        {
            Gradient grad = new Gradient();
            
            // If dragging the noodle, draw solid, slightly transparent
            if (input == null)
            {
                grad = DraggingDraw(output);
            }
            // If normal, draw gradient fading from one input color to the other
            else
            {
                BaseNode inNode = input.node as BaseNode;
                if (inNode != null)
                    switch (inNode.LastStatus)
                    {
                        case TaskStatus.Running:
                            grad = NormalDraw(output, input, Color.green, Color.green);
                            break;
                        case TaskStatus.Failed:
                            grad = NormalDraw(output, input, Color.red, Color.red);
                            break;
                        default:
                            grad = NormalDraw(output, input);
                            break;
                    }
                else
                {
                    grad = NormalDraw(output, input);
                }
            }
            return grad;
        }
        private Gradient DraggingDraw(NodePort output)
        {
            Gradient grad = new Gradient();

            Color a = GetTypeColor(output.ValueType);
            grad.SetKeys(
                new GradientColorKey[] {new GradientColorKey(a, 0f)},
                new GradientAlphaKey[] {new GradientAlphaKey(0.6f, 0f)}
            );
            return grad;
        }
        private Gradient DraggingDraw(Color c)
        {
            Gradient grad = new Gradient();

            Color a = c;
            grad.SetKeys(
                new GradientColorKey[] {new GradientColorKey(a, 0f)},
                new GradientAlphaKey[] {new GradientAlphaKey(0.6f, 0f)}
            );
            return grad;
        }

        private Gradient NormalDraw(NodePort output, NodePort input)
        {
            Gradient grad = new Gradient();

            Color a = GetTypeColor(output.ValueType);
            Color b = GetTypeColor(input.ValueType);

            if (window.hoveredPort == output || window.hoveredPort == input)
            {
                a = Color.Lerp(a, Color.white, 0.8f);
                b = Color.Lerp(b, Color.white, 0.8f);
            }
            grad.SetKeys(
                new GradientColorKey[]{new GradientColorKey(a, 0f), new GradientColorKey(b, 1f)},
                new GradientAlphaKey[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f)}
                );
            return grad;
        }
        
        private Gradient NormalDraw(NodePort output, NodePort input, Color c1, Color c2)
        {
            Gradient grad = new Gradient();

            Color a = c1;
            Color b = c2;

            if (window.hoveredPort == output || window.hoveredPort == input)
            {
                a = Color.Lerp(a, Color.white, 0.8f);
                b = Color.Lerp(b, Color.white, 0.8f);
            }
            grad.SetKeys(
                new GradientColorKey[]{new GradientColorKey(a, 0f), new GradientColorKey(b, 1f)},
                new GradientAlphaKey[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f)}
            );
            return grad;
        }
    }
}