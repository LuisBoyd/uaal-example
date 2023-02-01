using System;
using Events.Library.Models;
using NewManagers;
using Patterns.ObjectPooling.Model;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.NewScripts.Tilesets
{
    public abstract class LogicTile : RuleTile
    {
        [Flags]
        public enum TileIdentifiers
        {
            Path = 1
        }
        
        public enum VisualIndicator
        {
            Water = 1,
        }

        public TileIdentifiers Identifiers;
        public VisualIndicator Indicator;
        public abstract void PromptInteraction(IEntity entity);
    }

    [CreateAssetMenu]
    public class BoatSpawnerTile : LogicTile
    {
      
        public override void PromptInteraction(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
        {
            if(Application.isPlaying)
                GameManager_2_0.Instance.EventBus.Publish<TileEvents.BoatSpawnerTilePlaced>(new TileEvents.BoatSpawnerTilePlaced(
                    new TileEvents.SpecialTilePlacedArgs(position)),EventArgs.Empty);
            return base.StartUp(position, tilemap, instantiatedGameObject);
        }
    }

    [CreateAssetMenu]
    public class CustomerSpawnerTile : LogicTile
    {
        public override void PromptInteraction(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
        {
            if(Application.isPlaying)
                GameManager_2_0.Instance.EventBus.Publish<TileEvents.CustomerSpawnerTilePlaced>(new TileEvents.CustomerSpawnerTilePlaced(
                    new TileEvents.SpecialTilePlacedArgs(position)),EventArgs.Empty);
            return base.StartUp(position, tilemap, instantiatedGameObject);
        }
    }

    [CreateAssetMenu]
    public class BoatDestroyerTile : LogicTile
    {
        public override void PromptInteraction(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
        {
            if(Application.isPlaying)
                GameManager_2_0.Instance.EventBus.Publish<TileEvents.BoatDestroyerTilePlaced>(new TileEvents.BoatDestroyerTilePlaced(this), EventArgs.Empty);
            return base.StartUp(position, tilemap, instantiatedGameObject);
        }
    }
}