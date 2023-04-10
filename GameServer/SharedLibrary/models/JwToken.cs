namespace SharedLibrary.models;

public class JwToken
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    
    public string AccessTokenExpiry { get; set; }
    public string Message { get; set; }

    public static JwToken Empty = new JwToken();
}