using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BlackBoard.editor
{
    /// <summary>
    /// Represents a combination of a label and a image (rounded rectangle) indicating the type of the keylabel.
    /// </summary>
    public class Keylabel: VisualElement
    {
        public static readonly string ussClassname = "key-label";

        // Initialize the Collection with ussClass names that are relevant to the tpye.
        public static readonly IDictionary<BlackBoardKeyType, string> TypeUssName =
            new Dictionary<BlackBoardKeyType, string>()
            {
                { BlackBoardKeyType.Int, "" },
                { BlackBoardKeyType.Float, "" },
                { BlackBoardKeyType.Bool, "" },
                { BlackBoardKeyType.String, "" },
                { BlackBoardKeyType.Vector3, "" },
                { BlackBoardKeyType.GameObject, "" },
                { BlackBoardKeyType.Object, "" },
            };

        public Keylabel()
        {
            //Style the control overall.
            AddToClassList(ussClassname);

            //The KeyIcon will be the one targeted based on what type the key is.
            var KeyIcon = new VisualElement();
            
        }
    }
}