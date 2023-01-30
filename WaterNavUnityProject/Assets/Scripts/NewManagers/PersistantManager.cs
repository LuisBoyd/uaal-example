using System;
using System.Collections;
using System.Collections.Generic;
using Events.Library.Models;
using Events.Library.Models.SystemEvents;
using RCR.BaseClasses;
using System.Linq;
using Concurrent;
using Systems.PersistantData;
using UnityEngine;

namespace NewManagers
{
    public class PersistantManager : Singelton<PersistantManager>
    {
        private IEnumerable<IPersistantData> _persistantDatas;
        private Token _SaveRequestEvent;
        private Token _LoadRequestEvent;
        
        protected override void Awake()
        {
            base.Awake();
           _SaveRequestEvent = GameManager_2_0.Instance.EventBus.Subscribe<SaveRequestEvent>(SaveRequested);
           _LoadRequestEvent = GameManager_2_0.Instance.EventBus.Subscribe<LoadRequestEvent>(LoadRequested);
            _persistantDatas = FindObjectsOfType<MonoBehaviour>().OfType<IPersistantData>();
        }

        private void OnDisable()
        {
            GameManager_2_0.Instance.EventBus.UnSubscribe_Coroutine<SaveRequestEvent>(_SaveRequestEvent.TokenId);
            GameManager_2_0.Instance.EventBus.UnSubscribe_Coroutine<LoadRequestEvent>(_LoadRequestEvent.TokenId);
            _persistantDatas = null;
        }


        #region EventStuff
        private IEnumerator SaveRequested(SaveRequestEvent evnt, EventArgs args)
        {
            foreach (var data in _persistantDatas)
            {
                yield return new WaitForTask(FileManager.WriteToFileAsync(data));
            }
        }
        private IEnumerator LoadRequested(LoadRequestEvent evnt, EventArgs args)
        {
            foreach (var data in _persistantDatas)
            {
                yield return new WaitForTask(FileManager.LoadFromFileAsync(data));
            }
        }
        #endregion
    }
}