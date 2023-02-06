using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RCR.Settings.SuperNewScripts.SaveSystem.CustomContractResolvers
{
    public class NonPublicContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            prop.Writable = true;

            return prop;
        }
    }
    
    public class NonPublicContractResolver<T> : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            if(prop.DeclaringType == typeof(T))
                prop.Writable = true;

            return prop;
        }
    }
}