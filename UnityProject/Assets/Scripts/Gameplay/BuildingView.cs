using System;
using RCR.Interfaces;
using RCR.Patterns;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BuildingView : BaseView<BuildingModel, BuildingController>, ITouchInterface
    {
        private Sprite m_sprite;

        private SpriteRenderer m_renderer;
        //Would I need A Matireal?

        protected override void Awake()
        {
            base.Awake();
            m_sprite = null;
            m_renderer = GetComponent<SpriteRenderer>();
            Model.BuildingTexture.OnValueChanged += OnTextureChanged;
            Model.TimeLeftToBuild.OnValueChanged += OnTimeValueChanged;
        }

        private void OnDisable()
        {
            Model.BuildingTexture.OnValueChanged -= OnTextureChanged;
            Model.TimeLeftToBuild.OnValueChanged -= OnTimeValueChanged;
        }

        private void OnTimeValueChanged(int previousvalue, int currentvalue)
        {
            if (Controller.CanWeBuild())
            {
                
            }
            throw new System.NotImplementedException();
            //TODO Implement with Progress Bar for Building and put some form of check in place such as CheckIfWeCanBuild/Upgrade
        }

        private void OnTextureChanged(Texture2D previousvalue, Texture2D currentvalue)
        {
            if (!Equals(previousvalue, currentvalue))
            {
                m_sprite = Sprite.Create(currentvalue, new Rect(0,0, currentvalue.width, currentvalue.height), new Vector2(0.5f,0.5f));
                m_renderer.sprite = m_sprite;
            }
        }


        public void On_Touched()
        {
            throw new System.NotImplementedException();
        }
        
        
    }
}