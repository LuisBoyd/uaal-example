using Core3.SciptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Animation.AnimationProperties
{
    [CreateAssetMenu(fileName = "new_ButtonAnimationProperty", 
        menuName = "RCR/Animation/Button Animation Property", order = 0)]
    public class ButtonAnimationProperty : GenericBaseScriptableObject<ButtonAnimationProperty>, IAnimationProperty
    {
        [BoxGroup("Animation Property Settings")] 
        [SerializeField]
        private float timescale;
        [BoxGroup("Animation Property Settings")] 
        [SerializeField]
        private LeanTweenType easeType;
        
        public float TimeScale
        {
            get => timescale;
        }
        public LeanTweenType EaseType
        {
            get => easeType;
        }
        
        [BoxGroup("Animation Property Appearance")] 
        [SerializeField]
        private Sprite sprite;
        [BoxGroup("Animation Property Appearance")] 
        [SerializeField]
        private Color color;
        [BoxGroup("Animation Property Appearance")] 
        [SerializeField]
        private float alpha;
        
        public Sprite Sprite
        {
            get => sprite;
        }
        public Color Color
        {
            get => color;
        }
        public float Alpha
        {
            get => alpha;
        }
        
        
        [BoxGroup("Animation Property Transformation")] 
        [SerializeField]
        private Vector3 position;
        [BoxGroup("Animation Property Transformation")] 
        [SerializeField]
        private Vector3 rotation;
        [BoxGroup("Animation Property Transformation")] 
        [SerializeField]
        private Vector3 scale;

        public Vector3 Position
        {
            get => position;
        }
        public Vector3 Rotation
        {
            get => rotation;
        }
        public Vector3 Scale
        {
            get => scale;
        }
        
    }
}