using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core3.SciptableObjects
{
    [CreateAssetMenu(fileName = "newScene", menuName = "RCR/ScriptableObject/Scene SO", order = 0)]
    public class SceneSO : BaseScriptableObject
    {
        [EnumToggleButtons] 
        public SceneType sceneType;

        public AssetReference sceneReference;

        [InlineEditor(InlineEditorModes.FullEditor)]
        public AudioCueSO musicTrack;
        
        public enum SceneType
        {
            //Menu Scene are things like LoginScene, ManagementHub scenes and Loading Scene
            Menu, 
            /* Persistant Manager Scenes are these
             *  Gameplay Managers Scene, VisitGameplay Managers,
             *  Persistant manager
             */ 
            PersistantManager,
            /*
             *  Gameplay scenes are like Gameplay, Visit Scene
             */
            Gameplay,
            //Initialization scene is just initialization scene.
            Initialization,
            //Art scenes are scenes to do with VFX and such to make the game look good
            Art
        }
    }
}