using JetBrains.Annotations;

namespace DefaultNamespace.Core.models
{
    public class User
    {
        public int User_id { get; set; }
        [CanBeNull] public string Username { get; set; }
        public int Level { get; set; }
        public int Current_Exp { get; set; }
        public int Freemium_Currency { get; set; }
        public int Premium_Currency { get; set; }
    }
}