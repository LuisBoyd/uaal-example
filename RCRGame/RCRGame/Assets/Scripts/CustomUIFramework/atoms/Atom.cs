using CustomUIFramework.Animation;
using CustomUIFramework.enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomUIFramework.atoms
{
    /// <summary>
    /// An atom is a small part of the UI e.g a button, label, radio button, image, etc
    /// anything that can really be considered on it's own in terms of a UI.
    /// these atoms can make larger UI pieces which we can call Molecules
    /// this Idea comes from Natalia Rebrova GDC talk 2019 on unified cross project UI.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Atom : MonoBehaviour
    {
        // [SerializeField] [ChildGameObjectsOnly]
        // protected AnimationTargetEventHandler _animationTargetEventHandler;

        [SerializeField] [ChildGameObjectsOnly][Required]
        protected Animator _animator;
    }
}