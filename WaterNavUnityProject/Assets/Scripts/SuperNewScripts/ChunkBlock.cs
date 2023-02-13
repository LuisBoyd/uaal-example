using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RCR.Settings.NewScripts.Geometry;
using UnityEngine;

namespace RCR.Settings.SuperNewScripts
{
    public class ChunkBlock
    {
        
        public struct Layer
        {
            public int[] data { get; private set; } //For Uncompressed
            public char[] dataBase64 { get; private set; } //For Compressed
            public int Height { get; private set; }
            public int id { get; private set; }
            public string name { get; private set; }
            public float opacity { get; private set; }
            public string Type { get; private set; }
            public bool visible { get; private set; }
            public int Width { get; private set; }
            public int x { get; private set; }
            public int y { get; private set; }
            
            public void SetData(int[] data) => this.data = data;
            public void SetdataBase64(char[] data) => this.dataBase64 = data;
            public void SetHeight(int height) => this.Height = height;
            public void Setid(int id) => this.id = id;
            public void Setname(string name) => this.name = name;
            public void Setopacity(float opacity) => this.opacity = opacity;
            public void SetType(string Type) => this.Type = Type;
            public void Setvisible(bool visible) => this.visible = visible;
            public void SetWidth(int Width) => this.Width = Width;
            public void SetX(int x) => this.x = x;
            public void SetY(int y) => this.y = y;
        }
        
        public class Tileset
        {
            public Tileset()
            {
                firstid = 0;
                source = null;
                tiles = new Tiles[] { };
                addressableLookup = new Dictionary<int, string>();
            }
            
            public int firstid { get; private set; }
            public string source { get; private set; }
            
            public Tiles[] tiles { get; private set; }
            
            public wangset[] wangsets { get; private set; }
            
            public Dictionary<int, string> addressableLookup { get; private set; }
            

            public void SetFirstId(int id) => this.firstid = id;
            public void SetSource(string source) => this.source = source;

            public void SetaddressableLookup(Dictionary<int, string> newLookUp) => addressableLookup = newLookUp;
            public void SetTiles(Tiles[] tilesArray) => tiles = tilesArray;

            public void SetWangSets(wangset[] sets) => this.wangsets = sets;
            
            public struct Tiles
            {
                public int id { get; private set; }
                public Property[] Properties { get; private set; }
                public void Setid(int id) => this.id = id;
                public void setProperties(Property[] properties) => this.Properties = properties;
            }
            
        }
        
        public class wangset
        {
            public class WangColor
            {
                public Color color { get; private set; }
                public string name { get; private set; }
                public float probability { get; private set; }
                public int tile { get; private set; }
                
                public void Setcolor(Color color) => this.color = color;
                public void Setcolor(string color)
                {
                    color = color.Remove(0, 1);
                    var Rhex = color.Substring(0, 2);
                    var Ghex = color.Substring(2, 2);
                    var Bhex = color.Substring(4, 6);

                    this.color = new Color(
                        Mathf.Clamp01(Convert.ToInt32(Rhex, 16)),
                        Mathf.Clamp01(Convert.ToInt32(Ghex, 16)),
                        Mathf.Clamp01(Convert.ToInt32(Bhex, 16))
                    );
                }
                public void Setname(string name) => this.name = name;
                public void SetProbabilty(float probability) => this.probability = probability;
                public void Settile(int tileid) => this.tile = tileid;
            }
            
            public class WangTile
            {
                public int tileid { get; private set; }
                public int[] wangId { get; private set; }

                public void SetTileID(int id) => tileid = id;
                public void SetWangID(int[] ids)
                {
                    bool LessThan = false;
                    if (ids.Length > 8)
                    {
                        Debug.LogWarning("Wang Tiles can only have Max entries of 8 ignoring the overflow");
                     
                    }
                    if (ids.Length < 8)
                    {
                        Debug.LogWarning("Wang Tiles should be 8 entries filling remaining entries with default values");
                        LessThan = true;
                    }
                    int[] entries = new int[8];
                    if (LessThan)
                    {
                        for (int i = entries.Length; i > ids.Length; i++)
                        {
                            entries[i] = 0;
                        }
                        for (int i = 0; i < ids.Length; i++)
                        {
                            entries[i] = ids[i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < entries.Length; i++)
                        {
                            entries[i] = ids[i];
                        } 
                    }

                    wangId = entries;

                }
            }

