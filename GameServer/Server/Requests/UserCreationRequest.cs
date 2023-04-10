using Server.Models;

namespace Server.Requests;

public class UserCreationRequest : BaseRequest
{
    public User User { get; set; }

}