using UnityEngine;

namespace WaterNavTiled
{
    public class ImageLayer : Layer
    {
        [SerializeField]private string Image;
        [SerializeField] private bool repeatX;
        [SerializeField] private bool repeatY;
        [SerializeField] private string transparentColor;
    }
}