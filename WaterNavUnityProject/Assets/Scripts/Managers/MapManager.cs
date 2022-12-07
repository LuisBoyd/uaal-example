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
using UnityEngine.Tilemaps;

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

        private byte this[int index0, int index1]
        {
            get => m_ByteMapData[index0 * (Tile_sectionCellSize * m_mapsize_sqr) + index1];
            set => m_ByteMapData[index0 * (Tile_sectionCellSize * m_mapsize_sqr) + index1] = value;
        }

        private int MaxMapSectionCount;
        private int mapRowCount;
        private int MaxRowEntries;

        private int m_iterationX, m_iterationY;
        
        private MapSectionView[,] m_mapSections;

        private Tilemap m_tilemap;

        protected override void Awake()
        {
            base.Awake();
            m_tilemap = GetComponent<Tilemap>();
            if(m_tilemap == null)
                Debug.LogWarning($"Null Tilemap");
        }


        public void LoadAreaData(int[] data)
        {
            m_IntergerMapData = data;
            m_mapSize = m_IntergerMapData.Length;
            m_ByteMapData = new byte[Tile_sectionSize_bytes * data.Length];
            m_mapSections = new MapSectionView[m_mapsize_sqr, m_mapsize_sqr];

            if (!IsBlankMap(data)) //if any value != 0 returns true
            {
                StartCoroutine(NetworkManager.Instance.PutRequest("ProcessNewMap",
                    new Dictionary<string, string>
                    {
                        {"Userkey", GameManager.Instance.UserKey},
                        {"Region", GameManager.Instance.Region},
                        {"MapID",  GameManager.Instance.MapID.ToString()}
                    }, StartAreaResponse));
            }
            else
            {
                StartCoroutine(ProcessData());
            }
            
            
        }

        private IEnumerator ProcessData()
        {
            // for (byte_segment_offset = 0; byte_segment_offset < m_IntergerMapData.Length; byte_segment_offset++)
            // {
            //     yield return NetworkManager.Instance.PutRequest("RequestMapSectionData",
            //         new Dictionary<string, string>()
            //         {
            //             {"SegmentID", m_IntergerMapData[byte_segment_offset].ToString()}
            //         }, mapSection_Onrecived);
            // }
            for (m_iterationX = 0; (m_iterationX + 1) % (m_mapsize_sqr + 1) != 0; m_iterationX++)
            {
                for ( m_iterationY = 0; (m_iterationY + 1) % (m_mapsize_sqr + 1) != 0; m_iterationY++)
                {
                    if (m_IntergerMapData[(m_iterationX * m_mapsize_sqr) + m_iterationY] != 0)
                    {
                        yield return NetworkManager.Instance.PutRequest("RequestMapSectionData",
                            new Dictionary<string, string>()
                            {
                                {
                                    "SegmentID",
                                    m_IntergerMapData[(m_iterationX * m_mapsize_sqr) + m_iterationY].ToString()
                                }
                            }, mapSection_Onrecived);
                    }
                }
            }
            
            //m_ByteMapData = AreabyteSorting(m_ByteMapData);
            SortBytesForTileMap(ref m_ByteMapData);
            yield return RenderTiles(TileManager.Instance.recieveBytes(m_ByteMapData));
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

        private void SortBytesForTileMap(ref byte[] byteArray)
        {
            for (int x = 0; x < m_mapsize_sqr; x++)
            {
                for (int y = 0; y < m_mapsize_sqr; y++)
                {
                    if (m_mapSections[x, y] != null)
                    {
                        for (int mx = 0;
                             (mx + 1) % (m_mapSections[x, y].Model.SectionBounds.size.x + 1) != 0;
                             mx++) //This can be any Value that works with the size in this case both X and Y = 50
                        {
                            Buffer.BlockCopy(m_mapSections[x, y].Model.TileValues.Value,
                                mx * Tile_sectionCellSize,
                                byteArray,
                                ((mx * (Tile_sectionCellSize * m_mapsize_sqr)) +
                                 (x * (Tile_sectionSize_bytes * m_mapsize_sqr))) + (y * Tile_sectionCellSize),
                                Tile_sectionCellSize);

                            //Y pos + X Pos

                            // for (int my = 0; (my + 1) % (m_mapSections[x,y].Model.SectionBounds.yMax + 1) != 0; my++) //TODO this only works with 50x50 map section types
                            // {
                            //     Buffer.BlockCopy(m_mapSections[x,y].Model.TileValues.Value,
                            //         (mx*Tile_sectionCellSize));
                            // }
                        }
                    }
                }
            }
        }
        
        

        private void mapSection_Onrecived(bool valid, string data)
        {
            JObject mapBase64 = JObject.Parse(data);
            if (valid && mapBase64 != null)
            {
                byte[] tileBytes = Convert.FromBase64String(
                    mapBase64["SegmentData"].ToString());

                m_mapSections[m_iterationX, m_iterationY] = gameObject.AddComponent<MapSectionView>();

                m_mapSections[m_iterationX, m_iterationY].Model.TileValues.Value = new byte[Tile_sectionSize_bytes];
                
                Buffer.BlockCopy(tileBytes,0,
                    m_mapSections[m_iterationX, m_iterationY].Model.TileValues.Value,
                    0, Tile_sectionSize_bytes);
                m_mapSections[m_iterationX, m_iterationY].Model.SectionBounds =
                    new BoundsInt(
                        new Vector3Int(m_iterationY * Tile_sectionCellSize, m_iterationX * Tile_sectionCellSize),
                        new Vector3Int(Tile_sectionCellSize, Tile_sectionCellSize));

                // Buffer.BlockCopy(tileBytes,0, m_ByteMapData, byte_segment_offset * Tile_sectionSize_bytes,
                //     Tile_sectionSize_bytes);

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
                    StartCoroutine(ProcessData());
                }
                else
                {
                    //TODO Error Handling / Safety Check
                    RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.MAP_PROCESSING_PROBLEM));
                    Application.Quit();
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

        private bool IsBlankMap(int[] data) => data.Any(d => d != 0); //If Any != 0 returns true

        private IEnumerator RenderTiles(TileBase[] tiles)
        {
            m_tilemap.origin = Vector3Int.zero;
            m_tilemap.size = new Vector3Int(Tile_sectionCellSize * m_mapsize_sqr,
                Tile_sectionCellSize * m_mapsize_sqr,1);
            m_tilemap.ResizeBounds();
            m_tilemap.SetTilesBlock(m_tilemap.cellBounds, tiles);
            yield return new WaitForEndOfFrame();
        }

    }
}