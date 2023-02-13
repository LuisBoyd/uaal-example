using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCRCoreLibrary;

namespace RCRMapLibrary.JsonConverters
{
    public class TileLayerConverter : JsonConverter<TileLayer>
    {
        public override void WriteJson(JsonWriter writer, TileLayer? value, JsonSerializer serializer)
        {
            if(value == null)
                return;
            
            writer.WritePropertyName(nameof(value.Name));
            writer.WriteValue(value.Name);
            
            writer.WritePropertyName(nameof(value.Immutable));
            writer.WriteValue(value.Immutable);
            
            writer.WritePropertyName(nameof(value.Properties));
            writer.WriteStartArray();
            foreach (CustomProperty property in value.Properties)
            {
                JToken token = JToken.FromObject(property);
                token.WriteTo(writer);
            }
            writer.WriteEndArray();

            writer.WritePropertyName(nameof(value.UsedTiles));
            writer.WriteStartArray();
            foreach (TileBase UsedTile in GetUsedTilesInLayer(value))
            {
                JToken t = JToken.FromObject(UsedTile);
                t.WriteTo(writer);
            }
            writer.WritePropertyName(nameof(value.TileArray));
            writer.WriteStartArray();
            foreach (TileBase tileBase in value.TileArray.GetEnumarable())
            {
                writer.WriteValue(tileBase.ID);
            }
            writer.WriteEndArray();
            writer.WriteEndArray();
            

        }

        public override TileLayer? ReadJson(JsonReader reader, Type objectType, TileLayer? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (!hasExistingValue)
                existingValue = Activator.CreateInstance<TileLayer>();
            if (existingValue == null)
                return null;

            while (reader.Read())
            {
                switch (reader.Value)
                {
                    case nameof(existingValue.Name):
                        existingValue.Name = reader.ReadAsString() ?? string.Empty;
                        break;
                    case nameof(existingValue.TileArray):
                        PopulateTilesArray(ref existingValue, ref reader);
                        break;
                    case nameof(existingValue.Properties):
                        JArray CustomPropertyArray = JArray.Load(reader);
                        foreach (JToken token in CustomPropertyArray)
                        {
                            CustomProperty? property = token.ToObject<CustomProperty>();
                            if (property != null)
                                existingValue.Properties.Add(property);
                        }
                        break;
                    case nameof(existingValue.SortingOrder):
                        existingValue.SortingOrder = reader.ReadAsInt32() ?? -1;
                        break;
                    case nameof(existingValue.UsedTiles):
                        ReadUsedTiles(ref existingValue, ref reader);
                        break;
                    default:
                        break;
                }
            }

            return existingValue;
        }

        private void ReadUsedTiles(ref TileLayer layer, ref JsonReader reader)
        {
            reader.Read(); //Read to the Array Token
            JArray array = JArray.Load(reader);
            foreach (JToken token in array)
            {
                TileBase tileBase = token.ToObject<TileBase>() ?? throw new ArgumentNullException("token.ToObject<TileBase>()");
                layer.UsedTiles.Add(tileBase);
            }
        }

        private void PopulateTilesArray(ref TileLayer layer, ref JsonReader reader)
        {
            reader.Read(); //Read to the Array Token
            JArray t = JArray.Load(reader);

            for (int x = 0; x < Layer.LayerWidth; x++)
            {
                for (int y = 0; y < Layer.LayerHeight; y++)
                {
                    int index = t[(x * Layer.LayerHeight) + y].ToObject<int>();
                    TileBase fromUsed = layer.UsedTiles.First(tb => tb.ID == index);
                    if(fromUsed == null)
                        continue;
                    layer.TileArray.SetElement(x,y,fromUsed);
                }
            }
            
        }

        private IEnumerable<TileBase> GetUsedTilesInLayer(TileLayer layer)
        {
            Dictionary<int, TileBase> SortedTileBaseSet = new Dictionary<int, TileBase>();
            foreach (TileBase tileBase in layer.TileArray.GetEnumarable())
            {
                if(!SortedTileBaseSet.ContainsKey(tileBase.ID))
                    SortedTileBaseSet.Add(tileBase.ID, tileBase);
            }
            return SortedTileBaseSet.Values;
        }
    }
}