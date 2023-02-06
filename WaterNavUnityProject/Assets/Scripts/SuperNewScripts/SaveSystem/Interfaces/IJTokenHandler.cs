using Newtonsoft.Json;

namespace RCR.Settings.SuperNewScripts.SaveSystem.Interfaces
{
    public interface IJTokenHandler<T>
    {
        void Handle_Token_None(JsonTextReader reader, ref T value);
        void Handle_Token_StartObject(JsonTextReader reader, ref T value);
        void Handle_Token_StartArray(JsonTextReader reader, ref T value);
        void Handle_Token_StartConstructor(JsonTextReader reader, ref T value);
        void Handle_Token_PropertyName(JsonTextReader reader, ref T value);
        void Handle_Token_Comment(JsonTextReader reader, ref T value);
        void Handle_Token_Raw(JsonTextReader reader, ref T value);
        void Handle_Token_Integer(JsonTextReader reader, ref T value);
        void Handle_Token_Float(JsonTextReader reader, ref T value);
        void Handle_Token_String(JsonTextReader reader, ref T value);
        void Handle_Token_Boolean(JsonTextReader reader, ref T value);
        void Handle_Token_Null(JsonTextReader reader, ref T value);
        void Handle_Token_Undefined(JsonTextReader reader, ref T value);
        void Handle_Token_EndObject(JsonTextReader reader, ref T value);
        void Handle_Token_EndArray(JsonTextReader reader, ref T value);
        void Handle_Token_EndConstructor(JsonTextReader reader, ref T value);
        void Handle_Token_Date(JsonTextReader reader, ref T value);
        void Handle_Token_Bytes(JsonTextReader reader, ref T value);
    }
}