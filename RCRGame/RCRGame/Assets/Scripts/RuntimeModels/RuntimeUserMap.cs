using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Core.models;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RuntimeModels
{
    public class RuntimeUserMap : INotifyPropertyChanged
    {
        private readonly Tilemap _isometricTileMap;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDictionary<Vector2Int, RuntimeUserPlot> _userPlots;
        private readonly TilePlacementEventChannelSO _tilePlacementEventChannelSo;
        public UserMap UserMap { get; private set; }

        public RuntimeUserMap(UserMap userMap, Tilemap isometricTileMap)
        {
            UserMap = userMap;
            _isometricTileMap = isometricTileMap;
            _userPlots = new Dictionary<Vector2Int, RuntimeUserPlot>();
        }

        public void AddPlot(int xIndex, int yIndex, Plot plot) =>
            _userPlots.Add(new Vector2Int(xIndex, yIndex), new RuntimeUserPlot(plot));
        public void AddPlot(int xIndex, int yIndex, RuntimeUserPlot plot) =>
            _userPlots.Add(new Vector2Int(xIndex, yIndex), plot);

        public Vector3[] GetallWorldPlotMinMaxPoints()
        {
            List<Vector3> points = new List<Vector3>();
            foreach (RuntimeUserPlot plot in _userPlots.Values)
            {
                points.Add(_isometricTileMap.CellToWorld(plot.MinPoint)); //Min
                points.Add(_isometricTileMap.CellToWorld(new Vector3Int(plot.MinPoint.x, plot.MaxPoint.y))); //Xmin, Ymax
                points.Add(_isometricTileMap.CellToWorld(plot.MaxPoint)); //Max
                points.Add(_isometricTileMap.CellToWorld(new Vector3Int(plot.MaxPoint.x, plot.MinPoint.y))); //Xmax, Ymin
            }
            return points.ToArray();
        }
        public Vector2[] GetallWorldPlotMinMaxPoints2D()
        {
            List<Vector2> points = new List<Vector2>();
            foreach (RuntimeUserPlot plot in _userPlots.Values)
            {
                points.Add(_isometricTileMap.CellToWorld(plot.MinPoint)); //Min
                points.Add(_isometricTileMap.CellToWorld(new Vector3Int(plot.MinPoint.x, plot.MaxPoint.y))); //Xmin, Ymax
                points.Add(_isometricTileMap.CellToWorld(plot.MaxPoint)); //Max
                points.Add(_isometricTileMap.CellToWorld(new Vector3Int(plot.MaxPoint.x, plot.MinPoint.y))); //Xmax, Ymin
            }
            return points.ToArray();
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}