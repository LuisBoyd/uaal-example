using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public class JSONReader : SerializationReader<IJsonSerializable>, IDisposable
    {
        private Stream m_stream;
        private JsonTextReader m_reader;
        private string Contents;
        public JSONReader(Stream stream)
        {
            m_stream = stream;
            using (StreamReader Sr = new StreamReader(stream, Encoding.UTF8))
            { 
                Contents = Sr.ReadToEnd();
            }
            //COULD CLOSE THE STREAM HERE IF NEED TO
            m_reader = new JsonTextReader(new StringReader(Contents));
            m_reader.Read();
        }
        
        public override void Read(IJsonSerializable serializable)
        {
            serializable.ReciveObjectData(m_reader);
        }

        public void Dispose()
        {
            m_stream?.Dispose();
            ((IDisposable)m_reader)?.Dispose();
            Contents = null;
            m_reader = null;
            m_stream = null;
        }
    }
}