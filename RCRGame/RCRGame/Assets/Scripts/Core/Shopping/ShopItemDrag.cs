﻿using System;
using RCRCoreLib.Core.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopItemDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {

        [SerializeField]
        private ShopItem Item;
        
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

        public void Initialize(ShopItem item)
        {
            Item = item;
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

            Vector3 position = new Vector3(transform.position.x, transform.position.y);
            position = Camera.main.ScreenToWorldPoint(position);
            
           var obj = BuildingSystem.Instance.InitializeWithObject(Item.prefab, position);
           obj.GetComponent<PlaceableObject>().Initialize(Item);
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