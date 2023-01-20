using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Systems.PersistantData
{
    public interface IPersistantData
    {
        string FileLocation { get; set; }
        void Save(JsonWriter writer);
        void Load(JsonReader reader);//OverWrite Current Instance

        Task LoadAsync(JsonReader reader);
        Task SaveAsync(JsonWriter writer);

        void On_FailedLoad();

    }
}