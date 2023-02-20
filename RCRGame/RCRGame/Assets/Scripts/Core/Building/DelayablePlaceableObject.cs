using System;
using RCRCoreLib.Core.CameraLib;
using RCRCoreLib.Core.SaveSystem;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Systems;
using RCRCoreLib.Core.Timers;
using UnityEngine;

namespace RCRCoreLib.Core.Building
{
    public class DelayablePlaceableObject : PlaceableObject
    {
        [SerializeField] private Material ConstructionMaterial;
        [SerializeField] private Material BuiltMaterial;
        protected bool BuildingComplete = false;
        private Timer timerToBuild;

        public override void Initialize(ShopItem shopItem)
        {
            item = shopItem;
            DelayedPlaceableObjectData delayedPlaceableObjectData = new DelayedPlaceableObjectData();
            delayedPlaceableObjectData.assetName = item.name;
            delayedPlaceableObjectData.ID = SaveData.GeneratedID();
            data = delayedPlaceableObjectData;
        }

        public override void Place()
        {
            base.Place();
            timerToBuild = gameObject.AddComponent<Timer>();
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.material = ConstructionMaterial;
            var CustomTimeSpan = DateTime.Now
                .AddDays(item.TimeToBuild_Days)
                .AddHours(item.TimeToBuild_Hours)
                .AddMinutes(item.TimeToBuild_Minutes)
                .AddSeconds(item.TimeToBuild_Seconds);
            timerToBuild.Initialize("Building", DateTime.Now, CustomTimeSpan.Subtract(DateTime.Now));
            timerToBuild.StartTimer();
            timerToBuild.TimerUpdateEvent.AddListener(delegate(float time)
            {
                renderer.material.SetFloat("_Progress", time);
            });
            timerToBuild.TimerFinishedEvent.AddListener(delegate
            {
                Destroy(timerToBuild);
                timerToBuild = null;
                renderer.material = BuiltMaterial;
                BuildingComplete = true;
                //TODO if Multiple TIMERTOOLTIP destory here
            });
            
        }

        private void PlaceFromLoad()
        {
            DelayedPlaceableObjectData deleayedData = data as DelayedPlaceableObjectData;
            if(deleayedData == null)
                return;
            //TODO FAILED THROW ERROR
            base.Place();

            if (!deleayedData.IsBuilt)
            {
                timerToBuild = gameObject.AddComponent<Timer>();
                SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                renderer.material = ConstructionMaterial;
                timerToBuild.Initialize("Building", deleayedData.StartTime , deleayedData.Duration);
                timerToBuild.TimerUpdateEvent.AddListener(delegate(float time)
                {
                    renderer.material.SetFloat("_Progress", time);
                });
                timerToBuild.TimerFinishedEvent.AddListener(delegate
                {
                    Destroy(timerToBuild);
                    timerToBuild = null;
                    renderer.material = BuiltMaterial;
                    BuildingComplete = true;
                    //TODO if Multiple TIMERTOOLTIP destory here
                });
                var timespan = deleayedData.DateTimeFinish.Subtract(DateTime.Now);
                Debug.Log($"Starting Timer Anew {timespan.TotalSeconds}");
                timerToBuild.StartTimer(timespan.TotalSeconds);
            }
            else
            {
                BuildingComplete = deleayedData.IsBuilt;
            }
            
        }

        public override void Load()
        {
            PanZoom.Instance.UnfollowObject();
            Destroy(GetComponent<ObjectDrag>());
            PlaceFromLoad();
        }

        protected override void OnApplicationQuit()
        {
            DelayedPlaceableObjectData delayedPlaceableObjectData = data as DelayedPlaceableObjectData;
            if (delayedPlaceableObjectData == null)
            {
                Debug.LogError($"Failed To Save {gameObject.name}");
                return;
            }
            delayedPlaceableObjectData.position = transform.position;
            if (timerToBuild != null)
            {
                delayedPlaceableObjectData.IsBuilt = BuildingComplete;
                delayedPlaceableObjectData.StartTime = timerToBuild.startTime;
                delayedPlaceableObjectData.DateTimeFinish = timerToBuild.finishTime;
                delayedPlaceableObjectData.Duration = timerToBuild.timeToFinish;
                Debug.Log($"Seconds Left Save {timerToBuild.DisplayTime()}");
            }
            else
            {
                delayedPlaceableObjectData.IsBuilt = BuildingComplete;
                delayedPlaceableObjectData.StartTime = default;
                delayedPlaceableObjectData.DateTimeFinish = default;
                delayedPlaceableObjectData.Duration = default;
            }
            GameManager.Instance.saveData.AddData(delayedPlaceableObjectData);

        }
    }
}