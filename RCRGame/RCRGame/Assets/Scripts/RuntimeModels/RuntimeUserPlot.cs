using UnityEngine;

namespace RuntimeModels
{
    public class RuntimeUserPlot
    {
        private readonly Texture2D _plotTexture;
        private readonly Plot _plot;
        private Color32[] _Pixels;
        public Vector3Int MaxPoint { get; private set; }
        public Vector3Int MinPoint { get; private set; }
        public RuntimeUserPlot(Plot plot)
        {
            _plot = plot;
        }

        public RuntimeUserPlot(Plot plot, Color32[] pixels)
        {
            _Pixels = pixels;
            _plot = plot;
        }
        
        public RuntimeUserPlot(Plot plot, Color32[] pixels, Vector3Int minpoint, Vector3Int maxpoint)
        {
            _Pixels = pixels;
            _plot = plot;
            MinPoint = minpoint;
            MaxPoint = maxpoint;
        }
    }
}