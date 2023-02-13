using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RCRMapLibrary.TiledSupport
{
    public class TiledMap
    {
        const uint FLIPPED_HORIZONTALLY_FLAG = 0b10000000000000000000000000000000;
        const uint FLIPPED_VERTICALLY_FLAG = 0b01000000000000000000000000000000;
        const uint FLIPPED_DIAGONALLY_FLAG = 0b00100000000000000000000000000000;
        
           /// <summary>
        /// How many times we shift the FLIPPED flags to the right in order to store it in a byte.
        /// For example: 0b10100000000000000000000000000000 >> SHIFT_FLIP_FLAG_TO_BYTE = 0b00000101
        /// </summary>
        const int SHIFT_FLIP_FLAG_TO_BYTE = 29;

        /// <summary>
        /// Returns the Tiled version used to create this map
        /// </summary>
        public string TiledVersion { get; set; }

        /// <summary>
        /// Returns an array of properties defined in the map
        /// </summary>
        public TiledProperty[] Properties { get; set; }

        /// <summary>
        /// Returns an array of tileset definitions in the map
        /// </summary>
        public TiledMapTileset[] Tilesets { get; set; }

        /// <summary>
        /// Returns an array of layers or null if none were defined
        /// </summary>
        public TiledLayer[] Layers { get; set; }

        /// <summary>
        /// Returns an array of groups or null if none were defined
        /// </summary>
        public TiledGroup[] Groups { get; set; }

        /// <summary>
        /// Returns the defined map orientation as a string
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Returns the render order as a string
        /// </summary>
        public string RenderOrder { get; set; }

        /// <summary>
        /// The amount of horizontal tiles
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The amount of vertical tiles
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The tile width in pixels
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The tile height in pixels
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// The parallax origin x
        /// </summary>
        public float ParallaxOriginX { get; set; }

        /// <summary>
        /// The parallax origin y
        /// </summary>
        public float ParallaxOriginY { get; set; }

        /// <summary>
        /// Returns true if the map is configured as infinite
        /// </summary>
        public bool Infinite { get; set; }

        /// <summary>
        /// Returns the defined map background color as a hex string
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Returns an empty instance of TiledMap
        /// </summary>
        public TiledMap()
        {
        }
        
        // <summary>
        /// Loads a Tiled map in TMX format and parses it
        /// </summary>
        /// <param name="path">The path to the tmx file</param>
        /// <exception cref="TiledException">Thrown when the map could not be loaded or is not in a correct format</exception>
        public TiledMap(string path)
        {
            // Check the file
            if (!File.Exists(path))
            {
                throw new TiledException($"{path} not found");
            }

            var content = File.ReadAllText(path);

            if (path.EndsWith(".json"))
            {
                ParseJson(content);
            }
            else
            {
                throw new TiledException("Unsupported file format");
            }
        }
        
        /// <summary>
        /// Loads a Tiled map in TMX format and parses it
        /// </summary>
        /// <param name="stream">Stream of opened tmx file</param>
        /// <exception cref="TiledException">Thrown when the map could not be loaded</exception>
        public TiledMap(JsonReader reader)
        {
            ParseJson(reader.ToString());
        }
        
        public void ParseJson(string json)
        {
            try
            {
                JToken document = JToken.Parse(json);

                JToken map = document["map"];
                JArray Properties = JArray.Parse(document["properties"].ToString());
                JArray Layers = JArray.Parse(document["layers"].ToString());
                
            }
            catch (Exception e)
            {
                throw new TiledException("An error occurred while trying to parse the Tiled map file", e);
            }
        }
    }
}