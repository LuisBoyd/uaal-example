using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopItemDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {

        public static Canvas canvas;
        private RectTransform rt;
        private CanvasGroup cg;
        private Image img;

        private Vector3 orginPos;
        private bool drag;

        protected void Awake()
        {
            rt = GetComponent<RectTransform>();
            cg = GetComponent<CanvasGroup>();

            img = GetComponent<Image>();
            orginPos = rt.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;
            cg.blocksRaycasts = false;
            img.maskable = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnDrag(PointerEventData eventData)
        {
            drag = false;
            cg.blocksRaycasts = true;
            img.maskable = true;
            rt.anchoredPosition = orginPos;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            ShoppingManager.Instance.OnShop_Btn_clicked();
            Color c = Color.clear;
            c.a = 0f;
            img.color = c;
        }

        private void OnEnable()
        {
            drag = false;
            cg.blocksRaycasts = true;
            img.maskable = true;
            rt.anchoredPosition = orginPos;
            Color c = Color.clear;
            c.a = 1f;
            img.color = c;
        }
    }
}