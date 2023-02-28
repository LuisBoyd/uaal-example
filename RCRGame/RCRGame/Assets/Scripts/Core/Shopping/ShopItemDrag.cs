using System;
using RCRCoreLib.Core.Building;
using RCRCoreLib.Core.Systems.Unlockable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopItemDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private UnlockablePlaceables _placeable;
        public Canvas canvas;
        private RectTransform rt;
        private CanvasGroup cg;
        private Image img;
        private Vector3 orginPos;
        private CircleCollider2D Collider2D;
        private bool drag;

        protected void Awake()
        {
            rt = GetComponent<RectTransform>();
            cg = GetComponent<CanvasGroup>();
            canvas = ShoppingManager.Instance.MainCanvas;
            img = GetComponent<Image>();
            Collider2D = GetComponent<CircleCollider2D>();
            orginPos = rt.anchoredPosition;
        }
        
        public void Initialize(UnlockablePlaceables placeable)
        {
             _placeable = _placeable;
             Collider2D.radius = rt.sizeDelta.x / 2;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;
            cg.blocksRaycasts = false;
            img.maskable = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            drag = false;
            cg.blocksRaycasts = true;
            img.maskable = true;
            rt.anchoredPosition = orginPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            ShoppingManager.Instance.OnShop_Btn_clicked();
            Color c = Color.clear;
            c.a = 0f;
            img.color = c;
            Debug.Log("Entered Trigger");
            Vector3 position = new Vector3(transform.position.x, transform.position.y);
            position = Camera.main.ScreenToWorldPoint(position);
            
           // var obj = BuildingSystem.Instance.InitializeWithObject(Item.prefab, position);
           // obj.GetComponent<PlaceableObject>().Initialize(Item);
           //TODO FIX THIS!!!
        }

        private void OnEnable()
        {
            drag = false;
            cg.blocksRaycasts = true;
            img.maskable = true;
            rt.anchoredPosition = orginPos;
            // Color c = Color.clear;
            // c.a = 1f;
            // img.color = c;
        }
    }
}