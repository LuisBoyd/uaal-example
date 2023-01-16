using System.Collections.Generic;
using BuildingComponents.ScriptableObjects;
using NewManagers;
using RCR.Systems.ProgressSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace BuildingComponents
{
    public class ConstrcutionCrew: IConstructionCrew
    {
        private Canvas _Constructioncanvas;
        private GameObject _ProgressBar;
        private ProgressSystem _progressSystem;

        private readonly IDictionary<BuildingView, PrestigeXBuildingObject> _InConstruction;
        private readonly IDictionary<BuildingView, int> _ProgressIndicators;

        public ConstrcutionCrew(Canvas constructioncanvas, GameObject progressBar)
        {
            _progressSystem = new ProgressSystem();
            _Constructioncanvas = constructioncanvas;
            _ProgressBar = progressBar;
            _InConstruction = new Dictionary<BuildingView, PrestigeXBuildingObject>();
            _ProgressIndicators = new Dictionary<BuildingView, int>();
        }
        
        public int StartConstruction(BuildingView view, PrestigeXBuildingObject buildingObject)
        {
            if (!ConstructionExists(view))
            {
                _InConstruction.Add(view, buildingObject);
                GameObject LocalProgressBar = GameManager_2_0.Instance.Clone(_ProgressBar);
                Vector3 Position = view.transform.position;
                Position.y += view.ProgressUIOffset.y;
                LocalProgressBar.transform.position = Position;
                LocalProgressBar.transform.SetParent(_Constructioncanvas.transform);
                int startValue = _progressSystem.Start(buildingObject.Name, buildingObject.Description,
                    LocalProgressBar.GetComponent<ProgressBar>());
                _ProgressIndicators.Add(view, startValue);
                return startValue;
            }

            return -1;
        }

        public void PauseAllConstruction()
        {
            throw new System.NotImplementedException();
        }

        public void SkipConstruction(BuildingController controller)
        {
            throw new System.NotImplementedException();
        }

        public bool ConstructionExists(BuildingView view) => _InConstruction.ContainsKey(view);

    }
}