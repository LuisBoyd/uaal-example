using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Systems
{
    public class TilePaintingSystem : Singelton<TilePaintingSystem>
    {
        private bool isOpened;
        private const string KEYWORD_SHADER = "_OUTLINE_ENABLED";

        [SerializeField] 
        private TilemapRenderer BackGroundTilemapRenderer;
        private LocalKeyword OutlineEnabledKeyWord;

        [SerializeField] 
        private RectTransform TilePlacementMenu;
        [SerializeField] 
        private RectTransform ConstructionTab;

        private Material GridOutlineMatireal
        {
            get
            {
                //Returns back the Copy Material so if any other objects use the same
                //Matireal any Changes I make here will not happen to all gameObjects with
                //This Matireal only the gameObject this renderer is attached to.
                return BackGroundTilemapRenderer.material; 
            }
        }

        protected override void Awake()
        {
            base.Awake();
            OutlineEnabledKeyWord = new LocalKeyword(GridOutlineMatireal.shader, KEYWORD_SHADER);
        }

        public void OnTilePaint_Btn_Clicked()
        {
            if (!isOpened)
            {
                isOpened = true;
                GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, isOpened);
                Debug.Log("Open");
                TilePlacementMenu.gameObject.SetActive(true);
                ConstructionTab.gameObject.SetActive(false);
                //TODO in case object is not enabled at start compensate for this.
            }
            else
            {
                isOpened = false;
                Debug.Log("Close");
                GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, isOpened);
                TilePlacementMenu.gameObject.SetActive(false);
                ConstructionTab.gameObject.SetActive(true);
            }
        }
    }
}