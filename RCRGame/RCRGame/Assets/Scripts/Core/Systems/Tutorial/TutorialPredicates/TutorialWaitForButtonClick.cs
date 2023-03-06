using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates
{
    public class TutorialWaitForButtonClick : TutorialAction
    {
        private Button m_toBeClicked;
        private bool BtnClicked;

        public override bool keepWaiting
        {
            get
            {
                return !BtnClicked; //Return KeepWaiting = true (means keep waiting to complete), false means we are done
            }
        }

        public TutorialWaitForButtonClick(Button btn)
        {
            BtnClicked = false;
            m_toBeClicked = btn;
            m_toBeClicked.onClick.AddListener(OnButtonClicked);
        }
        

        private void OnButtonClicked()
        {
            BtnClicked = true;
            m_toBeClicked.onClick.RemoveListener(OnButtonClicked);
            m_toBeClicked = null;
        }

    }
}