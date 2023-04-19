using JetBrains.Annotations;

namespace DefaultNamespace.Core.models
{
    public class UserMariana
    {
        public int Id { get; set; } //Also add details about what the marina is but could just pull that from other DB
        public int marinaId { get; set; }
        public int system_user_id { get; set; }
        public string user_username { get; set; }
        public int AverageEarningPerSecond { get; set; }
    }
}