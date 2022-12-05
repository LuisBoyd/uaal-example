using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using Gameplay;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using RCR.DataStructures;
using RCR.Enums;
using RCR.Utilities;
using UnityEngine;

namespace RCR.Managers
{
    public class MapManager : Singelton<MapManager>
    {
        private int m_mapSize;
        private const int Tile_sectionSize_bytes = 2500;
        private const int Tile_sectionCellSize = 50;
        private int m_mapsize_sqr
        {
            get { return MathUtils.sqrt(m_mapSize); }
        }
        
        
        private int byte_segment_offset = 0;

        private int[] m_IntergerMapData;
        private byte[] m_ByteMapData;

        private int MaxMapSectionCount;
        private int mapRowCount;
        private int MaxRowEntries;

        private int MapSectionXCounter = 0;
        private int MapSectionYCounter = 0;
        
        
        private MapSectionView[,] m_mapSections;
        

        public void LoadAreaData(int[] data)
        {
            m_IntergerMapData = data;
            m_mapSize = m_IntergerMapData.Length;
            m_ByteMapData = new byte[Tile_sectionSize_bytes * data.Length];
            m_mapSections = new MapSectionView[m_mapsize_sqr, m_mapsize_sqr];

            if (!IsBlankMap(data))
            {
                StartCoroutine(NetworkManager.Instance.PutRequest("ProcessNewMap",
                    new Dictionary<string, string>
                    {
                        {"Userkey", GameManager.Instance.UserKey},
                        {"Region", GameManager.Instance.Region},
                        {"MapID",  GameManager.Instance.MapID.ToString()}
                    }, StartAreaResponse));
            }
            
            
        }

        private IEnumerator ProcessData()
        {
            for (byte_segment_offset = 0; byte_segment_offset < m_IntergerMapData.Length; byte_segment_offset++)
            {
                yield return NetworkManager.Instance.PutRequest("RequestMapSectionData",
                    new Dictionary<string, string>()
                    {
                        {"SegmentID", m_IntergerMapData[byte_segment_offset].ToString()}
                    }, mapSection_Onrecived);
            }

            m_ByteMapData = AreabyteSorting(m_ByteMapData);
        }

        /// <summary>
        /// Should be done before ordering
        /// </summary>
        private void ProcessMapSections()
        {
            for (int i = 0; i < m_mapsize_sqr; i++)
            {
                // m_mapSections[i] = gameObject.AddComponent<MapSectionView>();
                // m_mapSections[i].Model.SectionBounds = new BoundsInt(new Vector3Int((i % m_mapsize_sqr) * Tile_sectionCellSize,
                //     ))
                // byte[] values = new byte[Tile_sectionSize_bytes];
                for (int j = 0; j < m_mapsize_sqr; j++)
                {
                    m_mapSections[i, j] = gameObject.AddComponent<MapSectionView>();
                    m_mapSections[i, j].Model.SectionBounds = new BoundsInt(
                        new Vector3Int(j * Tile_sectionCellSize, i * Tile_sectionCellSize),
                        new Vector3Int(Tile_sectionCellSize, Tile_sectionCellSize));
                }
                

            }
        }
        

        private byte[] AreabyteSorting( byte[] unorderedBytes)
        {
             MaxMapSectionCount = MathUtils.DivisionInto(unorderedBytes.Length, Tile_sectionSize_bytes); //MaxMap Sections like 5x5 is 25
             mapRowCount = MathUtils.sqrt(Tile_sectionSize_bytes); //How many rows are in a Map Section
             MaxRowEntries = MathUtils.sqrt(Tile_sectionSize_bytes); //How many elements are in a row

            MapArray<byte> sortedBytes = new MapArray<byte>(unorderedBytes, MaxMapSectionCount,
                mapRowCount, MaxRowEntries);

            return sortedBytes.SortedArray;
        }

        private void mapSection_Onrecived(bool valid, string data)
        {
            JObject mapBase64 = JObject.Parse(data);
            if (valid && mapBase64 != null)
            {
                byte[] tileBytes = Convert.FromBase64String(
                    mapBase64["SegmentData"].ToString());
                
                Buffer.BlockCopy(tileBytes,0, m_ByteMapData, byte_segment_offset * Tile_sectionSize_bytes,
                    Tile_sectionSize_bytes);

            }
            else
            {
                RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.ADDRESABBLE_TILE_LOADING_ISSUE));
                Application.Quit();
            }
        }

        private void StartAreaResponse(bool valid, string data)
        {
            if (valid)
            {
                JObject randomMapId = JObject.Parse(data);
                if (randomMapId != null)
                {
                    startAreaSetup(randomMapId);
                }
                else
                {
                    //TODO Error Handling / Safety Check
                }
                
            }
        }

        private void startAreaSetup(JObject obj)
        {
            Tuple<int, int> medianValues = MathUtils.GetMedian(m_mapsize_sqr);
            if (medianValues.Item1 != 0 && medianValues.Item2 != 0)
            {
                m_IntergerMapData[medianValues.Item1] = obj["MapId"].ToObject<int>();
                m_IntergerMapData[medianValues.Item1 + m_mapsize_sqr] = obj["StaringLandID"].ToObject<int>();
            }
            else if (medianValues.Item1 != 0)
            {
                m_IntergerMapData[medianValues.Item1] = obj["MapId"].ToObject<int>();
                m_IntergerMapData[medianValues.Item1 + m_mapsize_sqr] = obj["StaringLandID"].ToObject<int>();
            }
            else
            {
                RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.MAP_PROCESSING_PROBLEM));
                Application.Quit();
            }
        }

        private bool IsBlankMap(int[] data) => data.Any(d => d != 0);

    }
}