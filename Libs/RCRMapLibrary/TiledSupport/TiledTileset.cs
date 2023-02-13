using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCRMapLibrary.TiledSupport.Enums;
using RCRMapLibrary.TiledSupport.TiledShapes;

namespace RCRMapLibrary.TiledSupport
{
    public class TiledTileset
    {
        /// <summary>
        /// The Tiled version used to create this tileset
        /// </summary>
        public string TiledVersion { get; set; }
        /// <summary>
        /// The tileset name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The tileset class
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// The tile width in pixels
        /// </summary>
        public int TileWidth { get; set; }
        /// <summary>
        /// The tile height in pixels
        /// </summary>
        public int TileHeight { get; set; }
        /// <summary>
        /// The total amount of tiles
        /// </summary>
        public int TileCount { get; set; }
        /// <summary>
        /// The amount of horizontal tiles
        /// </summary>
        public int Columns { get; set; }
        /// <summary>
        /// The image definition used by the tileset
        /// </summary>
        public TiledImage Image { get; set; }
        /// <summary>
        /// The amount of spacing between the tiles in pixels
        /// </summary>
        public int Spacing { get; set; }
        /// <summary>
        /// The amount of margin between the tiles in pixels
        /// </summary>
        public int Margin { get; set; }
        /// <summary>
        /// An array of tile definitions
        /// </summary>
        /// <remarks>Not all tiles within a tileset have definitions. Only those with properties, animations, terrains, ...</remarks>
        public TiledTile[] Tiles { get; set; }
        /// <summary>
        /// An array of tileset properties
        /// </summary>
        public TiledProperty[] Properties { get; set; }
        
        /// <summary>
        /// The tile offset in pixels
        /// </summary>
        public TiledOffset Offset { get; set; }

        /// <summary>
        /// Returns an empty instance of TiledTileset
        /// </summary>
        public TiledTileset()
        {

        }

        /// <summary>
        /// Loads a tileset in TSX format and parses it
        /// </summary>
        /// <param name="path">The file path of the TSX file</param>
        /// <exception cref="TiledException">Thrown when the file could not be found or parsed</exception>
        public TiledTileset(string path)
        {
            // Check the file
            if (!File.Exists(path))
            {
                throw new TiledException($"{path} not found");
            }
            
            var content = File.ReadAllText(path);

            if (path.EndsWith(".tsx"))
            {
                ParseXml(content);
            }
            else if (path.EndsWith(".json"))
            {
                ParseJson(content);
            }
            else
            {
                throw new TiledException("Unsupported file format");
            }
        }

        /// <summary>
        /// Loads a tileset in TSX format and parses it
        /// </summary>
        /// <param name="stream">The file stream of the TSX file</param>
        /// <exception cref="TiledException">Thrown when the file could not be parsed</exception>
        public TiledTileset(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            var content = streamReader.ReadToEnd();
            ParseXml(content);
        }

        public TiledTileset(JsonReader jsonReader)
        {
            ParseJson(jsonReader.ToString());
        }

        /// <summary>
        /// Can be used to parse the content of a TSX tileset manually instead of loading it using the constructor
        /// </summary>
        /// <param name="xml">The tmx file content as string</param>
        /// <exception cref="TiledException"></exception>
        public void ParseXml(string xml)
        {
            try
            {
                var document = new XmlDocument();
                document.LoadXml(xml);

                var nodeTileset = document.SelectSingleNode("tileset");
                var nodeImage = nodeTileset.SelectSingleNode("image");
                var nodeOffset = nodeTileset.SelectSingleNode("tileoffset");
                var nodesTile = nodeTileset.SelectNodes("tile");
                var nodesProperty = nodeTileset.SelectNodes("properties/property");

                var attrMargin = nodeTileset.Attributes["margin"];
                var attrSpacing = nodeTileset.Attributes["spacing"];
                var attrClass = nodeTileset.Attributes["class"];

                TiledVersion = nodeTileset.Attributes["tiledversion"].Value;
                Name = nodeTileset.Attributes["name"]?.Value;
                TileWidth = int.Parse(nodeTileset.Attributes["tilewidth"].Value);
                TileHeight = int.Parse(nodeTileset.Attributes["tileheight"].Value);
                TileCount = int.Parse(nodeTileset.Attributes["tilecount"].Value);
                Columns = int.Parse(nodeTileset.Attributes["columns"].Value);

                if (attrMargin != null) Margin = int.Parse(nodeTileset.Attributes["margin"].Value);
                if (attrSpacing != null) Spacing = int.Parse(nodeTileset.Attributes["spacing"].Value);
                if (attrClass != null) Class = attrClass.Value;
                if (nodeImage != null) Image = ParseImage(nodeImage);
                if (nodeOffset != null) Offset = ParseOffset(nodeOffset);

                Tiles = ParseTiles(nodesTile);
                Properties = ParseProperties(nodesProperty);
            }
            catch (Exception ex)
            {
                throw new TiledException("An error occurred while trying to parse the Tiled tileset file", ex);
            }
        }

