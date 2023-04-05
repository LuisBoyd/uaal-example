using System.Linq;
using UnityEngine;

namespace WQS
{
    public static class GeneratorBuilder
    {
        public static bool Generate_SubjectPoint(GameObject querier, string overriddenKey,
            ref BlackBoard.Blackboard blackboard)
        {
            blackboard.SetVector3(overriddenKey, querier.transform.position);
            return true;
        }
        
        public static bool Generate_WQSObjs_RadiusPoint(GameObject querier,float radius, string overriddenKey,
            ref BlackBoard.Blackboard blackboard)
        {
            Collider2D[] hits2ds = Physics2D.OverlapCircleAll(querier.transform.position, radius);
            if (hits2ds != null)
            {
                if (hits2ds.Length > 0)
                {
                    blackboard.SetGeneric(overriddenKey, hits2ds);
                    return true;
                }
            }
            return false;
        }
        
        public static bool Generate_WQSObjs_WithTag(GameObject querier,string tag, string overriddenKey,
            ref BlackBoard.Blackboard blackboard)
        {
            var WqsQuery = WQS.Instance.GetCompsOfType(typeof(WorldObject));
            if (WqsQuery != null)
            {
                WqsQuery = WqsQuery.Where(x => x.tag.Equals(tag)).ToList();
                if (WqsQuery.Count > 1)
                {
                    blackboard.SetGeneric(overriddenKey, WqsQuery);
                    return true;
                }
            }
            return false;
        }
        
        public static bool Generate_WQSObjs_WithTag_AroundRadius(GameObject querier,string tag,float radius, string overriddenKey,
            ref BlackBoard.Blackboard blackboard)
        {
            var WqsQuery = WQS.Instance.GetCompsOfType(typeof(WorldObject));
            if (WqsQuery != null)
            {
                WqsQuery = WqsQuery.Where(x => x.tag.Equals(tag) &&
                                               Vector2.Distance(querier.transform.position,
                                                   x.transform.position) < radius).ToList();
                if (WqsQuery.Count > 1)
                {
                    blackboard.SetGeneric(overriddenKey, WqsQuery);
                    return true;
                }
            }
            return false;
        }
        
        public static bool Generate_WQSObjs_Types<T>(GameObject querier, string overriddenKey,
            ref BlackBoard.Blackboard blackboard) where T : WorldObject
        {
            var WqsQuery = WQS.Instance.GetCompsOfType(typeof(T));
            if (WqsQuery != null)
            {
                if (WqsQuery.Count > 1)
                {
                    blackboard.SetGeneric(overriddenKey, WqsQuery);
                    return true;
                }
            }
            return false;
        }
        public static bool Generate_WQSObjs_Types_AroundPoint<T>(GameObject querier,float radius,string overriddenKey,
            ref BlackBoard.Blackboard blackboard) where T : WorldObject
        {
            var WqsQuery = WQS.Instance.GetCompsOfType(typeof(T));
            if (WqsQuery != null)
            {
                WqsQuery = WqsQuery.Where(x => Vector2.Distance(x.transform.position, querier.transform.position)
                                               < radius).ToList();
                if (WqsQuery.Count > 1)
                {
                    blackboard.SetGeneric(overriddenKey, WqsQuery);
                    return true;
                }
            }
            return false;
        }
    }
}