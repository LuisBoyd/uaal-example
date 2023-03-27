using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCRCoreLib.Core.Events
{
    public  class EventManager : Singelton<EventManager> //Only really needs to be static as only ever going to be one EventManager Instance.
    {
        public  bool limitQueueProcessing = false;
        public  float queueProcessTime = 0.0f;
        private  Queue ms_eventQueue = new Queue();

        public delegate void EventDelegate<T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        private  Dictionary<Type, EventDelegate> ms_delegates = new Dictionary<Type, EventDelegate>();
        private  Dictionary<Delegate, EventDelegate> ms_delegateLookUp =
            new Dictionary<Delegate, EventDelegate>();
        private  Dictionary<Delegate, Delegate> ms_onceLookups = new Dictionary<Delegate, Delegate>();

        private  EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
        {
            if (ms_delegateLookUp.ContainsKey(del))
                return null;//Early Quitting of function
            
            //Create a new non-generic delegate which calls the genric one.
            //This is the delegate we actually invoke.
            EventDelegate internalDelegate = (e) => del((T) e);
            ms_delegateLookUp[del] = internalDelegate;

            EventDelegate tempdel;
            if (ms_delegates.TryGetValue(typeof(T), out tempdel))
            {
                ms_delegates[typeof(T)] = tempdel += internalDelegate;
            }
            else
            {
                ms_delegates[typeof(T)] = internalDelegate;
            }

            return internalDelegate;
        }
        
        public  void AddListener<T> (EventDelegate<T> del) where T : GameEvent {
            AddDelegate<T>(del);
        }
        
        public  void AddListenerOnce<T> (EventDelegate<T> del) where T : GameEvent {
            EventDelegate result = AddDelegate<T>(del);

            if(result != null){
                // remember this is only called once
                ms_onceLookups[result] = del;
            }
        }
        
        public  void RemoveListener<T> (EventDelegate<T> del) where T : GameEvent {
            EventDelegate internalDelegate;
            if (ms_delegateLookUp.TryGetValue(del, out internalDelegate)) {
                EventDelegate tempDel;
                if (ms_delegates.TryGetValue(typeof(T), out tempDel)){
                    tempDel -= internalDelegate;
                    if (tempDel == null){
                        ms_delegates.Remove(typeof(T));
                    } else {
                        ms_delegates[typeof(T)] = tempDel;
                    }
                }

                ms_delegateLookUp.Remove(del);
            }
        }
        
        public  void RemoveAll(){
            ms_delegates.Clear();
            ms_delegateLookUp.Clear();
            ms_onceLookups.Clear();
        }
        
        public  bool HasListener<T> (EventDelegate<T> del) where T : GameEvent {
            return ms_delegateLookUp.ContainsKey(del);
        }
        
        public  void TriggerEvent (GameEvent e) {
            EventDelegate del;
            if (ms_delegates.TryGetValue(e.GetType(), out del)) {
                del.Invoke(e);

                // remove listeners which should only be called once
                foreach(EventDelegate k in ms_delegates[e.GetType()].GetInvocationList()){
                    if(ms_onceLookups.ContainsKey(k)){
                        ms_delegates[e.GetType()] -= k;

                        if(ms_delegates[e.GetType()] == null)
                        {
                            ms_delegates.Remove(e.GetType());
                        }

                        ms_delegateLookUp.Remove(ms_onceLookups[k]);
                        ms_onceLookups.Remove(k);
                    }
                }
            } else {
                Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
            }
        }
        
        //Inserts the event into the current queue.
        public  bool QueueEvent(GameEvent evt) {
            if (!ms_delegates.ContainsKey(evt.GetType())) {
                Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
                return false;
            }

            ms_eventQueue.Enqueue(evt);
            return true;
        }
        
        public void Update() {
            float timer = 0.0f;
            while (ms_eventQueue.Count > 0) {
                if (limitQueueProcessing) {
                    if (timer > queueProcessTime)
                        return;
                }

                GameEvent evt = ms_eventQueue.Dequeue() as GameEvent;
                TriggerEvent(evt);

                if (limitQueueProcessing)
                    timer += Time.deltaTime;
            }
        }
        
        public void OnApplicationQuit(){
            RemoveAll();
            ms_eventQueue.Clear();
        }
    }
}