        public void ParseJson(string json)
        {
            try
            {
                JToken TilesetToken = JToken.Parse(json);
                
                JToken attrMargin = TilesetToken["margin"];
                JToken attrSpacing = TilesetToken["spacing"];
                JToken attrClass = TilesetToken["class"];
                JToken attrImage = TilesetToken["image"];
                JToken attrTileOffset = TilesetToken["tileoffset"];
                JArray attrTiles = JArray.Parse(TilesetToken["tiles"].ToString());
                JArray attrPropertys = JArray.Parse(TilesetToken["properties"].ToString());

                TiledVersion = TilesetToken["tiledversion"].ToString();
                Name = TilesetToken["name"].ToString();
                TileWidth = TilesetToken["tilewidth"].ToObject<int>();
                TileHeight = TilesetToken["tileheight"].ToObject<int>();
                TileCount = TilesetToken["tilecount"].ToObject<int>();
                Columns = TilesetToken["columns"].ToObject<int>();

                if (attrSpacing != null) Spacing = attrSpacing.ToObject<int>();
                if (attrMargin != null) Margin = attrMargin.ToObject<int>();
                if (attrClass != null) Class = attrClass.ToString();
                if (attrImage != null) Image = ParseImage(attrImage);
                if(attrTileOffset != null) Offset = ParseOffset(attrTileOffset);

                Tiles = ParseTiles(attrTiles);
                Properties = ParseProperties(attrPropertys);
            }
            catch (Exception ex)
            {
                throw new TiledException("An error occured while trying to parse the Tiled tileset file", ex);
            }
        }

        private TiledOffset ParseOffset(XmlNode node)
        {
            var tiledOffset = new TiledOffset();
            tiledOffset.x = int.Parse(node.Attributes["x"].Value);
            tiledOffset.y = int.Parse(node.Attributes["y"].Value);

            return tiledOffset;
        }

        private TiledOffset ParseOffset(JToken token)
        {
            TiledOffset offset = new TiledOffset();
            offset.x = token["x"].ToObject<int>();
            offset.y = token["y"].ToObject<int>();
            return offset;
        }

        private TiledImage ParseImage(XmlNode node)
        {
            var tiledImage = new TiledImage();
            tiledImage.source = node.Attributes["source"].Value;
            tiledImage.width = int.Parse(node.Attributes["width"].Value);
            tiledImage.height = int.Parse(node.Attributes["height"].Value);

            return tiledImage;
        }

        private TiledImage ParseImage(JToken token)
        {
            TiledImage image = new TiledImage();
            image.source = token["source"].ToString();
            image.width = token["width"].ToObject<int>();
            image.height = token["height"].ToObject<int>();
            return image;
        }

        private TiledTileAnimation[] ParseAnimations(JArray animationsList)
        {
            List<TiledTileAnimation> result = new List<TiledTileAnimation>();
            foreach (JToken token in animationsList)
            {
                TiledTileAnimation animation = new TiledTileAnimation();
                animation.tileid = token["tileid"].ToObject<int>();
                animation.duration = token["duration"].ToObject<int>();
                
                result.Add(animation);
            }

            return result.ToArray();
        }
        
        private TiledTileAnimation[] ParseAnimations(XmlNodeList nodeList)
        {
            var result = new List<TiledTileAnimation>();

            foreach (XmlNode node in nodeList)
            {
                var animation = new TiledTileAnimation();
                animation.tileid = int.Parse(node.Attributes["tileid"].Value);
                animation.duration = int.Parse(node.Attributes["duration"].Value);
                
                result.Add(animation);
            }
            
            return result.ToArray();
        }

