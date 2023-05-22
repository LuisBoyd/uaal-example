namespace CustomUIFramework.Animation.AnimationProperties
{
    public interface IAnimationProperty
    {
        public float TimeScale { get; }
        public LeanTweenType EaseType { get; }
    }
}