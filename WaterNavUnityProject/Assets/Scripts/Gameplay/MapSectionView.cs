using System;
using RCR.Managers;
using RCR.Patterns;
using UnityEngine.Tilemaps;

namespace Gameplay
{
    public class MapSectionView: BaseView<MapSectionModel, MapSectionController>
    {
        private Tilemap m_tilemap;
        
        protected override void Awake()
        {
            base.Awake();
            m_tilemap = GetComponent<Tilemap>();
            IsValid = m_tilemap == null;
           // Model.SectionByteData.OnValueChanged += OnTileChanged;
        }

        private void OnDisable()
        {
            //Model.SectionByteData.OnValueChanged -= OnTileChanged;
        }

        private void OnTileChanged(byte[] previousvalue, byte[] currentvalue)
        {
            if (!Equals(previousvalue, currentvalue) && IsValid)
            {
               m_tilemap.SetTiles(Model.TilePositions, TileManager.Instance.recieveBytes(currentvalue));
            }
        }
        
    }
}