        private TiledProperty[] ParseProperties(XmlNodeList nodeList)
        {
            var result = new List<TiledProperty>();

            foreach (XmlNode node in nodeList)
            {
                var attrType = node.Attributes["type"];

                var property = new TiledProperty();
                property.name = node.Attributes["name"].Value;
                property.value = node.Attributes["value"]?.Value;
                property.type = TiledPropertyType.String;

                if (attrType != null)
                {
                    if (attrType.Value == "bool") property.type = TiledPropertyType.Bool;
                    if (attrType.Value == "color") property.type = TiledPropertyType.Color;
                    if (attrType.Value == "file") property.type = TiledPropertyType.File;
                    if (attrType.Value == "float") property.type = TiledPropertyType.Float;
                    if (attrType.Value == "int") property.type = TiledPropertyType.Int;
                    if (attrType.Value == "object") property.type = TiledPropertyType.Object;
                }

                if (property.value == null)
                {
                    property.value = node.InnerText;
                }

                result.Add(property);
            }

            return result.ToArray();
        }

        private TiledProperty[] ParseProperties(JArray propertyList)
        {
            List<TiledProperty> result = new List<TiledProperty>();
            foreach (JToken token in propertyList)
            {
                JToken attrtype = token["type"];

                TiledProperty property = new TiledProperty();
                property.name = token["name"].ToString();
                property.value = token["value"].ToString();
                property.type = TiledPropertyType.String;

                if (attrtype != null)
                {
                    switch (attrtype.ToString())
                    {
                        case "bool":
                            property.type = TiledPropertyType.Bool;
                            break;
                        case "color":
                            property.type = TiledPropertyType.Color;
                            break;
                        case "file":
                            property.type = TiledPropertyType.File;
                            break;
                        case "float":
                            property.type = TiledPropertyType.Float;
                            break;
                        case "int":
                            property.type = TiledPropertyType.Int;
                            break;
                        case "object":
                            property.type = TiledPropertyType.Object;
                            break;
                    }
                }

                if (property.value == null)
                    property.value = String.Empty;
                
                result.Add(property);
            }

            return result.ToArray();
        }

        private TiledTile[] ParseTiles(XmlNodeList nodeList)
        {
            var result = new List<TiledTile>();

            foreach (XmlNode node in nodeList)
            {
                var nodesProperty = node.SelectNodes("properties/property");
                var nodesObject = node.SelectNodes("objectgroup/object");
                var nodesAnimation = node.SelectNodes("animation/frame");
                var nodeImage = node.SelectSingleNode("image");

                var tile = new TiledTile();
                tile.id = int.Parse(node.Attributes["id"].Value);
                tile.@class = node.Attributes["class"]?.Value;
                tile.type = node.Attributes["type"]?.Value;
                //tile.terrain = node.Attributes["terrain"]?.Value.Split(',');
                tile.properties = ParseProperties(nodesProperty);
                tile.animation = ParseAnimations(nodesAnimation);
                tile.objects = ParseObjects(nodesObject);

                if (nodeImage != null)
                {
                    var tileImage = new TiledImage();
                    tileImage.width = int.Parse(nodeImage.Attributes["width"].Value);
                    tileImage.height = int.Parse(nodeImage.Attributes["height"].Value);
                    tileImage.source = nodeImage.Attributes["source"].Value;

                    tile.image = tileImage;
                }

                result.Add(tile);
            }

            return result.ToArray();
        }

        private TiledTile[] ParseTiles(JArray tilesList)
        {
            List<TiledTile> result = new List<TiledTile>();
            foreach (JToken token in tilesList)
            {
                JArray Properties = JArray.Parse(token["properties"].ToString());
                JArray objects = JArray.Parse(token["objectgroup"].ToString());
                JArray Animations = JArray.Parse(token["animation"].ToString());
                JToken Image = token["image"];

                TiledTile tile = new TiledTile();
                tile.id = token["id"].ToObject<int>();
                tile.@class = token["class"].ToString();
                tile.type = token["type"].ToString();
                JArray terrainArray = JArray.Parse(token["terrain"].ToString());
                tile.terrain = terrainArray.Select(ta => ta.ToObject<int>()).ToArray();
                tile.properties = ParseProperties(Properties);
                tile.animation = ParseAnimations(Animations);
                tile.objects = ParseObjects(objects);

                if (Image != null)
                {
                    TiledImage image = new TiledImage();
                    image.width = Image["width"].ToObject<int>();
                    image.height = Image["height"].ToObject<int>();
                    image.source = Image["source"].ToString();

                    tile.image = image;
                }
                result.Add(tile);
            }

            return result.ToArray();
        }

