using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cinemachine;
using Core.models.maths;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using RuntimeModels;
using UnityEngine;
using Utility;


namespace Core.Services.Marina
{
    public class ApplyCameraRestriction : IAsyncMarinaDecorator
    {

        private readonly PolygonCollider2D _worldboundariesPolygon;
        private readonly PolygonCollider2D _outsidePolygon;
        private readonly Transform _virtualCameraFollower;
        private readonly CinemachineConfiner2D _cinemachineConfiner2D;

        public ApplyCameraRestriction(PolygonCollider2D worldboundariesPolygon,PolygonCollider2D outsidePolygon, Transform virtualCamera, CinemachineConfiner2D confiner2D)
        {
            _worldboundariesPolygon = worldboundariesPolygon;
            _outsidePolygon = outsidePolygon;
            _virtualCameraFollower = virtualCamera;
            _cinemachineConfiner2D = confiner2D;
        }
        
        public async UniTask<MarinaResponseContext> SendAsync(MarinaRequestContext context, CancellationToken token, Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next)
        {
            //Before response comeback
            MarinaResponseContext responseContext = await next(context, token);
            //after response comeback
            
            //Get Outside Polygon points
            List<Vector3> OutviewPoints = new List<Vector3>()
            {
                context.IsometricOutofView.CellToWorld(context.IsometricOutofView.cellBounds.max),
                context.IsometricOutofView.CellToWorld(new Vector3Int(context.IsometricOutofView.cellBounds.max.x,
                    context.IsometricOutofView.cellBounds.min.y)),
                context.IsometricOutofView.CellToWorld(context.IsometricOutofView.cellBounds.min),
                context.IsometricOutofView.CellToWorld(new Vector3Int(context.IsometricOutofView.cellBounds.min.x,
                    context.IsometricOutofView.cellBounds.max.y)),
            };
            _outsidePolygon.points = OutviewPoints.ConvertAll(point => new Vector2(point.x, point.y)).ToArray();

            //Get list of points from graham scan
            Vector2[] points = MathHelper
                .ConvexHull_GrahamScan2D(responseContext.RuntimeUserMap.GetallWorldPlotMinMaxPoints2D()).ToArray();
            _worldboundariesPolygon.points = points;
            _outsidePolygon.SubtractPolygon(_worldboundariesPolygon);
            
            //find plot 0,0 put camera in the middle of there
            RuntimeUserPlot first_plot = responseContext.RuntimeUserMap.RuntimeUserPlots.First(plot =>
                plot._plot.Plot_index_X == 0 &&
                plot._plot.Plot_index_Y == 0);
            Vector3Int cameraPoint = first_plot.MinPoint + ((first_plot.MaxPoint - first_plot.MinPoint) / 2);
            
            cameraPoint.z = -10;
            _virtualCameraFollower.transform.position = context.IsometricTilemap.CellToWorld(cameraPoint);
            
            
            _cinemachineConfiner2D.m_BoundingShape2D = _worldboundariesPolygon;
            
            return responseContext;
        }

    }
}