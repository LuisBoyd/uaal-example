namespace SharedLibrary.models;

public class UserInfo
{
    public string Username { get; set; }
    public int Level { get; set; } //Foreign Key
    public string Role { get; set; }
    public int Current_Exp { get; set; }
    
    public int Max_Allowed_Marina_Count { get; set; }
    public int Freemium_Currency { get; set; }
    public int Premium_Currency { get; set; }
}