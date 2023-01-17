using System;
using BuildingComponents.ScriptableObjects;
using Patterns.ObjectPooling;
using Patterns.ObjectPooling.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI.uGUI
{
    [RequireComponent(typeof(ScrollRect), typeof(Pool<Icon_Attraction>))]
    public class Menu_BuildingPurchaseSelection : MonoBehaviour, IDataBindable<ConcreteBuildingObject, Icon_Attraction>
    {
        [SerializeField] 
        private ConcreteBuildingObject[] _scriptableConcreteBuildingObjects;
        
        [SerializeField]
        private RectTransform _contentRectTransform;

        private ComponentPool<Icon_Attraction> AttractionPool;

        private void Awake()
        {
            AttractionPool = GetComponent<ComponentPool<Icon_Attraction>>();
        }

        private void OnEnable()
        {
            if(_contentRectTransform == null || AttractionPool == null)
                return;
            AttractionPool.SetRoot(_contentRectTransform);
            AttractionPool.SetParent(this.transform);
            AttractionPool.PreWarm(8); //The number of building types
            foreach (ConcreteBuildingObject buildingObject in _scriptableConcreteBuildingObjects)
            {
                BindData(buildingObject,AttractionPool.Request());
            }
        }

        public void BindData(ConcreteBuildingObject data, Icon_Attraction obj)
        {
            obj.BuildingTittle = data.PrestigeXBuildingObjects[0].Name;
            obj.Cost = data.PrestigeXBuildingObjects[0].Cost;
            obj.Display = data.PrestigeXBuildingObjects[0].PrestigeTexture;
        }
    }
}