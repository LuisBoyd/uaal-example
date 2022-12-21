using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using WaterNavTiled.Data;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public class JSONWriter : SerializationWriter<IJsonSerializable>, IDisposable
    {
        private Stream m_stream;
        private JsonWriter m_writer;
        public JSONWriter(Stream stream)
        {
            m_stream = stream;
            m_writer = new JsonTextWriter(new StreamWriter(stream));
            m_writer.Formatting = Formatting.Indented;
            m_writer.WriteStartObject();
        }
        
        
        /// <summary>
        /// Writes to Stream, Owner should be responsible for closing the stream afterwards
        /// </summary>
        /// <param name="serializable"></param>
        /// <param name="stream"></param>
        public override void Write(IJsonSerializable serializable, bool autoStartEnd = false)
        {
            if(autoStartEnd)
                m_writer.WriteStartObject();
            serializable.GetObjectData(m_writer);
            if(autoStartEnd)
                m_writer.WriteEndObject();
        }

        public override void Write(IEnumerable<IJsonSerializable> serializables, bool autoStartEnd = false)
        {
            if(autoStartEnd)
                m_writer.WriteStartObject();
            foreach (IJsonSerializable serializable in serializables)
            {
                serializable.GetObjectData(m_writer);
            }
            if(autoStartEnd)
                m_writer.WriteStartObject();
        }


        public void Dispose()
        {
            m_writer.WriteEndObject();
            m_writer.Flush();
            m_stream?.Dispose();
            //((IDisposable)m_writer)?.Dispose();
            m_stream = null;
            m_writer = null;
        }
    }
}