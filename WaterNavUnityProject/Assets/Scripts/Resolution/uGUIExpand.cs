using System;
using UnityEngine;

namespace Resolution
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class uGUIExpand : MonoBehaviour
    {
        private Canvas m_canvas;
        private RectTransform m_canvasRectTransform;
        private RectTransform _transform;
        private Vector2 _orginalSize;
        [SerializeField] 
        private bool Match_Height;
        [SerializeField] 
        private bool Match_Width;
        
        private void Awake()
        {
            m_canvas = GetComponentInParent<Canvas>();
            if(m_canvas == null)
                return;
            m_canvasRectTransform = m_canvas.GetComponent<RectTransform>();
            _transform = GetComponent<RectTransform>();
            _orginalSize = new Vector2(_transform.rect.width, _transform.rect.height);
            ComputeSize();
        }

        private void ComputeSize()
        {
            if(m_canvas == null)
                return;
            if (!Match_Height && !Match_Width)
            {
                _transform.sizeDelta = _orginalSize;
                return;
            }
            
            Vector2 NewSize = Vector2.zero;
            NewSize.x = Match_Width ? m_canvasRectTransform.rect.width : _orginalSize.x;
            NewSize.y = Match_Height ? m_canvasRectTransform.rect.height : _orginalSize.y;
            _transform.sizeDelta = NewSize;
        }

        private void Update()
        {
#if UNITY_EDITOR
            ComputeSize();
#endif
        }

    }
}