using SharedLibrary.models;

namespace Server.Requests;

public class LoginRequest : BaseRequest
{
    public JwToken JwTtoken { get; set; }

}