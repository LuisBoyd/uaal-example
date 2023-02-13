namespace RCRMapLibrary
{
    public abstract class Layer
    {
        public const int LayerWidth = 128;
        public const int LayerHeight = 128;

        public bool Immutable { get; private set; }

        protected Layer()
        {
        }
        
    }
}