        private TiledObject[] ParseObjects(JArray objList)
        {
            List<TiledObject> result = new List<TiledObject>();
            foreach (JToken token in objList)
            {
                JArray Properties = JArray.Parse(token["properties"].ToString());
                JToken Polygon = token["polygon"];
                JToken Point = token["point"];
                JToken Ellipse = token["ellipse"];

                TiledObject obj = new TiledObject();
                obj.id = token["id"].ToObject<int>();
                obj.name = token["name"].ToString();
                obj.@class = token["class"].ToString();
                obj.type = token["type"].ToString();
                obj.gid = token["gid"].ToObject<int>();
                obj.x = token["x"].ToObject<float>();
                obj.y = token["y"].ToObject<float>();

                if (Properties != null)
                {
                    obj.properties = ParseProperties(Properties);
                }

                if (Polygon != null)
                {
                    JArray points = JArray.Parse(Polygon.ToString());
                    TiledPolygon tiledPolygon = new TiledPolygon();
                    tiledPolygon.points = new float[points.Count * 2];

                    for (int x = 0; x < points.Count; x++)
                    {
                        tiledPolygon.points[(x * 2) + 0] = points[x].ToObject<float>();
                        tiledPolygon.points[(x * 2) + 1] = points[x+1].ToObject<float>();
                    }

                    obj.polygon = tiledPolygon;
                }

                if (Ellipse != null) obj.ellipse = new TiledEllipse();
                if (Point != null) obj.point = new TiledPoint();

                obj.width = token["width"].ToObject<float>();
                obj.height = token["height"].ToObject<float>();
                obj.rotation = token["rotation"].ToObject<float>();
                result.Add(obj);
            }

            return result.ToArray();
        }
        
        private TiledObject[] ParseObjects(XmlNodeList nodeList)
        {
            var result = new List<TiledObject>();

            foreach (XmlNode node in nodeList)
            {
                var nodesProperty = node.SelectNodes("properties/property");
                var nodePolygon = node.SelectSingleNode("polygon");
                var nodePoint = node.SelectSingleNode("point");
                var nodeEllipse = node.SelectSingleNode("ellipse");

                var obj = new TiledObject();
                obj.id = int.Parse(node.Attributes["id"].Value);
                obj.name = node.Attributes["name"]?.Value;
                obj.@class = node.Attributes["class"]?.Value;
                obj.type = node.Attributes["type"]?.Value;
                obj.gid = int.Parse(node.Attributes["gid"]?.Value ?? "0");
                obj.x = float.Parse(node.Attributes["x"].Value, CultureInfo.InvariantCulture);
                obj.y = float.Parse(node.Attributes["y"].Value, CultureInfo.InvariantCulture);

                if (nodesProperty != null)
                {
                    obj.properties = ParseProperties(nodesProperty);
                }

                if (nodePolygon != null)
                {
                    var points = nodePolygon.Attributes["points"].Value;
                    var vertices = points.Split(' ');

                    var polygon = new TiledPolygon();
                    polygon.points = new float[vertices.Length * 2];

                    for (var i = 0; i < vertices.Length; i++)
                    {
                        polygon.points[(i * 2) + 0] = float.Parse(vertices[i].Split(',')[0], CultureInfo.InvariantCulture);
                        polygon.points[(i * 2) + 1] = float.Parse(vertices[i].Split(',')[1], CultureInfo.InvariantCulture);
                    }

                    obj.polygon = polygon;
                }

                if (nodeEllipse != null)
                {
                    obj.ellipse = new TiledEllipse();
                }

                if (nodePoint != null)
                {
                    obj.point = new TiledPoint();
                }

                if (node.Attributes["width"] != null)
                {
                    obj.width = float.Parse(node.Attributes["width"].Value, CultureInfo.InvariantCulture);
                }

                if (node.Attributes["height"] != null)
                {
                    obj.height = float.Parse(node.Attributes["height"].Value, CultureInfo.InvariantCulture);
                }

                if (node.Attributes["rotation"] != null)
                {
                    obj.rotation = float.Parse(node.Attributes["rotation"].Value, CultureInfo.InvariantCulture);
                }

                result.Add(obj);
            }

            return result.ToArray();
        }
    }
}