using System;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Currency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Timers
{
    public class TimerToolTip : Singelton<TimerToolTip>
    {
        private Timer Timer;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private TextMeshProUGUI callerNameText;
        [SerializeField] private TextMeshProUGUI timeLeftText;
        [SerializeField] private Button skipButton;
        [SerializeField] private TextMeshProUGUI skipAmountText;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Transform Parent;

        private bool countdown;

        protected override void Awake()
        {
            base.Awake();
            Parent.gameObject.SetActive(false);
        }

        private void ShowTimer(GameObject caller)
        {
            Timer = caller.GetComponent<Timer>();
            if(Timer == null)
                return;

            callerNameText.text = Timer.name;
            skipAmountText.text = Timer.skipAmount.ToString();
            skipButton.gameObject.SetActive(true);

            Vector3 position = caller.transform.position - uiCamera.transform.position;
            position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
            Parent.position = position;

            countdown = true;
            FixedUpdate();
            
           Parent.gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (countdown)
            {
                progressSlider.value = (float) (1.0 - Timer.secondsLeft / Timer.timeToFinish.TotalSeconds);
                timeLeftText.text = Timer.DisplayTime();
            }
        }

        public void SkipButton()
        {
            EventManager.Instance.AddListenerOnce<EnoughCurrencyGameEvent>(OnEnoughCurrency);
            EventManager.Instance.AddListenerOnce<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);

            CurrencyChangedGameEvent info = new CurrencyChangedGameEvent(-Timer.skipAmount, CurrencyType.Dollars);
            EventManager.Instance.QueueEvent(info);
        }

        private void OnEnoughCurrency(EnoughCurrencyGameEvent evnt)
        {
            Timer.SkipTimer();
            skipButton.gameObject.SetActive(false);
            EventManager.Instance.RemoveListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
        }

        private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent evnt)
        {
            EventManager.Instance.RemoveListener<EnoughCurrencyGameEvent>(OnEnoughCurrency);
        }

        public void HideTimer()
        {
            Parent.gameObject.SetActive(false);
            Timer = null;
            countdown = false;
        }

        public void ShowTimer_Static(GameObject caller)
        {
            Instance.ShowTimer(caller);
        }

        public static void HideTimer_Static()
        {
            Instance.HideTimer();
        }
    }
}