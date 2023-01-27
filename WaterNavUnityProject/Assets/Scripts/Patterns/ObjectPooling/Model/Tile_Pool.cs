using System.Collections.Generic;
using NewScripts.Model;
using Patterns.Factory;
using Patterns.Factory.Model;

namespace Patterns.ObjectPooling.Model
{
    public class Tile_Pool: IPool<Tile>
    {
        #region properties
        public IFactory<Tile> Factory { get; set; }
        private bool HasBeenPreWarmed { get; set; }
        #endregion

        #region varibles
        private readonly Stack<Tile> Avalible;
        #endregion

        #region Constructor
        public Tile_Pool(Chunk chunk) : base()
        {
            Factory = new Tile_Factory(chunk);
            Avalible = new Stack<Tile>();
            HasBeenPreWarmed = false;
        }
        #endregion


        public void PreWarm(int num)
        {
            if (!HasBeenPreWarmed)
            {
                for (int i = 0; i < num; i++)
                {
                    Avalible.Push(Factory.Create());
                }

                HasBeenPreWarmed = true;
            }
        }

        public Tile Request()
        {
            return Avalible.Count > 0 ? Avalible.Pop() : Factory.Create();
        }

        public void Return(Tile member)
        {
            Avalible.Push(member);
        }
    }
}