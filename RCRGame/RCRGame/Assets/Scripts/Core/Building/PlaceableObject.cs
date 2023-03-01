using System;
using RCRCoreLib.Core.CameraLib;
using RCRCoreLib.Core.SaveSystem;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Systems;
using Unity.Collections;
using UnityEngine;

namespace RCRCoreLib.Core.Building
{
    public class PlaceableObject: MonoBehaviour
    {
        public ShopItem item;
        
        public bool Placed { get; private set; }
        private Vector3 origin;

        public Transform OriginTransform; //Mainly Used as an offset for the TilePlacement
        public BoundsInt area;

        [ReadOnly] 
        public PlaceableObjectData data = new PlaceableObjectData();

        // private void Awake()
        // {
        //     OriginTransform = GetComponentInChildren<Transform>();
        // }
        

        public virtual void Initialize(ShopItem shopItem)
        {
            item = shopItem;
            data.assetName = item.name;
            data.ID = SaveData.GeneratedID();
        }

        public virtual void Initialize(ShopItem shopItem, PlaceableObjectData data)
        {
            item = shopItem;
            this.data = data;
        }

        public virtual bool CanBePlaced()
        {
            Vector3Int positionInt = BuildingSystem.Instance.gridLayout.LocalToCell(OriginTransform.position);
            BoundsInt areaTemp = new BoundsInt(positionInt, new Vector3Int(area.size.x, area.size.y, 1));
            return BuildingSystem.Instance.CanTakeArea(areaTemp);
        }

        public virtual void Place()
        {
            Vector3Int positionInt = BuildingSystem.Instance.gridLayout.LocalToCell(OriginTransform.position);
            BoundsInt areaTemp = new BoundsInt(positionInt, new Vector3Int(area.size.x, area.size.y, 1));
            Placed = true;
            BuildingSystem.Instance.TakeArea(areaTemp);
            
            PanZoom.Instance.UnfollowObject();
        }

        public void CheckPlacement()
        {
            if (!Placed)
            {
                if (CanBePlaced())
                {
                    Place();
                    origin = transform.position;
                }
                else
                {
                    Destroy(transform.gameObject);
                }
                ShoppingManager.Instance.OnShop_Btn_clicked();
            }
            else
            {
                if (CanBePlaced())
                {
                    Place();
                    origin = transform.position;
                }
                else
                {
                    transform.position = origin;
                    Place();
                }
            }
            
        }

        public virtual void Load()
        {
            PanZoom.Instance.UnfollowObject();
            Destroy(GetComponent<ObjectDrag>());
            Place();
        }

        private float time = 0f;
        private bool touching;

        private void Update()
        {
            if (!touching && Placed)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    time = 0f;
                }else if (UnityEngine.Input.GetMouseButton(0))
                {
                    time += Time.deltaTime;

                    if (time > 3f)
                    {
                        touching = true;
                        gameObject.AddComponent<ObjectDrag>();
                        Vector3Int positionInt = BuildingSystem.Instance.gridLayout.WorldToCell(OriginTransform.position);
                        BoundsInt areaTemp = new BoundsInt(positionInt, new Vector3Int(area.size.x, area.size.y, 1));
                        BuildingSystem.Instance.ClearArea(areaTemp, BuildingSystem.Instance.MainTilemap);
                    }
                }
            }

            if (touching && UnityEngine.Input.GetMouseButtonUp(0))
            {
                touching = false;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            data.position = transform.position;
            GameManager.Instance.saveData.AddData(data);
        }
    }
}