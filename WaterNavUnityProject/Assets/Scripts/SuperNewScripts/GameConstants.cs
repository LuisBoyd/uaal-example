using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.UnityConverters;
using RCR.Settings.NewScripts.DataStorage.Custom;
using RCR.Settings.SuperNewScripts.SaveSystem.CustomContractResolvers;
using ChunkConverter =  RCR.Settings.SuperNewScripts.SaveSystem.CustomConverters.ChunkConverter;

namespace RCR.Settings.SuperNewScripts
{
    public class GameConstants
    {
        public const int ChunkSize = 128;
        public static event EventHandler<ErrorEventArgs> SerializationError;

        public static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings()
        {
            //Check for Additional Content after check: false
            //ConstructorHandling First try Public Default Constructor, Then Single parameterized Constructor, Finally fall back to Non-public
            //Streaming Context used for interface with .NET Native ISerializable https://stackoverflow.com/questions/26597244/what-is-the-use-of-the-streamingcontext-parameter-in-json-net-serialization-call
            ContractResolver = new NonPublicContractResolver(), //This Contract will Write to NonPublic Setters
            Converters =
              DefaultConverters(), //Custom Converters Unity Types have currently been added to this
            //Set the Culture the default is Invariant Culture
            //Date FormatHandling Default is ISO which looks like this "2012-03-21T05:40Z".
            //DateFormatString controls how a dateTime or DateTimeOffset values are written in json default is "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK".
            //TODO write more descriptions https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm
            Error = SerializationError,
            Formatting = Formatting.Indented,
            //MaxDepth going down the chain of children objects
            //PersevereReferenceHandling https://stackoverflow.com/questions/23453977/what-is-the-difference-between-preservereferenceshandling-and-referenceloophandl
            //ReferenceResolverProvider how to resolve references I made a custom one AddresableAssetRefrenceResolver.cs
            TypeNameHandling = TypeNameHandling.All, //when A property is written the type is also written alongside it.

        };
        
        public const string schemaJson = @"{
  'description': 'A person',
  'type': 'object',
  'properties': {
    'name': {'type': 'string'},
    'hobbies': {
      'type': 'array',
      'items': {'type': 'string'}
    }
  }
}";
        public static JsonSchema Schema = JsonSchema.Parse(schemaJson);
        public const int WorldMaxSize = 5;

        private static IList<JsonConverter> DefaultConverters()
        {
            var converters = JsonConvert.DefaultSettings().Converters;
            converters.Add(new ChunkConverter());

            return converters;
        }
    }
}