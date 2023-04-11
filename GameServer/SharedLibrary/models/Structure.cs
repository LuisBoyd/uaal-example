

namespace SharedLibrary.models;
[Serializable]
public class Structure
{
    public int Id { get; set; }
    public int Structure_Type { get; set; }
    public int OwningPlotId { get; set; }
    public byte X { get; set; }
    public byte Y { get; set; }
}