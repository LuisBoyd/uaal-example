using System;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class CardViewFactory : MonoBehaviour
    {
        private ScrollRect ScrollRect;
       // private GameObject

        private void Awake()
        {
            ScrollRect = GetComponent<ScrollRect>();
        }
        
        
        
    }
}