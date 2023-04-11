

namespace SharedLibrary.models;
[Serializable]
public class Plot
{
    public int Id { get; set; }
    public int OwningMarinaId { get; set; }
    public byte[] Tile_Data { get; set; }
    public byte Plot_index { get; set; }
}