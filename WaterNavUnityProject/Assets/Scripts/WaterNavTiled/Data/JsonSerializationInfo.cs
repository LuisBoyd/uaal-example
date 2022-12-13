using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaterNavTiled.Data
{
    public class JsonSerializationInfo : IDisposable
    {
        private Dictionary<string, Tuple<object, Type>> m_store;

        public int MemberCount
        {
            get => m_store.Count;
        }

        private readonly Type m_objectType;

        public Type ObjectType
        {
            get => m_objectType;
        }

        private readonly string m_assemblyName;

        public string AssemblyName
        {
            get => m_assemblyName;
        }

        public string FullTypeName
        {
            get => m_objectType.FullName;
        }

        public JsonSerializationInfo(object obj)
        {
            m_objectType = obj.GetType();
            m_store = new Dictionary<string, Tuple<object, Type>>();
        }
        public void Dispose()
        {
            m_store.Clear();
            m_store = null;
        }

        public bool AddValue<T>(string identifier, T obj)
        {
            try
            {
                m_store.Add(identifier, new Tuple<object, Type>(obj, obj.GetType()));
            }
            catch (ArgumentNullException argumentNullException)
            {
                Debug.LogError($"{argumentNullException.Message} \n" +
                               $"{argumentNullException.StackTrace}");
                return false;
            }
            catch (ArgumentException argumentException)
            {
                Debug.LogError($"{argumentException.Message} \n" +
                               $"{argumentException.StackTrace}");
                return false;
            }
            return true;
        }

        public T ReadValue<T>(string identifier, Type type)
        {
            //TODO Implement
            T value = default;
            return value;
        }
        
        public byte Readbyte(string identifier)
        {
            //TODO Implement
            byte value = 0;
            return value;
        }
        public sbyte Readsbyte(string identifier)
        {
            //TODO Implement
            sbyte value = 0;
            return value;
        }
        public Int16 ReadInt16(string identifier)
        {
            //TODO Implement
            Int16 value = 0;
            return value;
        }
        public UInt16 ReadUInt16(string identifier)
        {
            //TODO Implement
            UInt16 value = 0;
            return value;
        }
        public Int32 ReadInt32(string identifier)
        {
            //TODO Implement
            Int32 value = 0;
            return value;
        }
        public UInt32 ReadUInt32(string identifier)
        {
            //TODO Implement
            UInt32 value = 0;
            return value;
        }
        public Int64 ReadInt64(string identifier)
        {
            //TODO Implement
            Int64 value = 0;
            return value;
        }
        public UInt64 ReadUInt64(string identifier)
        {
            //TODO Implement
            UInt64 value = 0;
            return value;
        }
        public Single ReadSingle(string identifier)
        {
            //TODO Implement
            Single value = 0;
            return value;
        }
        public double Readdouble(string identifier)
        {
            //TODO Implement
            double value = 0;
            return value;
        }
    }
}