using Sirenix.Utilities;
using Sirenix.OdinInspector;
namespace RCRCoreLib.Core
{
    public abstract class JsonGlobalConfig <T> : GlobalConfig<T> where T : GlobalConfig<T>, new()
    {
        public abstract void SerializeConfig();
        public abstract void DeserializeConfig();
        
    }
}