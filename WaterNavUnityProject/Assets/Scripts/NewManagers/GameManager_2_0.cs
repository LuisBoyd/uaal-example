using System;
using System.Collections;
using System.Collections.Generic;
using Events.Library;
using Events.Library.Models.SystemEvents;
using Events.Library.Unity;
using Events.Library.Utils;
using Patterns.ObjectPooling.Model;
using RCR.BaseClasses;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace NewManagers
{
    [DefaultExecutionOrder(-100)]
    public class GameManager_2_0 : Singelton<GameManager_2_0>
    {
        #region SerilizedFields
        [SerializeField] 
        private Sprite BoatTexture;
        [SerializeField] 
        private Sprite CustomerTexture;
        #endregion

        #region Values
        public IUnityEventBus EventBus;
        public readonly Dictionary<Type, Sprite> SpriteDicitonary = new Dictionary<Type, Sprite>();
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            EventBus = new UnityEventBus(new TokenUtils());

        }

        // private IEnumerator Start()
        // {
        //    //yield return EventBus.Publish(new LoadRequestEvent(), EventArgs.Empty);
        // }

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