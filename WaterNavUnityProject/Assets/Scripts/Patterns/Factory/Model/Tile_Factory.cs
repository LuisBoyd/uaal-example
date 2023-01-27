using NewScripts.Model;

namespace Patterns.Factory.Model
{
    public class Tile_Factory: IFactory<Tile>
    {

        private Chunk chunk;
        
        public Tile_Factory(Chunk chunk) : base()
        {
            this.chunk = chunk;
        }

        public Tile Create()
        {
            return new Tile(chunk);
        }

        public Tile Clone(Tile original)
        {
            throw new System.NotImplementedException();
        }
    }
}