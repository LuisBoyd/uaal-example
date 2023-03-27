using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.MapModification;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Systems.Tiles
{
    public class SubTileSelectionTab : TileSelectionTab
    {
        public event EventHandler<SubTileSelectionTab> OnPressed;
        public override void OnPointerDown(PointerEventData eventData)
        {
            EventManager.Instance.QueueEvent(new NewTilePaintBrushSelected(option));
            OnPressed?.Invoke(this, this);
        }
    }
}