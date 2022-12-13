using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace WaterNavTiled
{
    public class TileLayer : Layer
    {
        //Array of Chunks??
        [SerializeField] private string Compression; //Could be a enum to represent this
        private UInt16[] Data;
        [SerializeField] private string encoding; //Could represent with enum
        [SerializeField] private int rowCount; //Same as MapHeight
        [SerializeField] private int ColumnCount; //same as MapWidth
    }
}