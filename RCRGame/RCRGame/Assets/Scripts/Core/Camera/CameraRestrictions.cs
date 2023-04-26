using System;
using Core.models;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Core.Camera
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class CameraRestrictions : BaseMonoBehavior
    {
        [Title("Configurations", TitleAlignment = TitleAlignments.Centered)] 
        [SerializeField] [ReadOnly]private PolygonCollider2D MapBoundry;

        #region InjectValues
        private UserMap _userMap;
        [Inject]
        private void InjectValues(UserMap userMap)
        {
            _userMap = userMap;
        }
        #endregion
        private void Awake()
        {
            MapBoundry = GetComponent<PolygonCollider2D>();
        }
        
        
        
    }
}