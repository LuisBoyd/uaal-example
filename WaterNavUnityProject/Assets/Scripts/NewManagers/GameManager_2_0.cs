using System;
using System.Collections;
using Events.Library;
using Events.Library.Models.SystemEvents;
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

        private IEnumerator Start()
        {
           yield return EventBus.Publish(new LoadRequestEvent(), EventArgs.Empty);
        }

        #region EventBusStuff
        
        #endregion

        #region utilities

        public GameObject Clone(GameObject obj) => Instantiate(obj);

        #endregion

        #region OnEnable
        #endregion
        
        #region OnDisable
        

        #endregion
        

    }
}