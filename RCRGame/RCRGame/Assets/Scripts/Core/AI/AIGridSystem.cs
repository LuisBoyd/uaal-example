using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Tiles;
using RCRCoreLib.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.AI
{
    [RequireComponent(typeof(Tilemap))]
    public class AIGridSystem : Singelton<AIGridSystem>
    {
        [SerializeField] 
        private List<AStarAgentSettings> AgentSettings = new List<AStarAgentSettings>();
        private IDictionary<AStarAgentSettings, AIGrid> Graph;

        private Tilemap Tilemap;

        protected override void Awake()
        {
            base.Awake();
            Graph = new Dictionary<AStarAgentSettings, AIGrid>();
            Tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            foreach (AStarAgentSettings aStarAgentSettings in AgentSettings)
            {
                //Graph.Add(aStarAgentSettings, new );
            }

            WorldTile[] tileCollection = Tilemap.GetTilesBlock<WorldTile>(Tilemap.cellBounds);
        }
    }
}