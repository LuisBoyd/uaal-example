using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DataStructures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using RCR.DataStructures;
using RCR.Enums;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Grid = UnityEngine.Grid;

/*
 * Information To be passed into me
 *
 * Location - poiID
 * user - username/Email 
 */

namespace RCR.Managers
{
    /// <summary>
    /// Main "Program Class" First Execution Happens here
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameManager: Singelton<GameManager>
    {
        private PlayerData m_playerData;
        private MapData m_mapData;

        private bool m_mapProccessingProblem = false;

        private Tilemap m_tilemap;
        
        private const int Tile_sectionSize_bytes = 2500;

        // private DelegateNoArg m_data_proccessed_ready;
        // private DelegateNoArg m_initialRenderingDone;


        public event OnQuit OnQuitting
        {
            add
            {
                Debug.Log($"{value.Method.Name} subscribed to OnQuit Event");
                m_OnQuitting += value;
            }
            remove
            {
                Debug.Log($"{value.Method.Name} Un-subscribed to OnQuit Event");
                m_OnQuitting -= value;
            }
        }

        private event OnQuit m_OnQuitting;
        
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void PoulateData(string userKey, string mapID, string regionName)
        {
            m_playerData = new PlayerData();
            m_mapData = new MapData();

            m_mapData.Region = regionName;
            m_mapData.MapID = mapID;
            m_playerData.UserKey = userKey;
        }

        private bool IsBlankMap()
        {
            for (int i = 0; i < m_mapData.MapSize ; i++)
            {
                if (m_mapData.MapIdArray[i] != 0)
                    return false;
            }

            return true;
        }
        
        public void ProcessAreaData(int[] areaData)
        {
            m_mapData.MapSize = areaData.Length;
            m_mapData.MapSize_sqr = Mathf.Sqrt(areaData.Length);
            m_mapData.MapIdArray = areaData;
            m_mapData.MapByteStructure = new byte[Tile_sectionSize_bytes * areaData.Length];
            if (!MathUtils.IsFloatWhole(m_mapData.MapSize_sqr))
            {
                RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.WARNING, RCRMessageType.MAP_NOT_EQUIDIMENSIONAL));
                Application.Quit(); //TODO APPLE IMPLEMENTATION DOES NOT RECOMMEND CALLING QUIT https://developer.apple.com/library/archive/qa/qa1561/_index.html
                return;
            }
            else
            {
                if (!IsBlankMap())
                {
                    StartCoroutine(ProcessOldAreaData());
                }
                else
                {
                    ProcessNewArea();
                }
            }
        }

        private void ProcessNewArea()
        {
            Action<bool, string> responseMethod = (b, s) =>
            {
                if (b)
                {
                    JObject RandomMapID = JObject.Parse(s);
                    if (RandomMapID != null)
                    {
                        m_mapData.MapIdArray[0] = RandomMapID["MapId"].ToObject<int>();
                        StartCoroutine(ProcessOldAreaData());
                    }
                    else
                    {
                        RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.MAP_PROCESSING_PROBLEM));
                        Application.Quit();//TODO ALERT USER THAT THEIR WAS A PROBLEM AND IMPLEMENT A WAY TO RESOLVE THIS
                    }
                }
            };

            StartCoroutine(NetworkManager.Instance.PutRequest("ProcessNewMap",
                new Dictionary<string, string>
                {
                    {"Userkey", m_playerData.UserKey},
                    {"Region", m_mapData.Region},
                    {"MapID", m_mapData.MapID}
                }, responseMethod));
        }

        private IEnumerator ProcessOldAreaData()
        {
            int i = 0;
            JObject MapBase64 = null;
            Action<bool, string> response = (b, s) =>
            {
                MapBase64 = null;
                if (b)
                {
                    MapBase64 = JObject.Parse(s);
                    if (MapBase64 != null)
                    {
                        string Base64 = MapBase64["SegmentData"].ToString();
                        Debug.Log(Base64);
                        byte[] TileBytes = Convert.FromBase64String(Base64);
                        Debug.Log($"{TileBytes.Length} The length of tiles");
                        Buffer.BlockCopy(TileBytes,0, m_mapData.MapByteStructure, i * Tile_sectionSize_bytes, Tile_sectionSize_bytes);
                        Debug.Log("Buffer Copy");
                    }
                    else
                    {
                        RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.ADDRESABBLE_TILE_LOADING_ISSUE));
                        Application.Quit();
                    }
                }
                else
                {
                    RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.WARNING, s));
                }
            };
            Debug.Log($"Map ID ARRAY Length : {m_mapData.MapIdArray.Length}");
            for (i = 0; i < m_mapData.MapIdArray.Length; i++)
            {

                yield return NetworkManager.Instance.PutRequest("RequestMapSectionData",
                    new Dictionary<string, string>()
                    {
                        {"SegmentID", m_mapData.MapIdArray[i].ToString()}
                    }, response);
                Debug.Log("completed one instance");
            }

            m_mapData.MapByteStructure = AppUtilities.SortBytes(m_mapData.MapByteStructure, Tile_sectionSize_bytes);

            yield return RenderTiles(TileManager.Instance.recieveBytes(m_mapData.MapByteStructure));
        }

        private IEnumerator RenderTiles(TileBase[] tiles)
        {
            Grid grid = new GameObject("Grid").AddComponent<Grid>();
            m_tilemap = new GameObject("Map").AddComponent<Tilemap>();
            Debug.Log($"Added Grid length of tile is {tiles.Length}");
            m_tilemap.gameObject.AddComponent<TilemapRenderer>();
            m_tilemap.gameObject.transform.SetParent(grid.transform);
            m_tilemap.origin = Vector3Int.zero;
            int Length = Mathf.FloorToInt(Mathf.Sqrt(tiles.Length));
            m_tilemap.size = new Vector3Int(Length,Length, 1);
            m_tilemap.ResizeBounds();
            m_tilemap.SetTilesBlock(m_tilemap.cellBounds, tiles);
            yield return new WaitForEndOfFrame();
        }


    }
}