            public WangColor[] Colors { get; private set; }
            public string name { get; private set; }
            public int tile { get; private set; }
            public string type { get; private set; }
            public WangTile[] WangTiles { get; private set; }

            public void SetWangColors(WangColor[] colors) => this.Colors = colors;
            public void SetWangName(string name) => this.name = name;
            public void SetWangTile(int tileid) => this.tile = tileid;
            public void SetWangType(string type) => this.type = type;
            public void SetWangTiles(WangTile[] tiles) => this.WangTiles = tiles;
            public int[] ReturnWangTilesIDs()
            {
                List<int> wangTilesIDs = new List<int>();
                foreach (WangTile wangTile in WangTiles)
                {
                    wangTilesIDs.Add(wangTile.tileid);
                }

                return wangTilesIDs.ToArray();
            }
        }
        
        public struct Property
        {
            public string name { get; private set; }
            public string type { get; private set; }
            public object value { get; private set; }

            public void setName(string name) => this.name = name;
            public void setType(string type) => this.type = type;
            public void setValue(object value) => this.value = value;
        }

        
        #region Save/Load Properties
        /// <summary>
        /// Tiles Stores all the Datatiles inside this Chunk
        /// </summary>
        public DataTile[,] Tiles;
        public Vector2Int Origin { get; private set; }
        public bool Active { get; private set; }
        
        //--------
        public string fileName { get; private set; }
        public int compressionlevel { get; private set; }
        public int height { get; private set; }
        public bool infinite { get; private set; }
        public Layer[] layers { get; private set; }
        public int nextlayerid { get; private set; }
        public int nextobjectid { get; private set; }
        
        public string orientation { get; private set; }
        public string renderorder { get; private set; }
        public string tiledversion { get; private set; }
        //Orentation should always be orthogonal
        //render order not matters much but always left-up
        public int tileheight { get; private set; }
        public int tilewidth { get; private set; }
        public Tileset[] tilesets { get; private set; }
        public string type { get; private set; }
        public string version { get; private set; }
        public int width { get; private set; }
        
        public int X { get; private set; }
        public int Y { get; private set; }
        
        //TODO addressable

        #endregion

        //No Need for Size as a const is set for the chunks size;

        public ChunkBlock()
        {
            fileName = null;
            Tiles = new DataTile[,] { };
            Origin = default;
            Active = false;
            compressionlevel = 0;
            height = 0;
            infinite = false;
            layers = new Layer[] { };
            nextlayerid = 0;
            nextobjectid = 0;
            orientation = null;
            renderorder = null;
            tiledversion = null;
            tileheight = 0;
            tilewidth = 0;
            tilesets = new Tileset[] { };
            type = null;
            version = null;
            width = 0;
            X = 0;
            Y = 0;
        }
        
        public void Setversion(string version) => this.version = version;
        public void Settiledversion(string tiledVersion) => this.tiledversion = tiledversion;
        public void Setrenderorder(string renderorder) => this.renderorder = renderorder;
        public void Setorientation(string orientation) => this.orientation = orientation;
        public void SetCompressionLevel(int level) => compressionlevel = level;
        public void setHeight(int height) => this.height = height;
        public void setInfinite(bool Isinfinte) => this.infinite = Isinfinte;
        public void setLayers(Layer[] layers) => this.layers = layers;
        public void setLayer(Layer layer, int index) => this.layers[index] = layer;
        public void setNextLayerID(int id) => nextlayerid = id;
        public void setNextObjectID(int id) => nextobjectid = id;
        public void setTileHeight(int height) => tileheight = height;
        public void setTileWidth(int width) => tilewidth = width;

        public void SetTileSets(Tileset[] tilesets) => this.tilesets = tilesets;
        public void setLayer(Tileset tileset, int index) => this.tilesets[index] = tileset;
        
        public void setType(string type) => this.type = type;
        public void Setwidth(int width) => this.width = width;

        public void SetFileName(string name) => this.fileName = name;
        public void SetX(int x) => this.X = x;
        public void SetY(int y) => this.Y = y;

        public Line[] GetEdges() => new Line[4]
        {
            new Line(new Vector2(X, Y), new Vector2(X, Y + height)),
            new Line(new Vector2(X, Y + height), new Vector2(X + width, Y + height)),
            new Line(new Vector2(X + width, Y + height), new Vector2(X + width, Y)),
            new Line(new Vector2(X + width, Y), new Vector2(X, Y))
        };

    }
}