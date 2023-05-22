using UnityEngine;

namespace Utility
{
    public static class Conditionals
    {

        //Unity Editor and standalone sometimes share some platform values.
#if UNITY_STANDALONE_WIN
        
#endif
        
        //For Shared values across different platforms
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        public static readonly string RstreamingAssetsPathWin = Application.dataPath + "/StreamingAssets/";
        public static readonly string RWstreamingAssetsPathWin = Application.persistentDataPath;
#elif UNITY_STANDALONE_OSX
        public static readonly string RstreamingAssetsPathOSX =
 Application.dataPath + "/Resources/Data/StreamingAssets/";
        public static readonly string RWstreamingAssetsPathOSX = Application.persistentDataPath;
#elif UNITY_ANDROID
//Can't read directly from android check out https://docs.unity3d.com/Manual/StreamingAssets.html
        public static readonly string RstreamingAssetsPathAndroid = Application.streamingAssetsPath;
        public static readonly string RWstreamingAssetsPathAndroid = Application.persistentDataPath;
#elif UNITY_IOS
        public static readonly string RstreamingAssetsPathIOS = Application.dataPath + "/Raw/";
        public static readonly string RWstreamingAssetsPathIOS = Application.persistentDataPath;
#elif UNITY_WEBGL
//Can't read directly from webGL check out https://docs.unity3d.com/Manual/StreamingAssets.html
        public static readonly string RstreamingAssetsPathWebGl = Application.streamingAssetsPath;
        public static readonly string RWstreamingAssetsPathWebGl = Application.persistentDataPath;
#else

#endif
    }
}