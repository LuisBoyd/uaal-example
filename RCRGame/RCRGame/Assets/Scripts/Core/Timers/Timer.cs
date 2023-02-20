using System;
using UnityEngine;
using UnityEngine.Events;

namespace RCRCoreLib.Core.Timers
{
    public class Timer : MonoBehaviour
    {
        public string Name { get; private set; }
        public bool IsRunning { get; private set; }
        public DateTime startTime { get; private set; }
        public TimeSpan timeToFinish { get; private set; }
        public DateTime finishTime { get; private set; }
        public UnityEvent TimerFinishedEvent;
        
        /// <summary>
        /// Updates are done in a normalized value so if Max time
        /// was 1 Hour then 1 = 1 hour, 0.5 = 30 mins, etc...
        /// </summary>
        public UnityEvent<float> TimerUpdateEvent;
        
        
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
            TimerUpdateEvent = new UnityEvent<float>();
        }
        

        public void StartTimer()
        {
            secondsLeft = timeToFinish.TotalSeconds;
            IsRunning = true;
        }
        
        /// <summary>
        /// Loads the Timer where it was previously left off at
        /// </summary>
        /// <param name="startTimerAt">Seconds into the timer</param>
        public void StartTimer(double startTimerAt)
        {
            secondsLeft = startTimerAt;
            Debug.Log($"Start Time At {startTimerAt}");
            Debug.Log($"Seconds Left {secondsLeft}");
            if (secondsLeft > 0)
            {
                IsRunning = true;
            }
            else
            {
                TimerFinishedEvent.Invoke();
                secondsLeft = 0;
                IsRunning = false;
            }
        }

        private void Update()
        {
            if (IsRunning)
            {
                if (secondsLeft > 0)
                {
                    secondsLeft -= Time.deltaTime;
                    TimerUpdateEvent?.Invoke((float) NormalizedPercentageDone());
                }
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

        public double NormalizedPercentageDone()
        {
            TimeSpan timeleft = TimeSpan.FromSeconds(timeToFinish.TotalSeconds - secondsLeft);
            return (double) timeleft.Ticks / (double) timeToFinish.Ticks;
        }

       
    }
}