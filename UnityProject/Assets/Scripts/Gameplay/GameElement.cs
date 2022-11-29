using System;
using UnityEngine;

namespace RCR.Gameplay
{
    public class GameElement : MonoBehaviour
    {
        public Guid guid;

        protected virtual void Awake()
        {
            guid = Guid.NewGuid();
        }
    }
}