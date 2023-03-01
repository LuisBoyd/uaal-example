using System;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.Events.Input;
using RCRCoreLib.Core.Node.Graphs;
using RCRCoreLib.Core.Node.Nodes;
using RCRCoreLib.Core.UI;
using RCRCoreLib.TutorialEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace RCRCoreLib.Core.Systems.Tutorial
{
    [RequireComponent(typeof(RectTransform))]
    public class TutorialGuide : Singelton<TutorialGuide>
    {
        public TutorialGraph Graph
        {
            get => graph;
        }

        [SerializeField] 
        private TutorialGraph graph;

        public bool playOnce = false;
        private bool played = false;
        private RectTransform OwnTransfrom;
        

        [SerializeField] 
        private TextMeshProUGUI tutorialSpeechBubbleText;
        
        [SerializeField] 
        private RectTransform TutorialSpritesContainer;

        [SerializeField] 
        private Button continueButton;

        public Button ContinueButton
        {
            get => continueButton;
        }
        
        [SerializeField] 
        private Button skipButton;

        public Button SkipButton
        {
            get => skipButton;
        }

        [SerializeField] 
        private List<TutorialPair> TutorialStarts 
            = new List<TutorialPair>();

        private Dictionary<TutorialType, BaseTutorialNode> tutorial_Lookup
            = new Dictionary<TutorialType, BaseTutorialNode>();

        protected override void Awake()
        {
            base.Awake();
            OwnTransfrom = GetComponent<RectTransform>();
            foreach (TutorialPair tutorialStart in TutorialStarts)
            {
                tutorial_Lookup.TryAdd(tutorialStart.tutorialType, tutorialStart.tutorialNode);
            }
        }

        private void Start()
        {
            EventManager.Instance.AddListener<StartTutorialEvent>(On_startTutorialRequested);
            EventManager.Instance.AddListener<EndTutorialEvent>(On_GraphFinished);
            EventManager.Instance.AddListener<ShowTutorialInterface>(On_showTutorialinterface);
            EventManager.Instance.AddListener<HideTutorialinterface>(On_hideTutorialinterace);
            foreach (Transform child in OwnTransfrom)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if(Graph != null)
                Graph.Update();
        }

        private void On_hideTutorialinterace(HideTutorialinterface e)
        {
            TutorialSpritesContainer.transform.localScale = Vector3.one;
            tutorialSpeechBubbleText.text = string.Empty;
            // skipButton.gameObject.SetActive(false);
            // continueButton.gameObject.SetActive(false);
            // TutorialSpritesContainer.gameObject.SetActive(false);
            // tutorialSpeechBubbleText.gameObject.SetActive(false);
            foreach (Transform child in OwnTransfrom)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void On_showTutorialinterface(ShowTutorialInterface e)
        {
            Debug.Log("Fired Show Tutorial Ui Event");
            if (e.HorizontalFlipped)
            {
                Vector3 Scale = Vector3.one * -1;
                Scale.y = 1;
                TutorialSpritesContainer.transform.localScale = Scale;
            }
            foreach (Transform child in OwnTransfrom)
            {
                child.gameObject.SetActive(true);
            }
            // skipButton.gameObject.SetActive(true);
            // continueButton.gameObject.SetActive(true);
            // TutorialSpritesContainer.gameObject.SetActive(true);
            // tutorialSpeechBubbleText.gameObject.SetActive(true);
            tutorialSpeechBubbleText.text = e.Message;
            OwnTransfrom.localPosition = e.LocationOnScreen;
        }

        private void On_startTutorialRequested(StartTutorialEvent evnt)
        {
            //TODO some sort of check to see if we have played this one before...
            Graph.startNode = tutorial_Lookup[evnt.type];
            EventManager.Instance.QueueEvent(new SetInputState(false));
            Graph.Start();
        }

        private void On_GraphFinished(EndTutorialEvent evnt)
        {
            EventManager.Instance.QueueEvent(new SetInputState(true));
        }
    }
}