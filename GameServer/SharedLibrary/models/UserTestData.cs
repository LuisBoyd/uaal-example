

namespace SharedLibrary.models;

public class UserTestData
{
    /*
     * Example of a model each of the fields in the MVC architecture (all though here it's restful api
     * so really we only have Model and controllers)
     * represent a column in a table while the model represents a Entity/Table
     * using the EntityFrameworkCore.MySql we can build migrations
     * cmd > dotnet ef migrations add {Name of Migration (can be anything like a git commit)}
     * to push this to our database we require that to be set up I have detailed that in the sever files.
     * but the command for that is
     * cmd > dotnet ef database update.
     */
    
    public int Id { get; set; }
    public string Username { get; set; } //Foreign Key
    public string PasswrodHash { get; set; }
    public string Salt { get; set; }
    
    public string Role { get; set; } //Foreign Key
    public int Level { get; set; } //Foreign Key
    
    public int Current_Exp { get; set; }
    
    public int Max_Allowed_Marina_Count { get; set; }
    public int Freemium_Currency { get; set; }
    public int Premium_Currency { get; set; }
    
    
}