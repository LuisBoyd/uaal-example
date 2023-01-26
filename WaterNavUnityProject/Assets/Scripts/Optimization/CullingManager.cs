using System;
using System.Collections.Generic;
using System.Linq;
using Patterns.Factory;
using Patterns.ObjectPooling;
using RCR.BaseClasses;
using RCR.Utilities;
using UI.uGUI;
using UnityEngine;

namespace RCR.Settings.Optimization
{
    public class CullingManager : Singelton<CullingManager>
    {
        private CullingGroup localCullingGroup;

        private readonly Dictionary<Guid, int> m_InExistence =
            new Dictionary<Guid, int>(); //Stores Index in the Max_BoundingSpheres array

        [SerializeField] 
        private int PreWarm_Count = 300;

        private CullingComponent[] Max_CullingComponents;
        private BoundingSphere[] Max_BoundingSpheres;
       
        
        protected override void Awake()
        {
            base.Awake(); //Init Collections
            localCullingGroup = new CullingGroup();
            localCullingGroup.onStateChanged = OnStateChanged;
            localCullingGroup.targetCamera = Camera.main;
            /*Used for bands of distance so for example 0 - 10 distance band
             I could use a different LOD or whatever to the 10 - 20 band
             */
            localCullingGroup.SetBoundingDistances(new float[]
            {
                0, 10
            });

            Max_BoundingSpheres = new BoundingSphere[PreWarm_Count];
            Max_CullingComponents = new CullingComponent[PreWarm_Count];
            
            localCullingGroup.SetBoundingSphereCount(PreWarm_Count);
            localCullingGroup.SetBoundingSpheres(Max_BoundingSpheres);
            //this will be the centre of the camera
            localCullingGroup.SetDistanceReferencePoint(Camera.main.transform); 
            
        }

        private void FixedUpdate()
        {
            UpdateBoundingSpheres();
        }

        public Guid AddCullingObject(CullingComponent cullingComponent)
        {
            if (cullingComponent.ID != Guid.Empty && !m_InExistence.ContainsKey(cullingComponent.ID))
            {
                Guid newGUID = Guid.NewGuid();
                m_InExistence.Add(newGUID, m_InExistence.Count);
                Max_BoundingSpheres[m_InExistence[newGUID]] = cullingComponent.BoundingSphere;
                return newGUID;
            }
            return Guid.Empty;
        }
        public void RemoveCullingObject(Guid id)
        {
            if (id != Guid.Empty && m_InExistence.ContainsKey(id))
            {
                Max_BoundingSpheres[m_InExistence[id]].position = Vector3.zero;
                Max_BoundingSpheres[m_InExistence[id]].radius = 0f;
                m_InExistence.Remove(id);
            }
        }
        private void OnStateChanged(CullingGroupEvent evnt)
        {
            foreach (CullingComponent component in Max_CullingComponents)
            {
                if (LBUtilities.AssertNull(component))
                {
                    component.ChangeInCulling(evnt);
                }
            }
        }

        private void UpdateBoundingSpheres()
        {
            for (int i = 0; i < Max_CullingComponents.Length; i++)
            {
                if (!LBUtilities.AssertNull(Max_CullingComponents[i]))
                {
                    Max_BoundingSpheres[i] = Max_CullingComponents[i].BoundingSphere;
                }
                else
                {
                    Max_BoundingSpheres[i].position = Vector3.zero;
                    Max_BoundingSpheres[i].radius = 0f;
                }
            }
        }

      
    }
}