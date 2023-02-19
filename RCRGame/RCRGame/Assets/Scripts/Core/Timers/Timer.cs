using System;
using UnityEngine;
using UnityEngine.Events;

namespace RCRCoreLib.Core.Timers
{
    public class Timer : MonoBehaviour
    {
        public string Name { get; private set; }
        public bool IsRunning { get; private set; }
        private DateTime startTime;
        public TimeSpan timeToFinish { get; private set; }
        private DateTime finishTime;
        public UnityEvent TimerFinishedEvent;
        
        public double secondsLeft { get; private set; }

        public int skipAmount
        {
            get
            {
                return (int) (secondsLeft / 60) * 2;
            }
        }

        public void Initialize(string processName, DateTime start, TimeSpan time)
        {
            name = processName;
            startTime = start;
            timeToFinish = time;
            finishTime = start.Add(time);

            TimerFinishedEvent = new UnityEvent();
        }

        public void StartTimer()
        {
            secondsLeft = timeToFinish.TotalSeconds;
            IsRunning = true;
        }

        private void Update()
        {
            if (IsRunning)
            {
                if (secondsLeft > 0)
                    secondsLeft -= Time.deltaTime;
                else
                {
                    TimerFinishedEvent.Invoke();
                    secondsLeft = 0;
                    IsRunning = false;
                }
                    
            }
        }

        public string DisplayTime()
        {
            string text = "";
            TimeSpan timeleft = TimeSpan.FromSeconds(secondsLeft);

            if (timeleft.Days != 0)
            {
                text += timeleft.Days + "d ";
                text += timeleft.Hours + "h";
            }
            else if(timeleft.Hours != 0)
            {
                text += timeleft.Hours + "h ";
                text += timeleft.Minutes + "min";
            }
            else if (timeleft.Minutes != 0)
            {
                text += timeleft.Minutes + "min ";
                text += timeleft.Seconds + "sec";
            }
            else if (secondsLeft > 0)
            {
                text += Mathf.FloorToInt((float) secondsLeft) + "sec";
            }
            else
            {
                text = "Finished";
            }

            return text;
        }

        public void SkipTimer()
        {
            secondsLeft = 0;
            finishTime = DateTime.Now;
        }
    }
}