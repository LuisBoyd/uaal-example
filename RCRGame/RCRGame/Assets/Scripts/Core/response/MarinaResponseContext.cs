using Core.models;
using DefaultNamespace.Core.requests;
using RuntimeModels;
using UnityEngine;

namespace DefaultNamespace.Core.response
{
    public class MarinaResponseContext
    {
        public bool Success { get; set; }
        public bool WasLoadedLocally { get; set; }
        
        public UserMap UserMap { get; private set; }
        
        public RuntimeUserMap RuntimeUserMap { get; set; }
        
        public Vector2 CameraStartPoint { get; set; }

        public MarinaResponseContext(bool success, bool wasLoadedLocally,RuntimeUserMap runtimeUserMap)
        {
            Success = success;
            WasLoadedLocally = wasLoadedLocally;
            RuntimeUserMap = runtimeUserMap;
            UserMap = RuntimeUserMap.UserMap;
        }
    }
}