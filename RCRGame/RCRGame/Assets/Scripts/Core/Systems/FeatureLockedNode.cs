namespace RCRCoreLib.Core.Systems
{
    public class FeatureLockedNode
    {
        public int ID { get; protected set; }
        public int UnlockedAtLevel { get; protected set; }
        public int[] DependantOn { get; protected set; }
    }
}