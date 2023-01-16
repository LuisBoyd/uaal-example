using Events.Library;
using Events.Library.Unity;
using Events.Library.Utils;
using RCR.BaseClasses;
using UnityEngine;

namespace NewManagers
{
    [DefaultExecutionOrder(-100)]
    public class GameManager_2_0 : Singelton<GameManager_2_0>
    {
        public IUnityEventBus EventBus;

        protected override void Awake()
        {
            base.Awake();
            EventBus = new UnityEventBus(new TokenUtils());
        }

        #region EventBusStuff
        
        #endregion

        #region utilities

        public GameObject Clone(GameObject obj) => Instantiate(obj);

        #endregion

    }
}