
using JetBrains.Annotations;

public class UserloginResponse
{
    public int user_id { get; set; }
    [CanBeNull] public string username { get; set; }
    public int level { get; set; }
    public int current_exp { get; set; }
    public int taken_slots { get; set; }
    public int free_slots { get; set; }
    public int freemium_currency { get; set; }
    public int premium_currency { get; set; }
}