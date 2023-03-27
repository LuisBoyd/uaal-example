using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utilities
{
    /// <summary>
    /// BT_Context Is a blackboard, where a node can ask for a specific value that maybe another node has written to the blackboard.
    /// or a node itself can write to the blackboard. there are mainly 2 types of context
    ///
    /// GLOBAL_Context this would be a global accessable to every node.
    /// LOCAL_Context a decorator node would create a new context and pass it down the sub-tree and terminate the context after it's out of scope.
    ///
    /// BT_Context Support's string based lookup however in derived classes you can implement whatever is better, but string is recommended.
    /// BT_Context Support's most value types as well as strings.
    /// </summary>
    [Serializable][CreateAssetMenu(fileName = "New_BlackBoard", menuName = "AI/BT/BlackBoard", order = 0)]
    public class BT_Context : ScriptableObject ,IDisposable
    {
        protected IDictionary<string, bool> _boolLookUp;
        protected IDictionary<string, int> _intLookUp;
        protected IDictionary<string, float> _floatLookUp;
        protected IDictionary<string, byte> _byteLookUp;
        public static BT_Context BTEmpty
        {
            get
            {
                return new BT_Context();
            }
        }
        
        public BT_Context()
        {
            _boolLookUp = new Dictionary<string, bool>();
            _intLookUp = new Dictionary<string, int>();
            _floatLookUp = new Dictionary<string, float>();
            _byteLookUp = new Dictionary<string, byte>();
        }
        

        public void Dispose()
        {
            _boolLookUp.Clear();
            _boolLookUp = null;
            _intLookUp.Clear();
            _intLookUp = null;
            _floatLookUp.Clear();
            _floatLookUp = null;
            _byteLookUp.Clear();
            _byteLookUp = null;
        }

        public bool ReadValueAsBool(string valueName, bool fallbackValue = false)
        {
            if (!_boolLookUp.ContainsKey(valueName))
                return fallbackValue;

            fallbackValue = _boolLookUp[valueName];
            return fallbackValue;
        }
        
        public int ReadValueAsInt(string valueName, int fallbackValue = 0)
        {
            if (!_intLookUp.ContainsKey(valueName))
                return fallbackValue;

            fallbackValue = _intLookUp[valueName];
            return fallbackValue;
        }
        
        public byte ReadValueAsByte(string valueName, byte fallbackValue = 0)
        {
            if (!_byteLookUp.ContainsKey(valueName))
                return fallbackValue;

            fallbackValue = _byteLookUp[valueName];
            return fallbackValue;
        }
        
        public float ReadValueAsFloat(string valueName, float fallbackValue = 0)
        {
            if (!_floatLookUp.ContainsKey(valueName))
                return fallbackValue;

            fallbackValue = _floatLookUp[valueName];
            return fallbackValue;
        }

        public void WriteValue(string valueName, bool value)
        {
            if (!_boolLookUp.ContainsKey(valueName))
            {
                _boolLookUp.Add(valueName, value);
            }
            else
            {
                _boolLookUp[valueName] = value;
            }
        }
        
        public void WriteValue(string valueName, float value)
        {
            if (!_floatLookUp.ContainsKey(valueName))
            {
                _floatLookUp.Add(valueName, value);
            }
            else
            {
                _floatLookUp[valueName] = value;
            }
        }
        
        public void WriteValue(string valueName, byte value)
        {
            if (!_byteLookUp.ContainsKey(valueName))
            {
                _byteLookUp.Add(valueName, value);
            }
            else
            {
                _byteLookUp[valueName] = value;
            }
        }
        
        public void WriteValue(string valueName, int value)
        {
            if (!_intLookUp.ContainsKey(valueName))
            {
                _intLookUp.Add(valueName, value);
            }
            else
            {
                _intLookUp[valueName] = value;
            }
        }
    }
}