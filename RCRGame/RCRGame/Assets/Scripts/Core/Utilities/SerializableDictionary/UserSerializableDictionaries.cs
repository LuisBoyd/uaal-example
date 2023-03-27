using System;
using RCRCoreLib.Core.Systems;
using RCRCoreLib.Core.Systems.Tutorial.Enum;
using RCRCoreLib.Core.Tiles;
using RCRCoreLib.Core.UI.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Utilities.SerializableDictionary
{
    
    [Serializable]
    public class TutBtnDictionary : SerializableDictionary<TutorialBtnType, Button>{}
    
    [Serializable]
    public class WorldTileDictionary : SerializableDictionary<TileType, WorldTile>{}
    
    [Serializable]
    public class UIrectDictionary : SerializableDictionary<UIType, UIMenuRoot>{}
    
    [Serializable]
    public class StructureCategoryDictionary : SerializableDictionary<CategoryShopBtn, RectTransform>{}
    
    [Serializable]
    public class SystemDictionary : SerializableDictionary<SystemType,Button>{}

}