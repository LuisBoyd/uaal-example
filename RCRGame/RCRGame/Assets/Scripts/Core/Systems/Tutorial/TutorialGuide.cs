using System;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.CameraLib;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.Input;
using RCRCoreLib.Core.Events.TutorialEvents;
using RCRCoreLib.Core.Node.Graphs;
using RCRCoreLib.Core.Node.Nodes;
using RCRCoreLib.Core.Systems.Tutorial.Enum;
using RCRCoreLib.Core.UI;
using RCRCoreLib.Core.Utilities.SerializableDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnmaskForUGUI;
using XNode;

namespace RCRCoreLib.Core.Systems.Tutorial
{
    [RequireComponent(typeof(RectTransform))]
    public class TutorialGuide : Singelton<TutorialGuide>, ISystem
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
        private RectTransform TutorialPointer;
        
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
        private Unmask unmask;
        public Unmask Unmask
        {
            get => unmask;
        }

        public GameObject Unmask_Parent
        {
            get
            {
                return unmask.transform.parent.gameObject;
            }
        }

        [SerializeField] 
        private TutBtnDictionary m_tutBtnDictionary = null;
        public IDictionary<TutorialBtnType, Button> TutBtnDictionary
        {
            get { return m_tutBtnDictionary; }
            set{m_tutBtnDictionary.CopyFrom(value);}
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
            GameManager.Instance.RegisterSystem(SystemType.TutorialSystem, this);
            EventManager.Instance.AddListener<StartTutorialEvent>(On_startTutorialRequested);
            EventManager.Instance.AddListener<EndTutorialEvent>(On_GraphFinished);
            EventManager.Instance.AddListener<ShowTutorialInterface>(On_showTutorialinterface);
            EventManager.Instance.AddListener<HideTutorialinterface>(On_hideTutorialinterace);
            foreach (Transform child in OwnTransfrom)
            {
                child.gameObject.SetActive(false);
            }
            EventManager.Instance.QueueEvent(new StartTutorialEvent(TutorialType.WelcomeNewPerson)); //TODO This is a test function
            DisableOnStartUp();
        }

        private void DisableOnStartUp()
        {
            Unmask_Parent.SetActive(false);
            
        }

        public void EnableUnmask(RectTransform toFit)
        {
            Unmask_Parent.SetActive(true);
            Unmask.fitTarget = toFit;
        }
        public LTDescr EnableUnmask(RectTransform toFit, Vector2 fingerPos, float fingerRotation,float toPos ,bool horizontal = false)
        {
            Unmask_Parent.SetActive(true);
            Unmask.fitTarget = toFit;

            TutorialPointer.localPosition = fingerPos;
            TutorialPointer.rotation = Quaternion.Euler(0,0, fingerRotation);
            LTDescr descr = null;
            if (horizontal)
                descr = LeanTween.moveX(TutorialPointer, toPos, 1.2f).setLoopPingPong();
            else
            {
                descr = LeanTween.moveY(TutorialPointer, toPos, 1.2f).setLoopPingPong();
            }
            return descr;
        }

        public void DisableUnmask()
        {
            Unmask_Parent.SetActive(false);
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

            DisableUnmask();
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
            if (!e.SkipOrContinue)
            {
                SkipButton.gameObject.SetActive(false);
                ContinueButton.gameObject.SetActive(false);
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
            PanZoom.Instance.MoveEnabled = false;
            foreach (Button button in TutBtnDictionary.Values)
            {
                button.interactable = false;
            }
            // EventManager.Instance.QueueEvent(new SetInputState(false));
            Graph.Start();
        }

        private void On_GraphFinished(EndTutorialEvent evnt)
        {
            // EventManager.Instance.QueueEvent(new SetInputState(true));
            PanZoom.Instance.MoveEnabled = true;
            foreach (Button button in TutBtnDictionary.Values)
            {
                button.interactable = true;
            }
        }

        public void EnableSystem()
        {
            throw new NotImplementedException();
        }

        public void DisableSystem()
        {
            throw new NotImplementedException();
        }
    }
}