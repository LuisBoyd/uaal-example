using Events.Library.Models;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public interface IEntitySpawningSystem
    {
        public bool Active { get; }
        public Token On_spawnerPlacement { get; }
        public void Spawn(EntitySpawningOptions options = default);
        public void Spawn(Vector2Int requestingLocation ,EntitySpawningOptions options = default);
        public void Despawn(Entity e);

        public void StopPauseSystem();
        public void ResumeSystem();
    }
}