using System;
using System.Collections.Generic;
using DefaultNamespace.Events;
using UnityEngine;

namespace CustomUIFramework.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimationTargetEventHandler : MonoBehaviour
    {
        [HideInInspector]
        private StringEventChannelSO animationTriggerSO;
        public StringEventChannelSO AnimationTriggerSO
        {
            get => animationTriggerSO;
        }

        private IDictionary<string, int> string_to_AnimHash;
        private List<String> _invalidStrings;

        private Animator _animator;
        
        private void Awake()
        {
            _invalidStrings = new List<string>();
            string_to_AnimHash = new Dictionary<string, int>();
            _animator = GetComponent<Animator>();
            animationTriggerSO = ScriptableObject.CreateInstance<StringEventChannelSO>();
        }

        private void OnEnable()
        {
            AnimationTriggerSO.onEventRaised += PlayAnimation;
        }

        private void OnDisable()
        {
            AnimationTriggerSO.onEventRaised -= PlayAnimation;
        }

        private void PlayAnimation(string animationName)
        {
            if(_animator == null)
                return;
            if (string_to_AnimHash.TryGetValue(animationName, out var value))
            {
                _animator.Play(value);
            }
            else
            {
                if(_invalidStrings.Contains(animationName))
                    return;
                int hashCode = Animator.StringToHash(animationName);
                if (_animator.HasState(0, hashCode))
                {
                    string_to_AnimHash.Add(animationName, hashCode);
                    _animator.Play(hashCode);
                }
                else
                {
                    _invalidStrings.Add(animationName);
                }
            }
        }
        
    }
}