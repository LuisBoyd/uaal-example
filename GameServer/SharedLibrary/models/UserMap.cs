

namespace SharedLibrary.models;

public class UserMap
{

    public int Id { get; set; }
    
    public int MarinaID { get; set; } //foreignKey
    
    public string OwnerID { get; set; } //foreignKey
}