
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI
{
    public class CutOutmaskUI : Image
    {
        public override Material materialForRendering
        {
            get
            {
                Material material = new Material(base.materialForRendering);
                material.SetFloat("_StencilComp",(float)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}