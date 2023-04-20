using JetBrains.Annotations;

namespace DefaultNamespace.Core.models
{
    public class User
    {
        public int User_id { get; set; } = 7; //Default value for testing TODO take out in production
        [CanBeNull] public string Username { get; set; }
        public int Level { get; set; }
        public int Current_Exp { get; set; }
        public int Freemium_Currency { get; set; }
        public int Premium_Currency { get; set; }
    }
}