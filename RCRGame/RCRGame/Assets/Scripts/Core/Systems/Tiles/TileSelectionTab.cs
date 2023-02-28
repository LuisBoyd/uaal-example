using RCRCoreLib.Core.Shopping;
using RCRCoreLib.MapModification;
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



        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.Instance.AddListener<NewTilePaintBrushSelected>(OnNewTilePaintBrushSelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.Instance.RemoveListener<NewTilePaintBrushSelected>(OnNewTilePaintBrushSelected);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var TileSelectedEvent = new NewTilePaintBrushSelected(option);
            EventManager.Instance.QueueEvent(TileSelectedEvent);
        }

        protected virtual void OnNewTilePaintBrushSelected(NewTilePaintBrushSelected evnt)
        {
            if (evnt.option == option)
            {
                Selected();
            }
            else
            {
                Unselected();
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }

        public override void SlideIn()
        {
            throw new System.NotImplementedException();
        }

        public override void SlideOut()
        {
            throw new System.NotImplementedException();
        }

        public override void FadeIn()
        {
            throw new System.NotImplementedException();
        }

        public override void FadeOut()
        {
            throw new System.NotImplementedException();
        }

        public override void ScaleIn()
        {
            throw new System.NotImplementedException();
        }

        public override void ScaleOut()
        {
            throw new System.NotImplementedException();
        }

        public override void Selected()
        {
            Extenstions.ScaleGUI(UItransform, SelectedSizeScale, TimeToLerp);
            //LeanTween.scale(UItransform, new Vector3(SelectedSizeScale.x, SelectedSizeScale.y, .1f), TimeToLerp);
            Buttonbackground.color = SelectedColor;
        }

        public override void Unselected()
        {
            Extenstions.ScaleGUI(UItransform, UnSelectedSizeScale, TimeToLerp);
            //LeanTween.scale(UItransform, new Vector3(UnSelectedSizeScale.x, UnSelectedSizeScale.y, .1f), TimeToLerp);
            Buttonbackground.color = UnSelectedColor;
        }
    }
}