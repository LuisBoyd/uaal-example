using UnityEngine;

namespace RCRCoreLib.Core.Systems.Unlockable
{
    public class UnlockablePlaceables : Unlockable
    {
        public virtual string prefabPath  //Where is the location for the prefab.
        {
            get;
            protected set;
        }

        public virtual string SpriteIconPath //Where is the location of the IconSprite
        {
            get;
            protected set;
        }

        public Sprite Icon { get; protected set; }
        public GameObject Prefab { get; protected set; }

        public void LoadIn(GameObject prefab, Sprite icon)
        {
            Icon = icon;
            Prefab = prefab;
        }
    }
}