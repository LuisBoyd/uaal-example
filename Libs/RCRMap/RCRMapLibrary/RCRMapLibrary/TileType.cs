namespace RCRMapLibrary
{
    public class TileType
    {
        public string Name { get; set; }
        public string Source { get; set; }

        public TileType(string name)
        {
            this.Name = name;
        }
        
    }
}