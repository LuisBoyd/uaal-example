using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.MapModification;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Shopping;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using RCRCoreLib.Core.Utilities;

namespace RCRCoreLib.Core.Systems.Tiles
{
    public class TileSelectionTab : TabGroup
    {
        [SerializeField] 
        protected TileSelectionOptions option;
        
        [SerializeField] 
        protected Vector2 SelectedSizeScale;
        [SerializeField]
        protected Vector2 UnSelectedSizeScale;

        [SerializeField] 
        protected Color SelectedColor;
        [SerializeField] 
        protected Color UnSelectedColor;

        [SerializeField] 
        protected Image Buttonbackground;
        

        protected void OnDisable()
        {
            Close();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var TileSelectedEvent = new GroupTabSelectedEvent(this);
            EventManager.Instance.QueueEvent(TileSelectedEvent);
            EventManager.Instance.QueueEvent(new NewTilePaintBrushSelected(option));
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
        }
        

        public override void Open()
        {
            Extenstions.ScaleGUI(selfTransform, SelectedSizeScale, time_to_lerp);
            Buttonbackground.color = SelectedColor;
        }

        public override void Close()
        {
            Extenstions.ScaleGUI(selfTransform, UnSelectedSizeScale, time_to_lerp);
            Buttonbackground.color = UnSelectedColor;
        }
    }
}