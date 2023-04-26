using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using UnityEngine;
using Utility;

namespace Core.Services.Marina
{
    public class ApplyCameraRestriction : IAsyncMarinaDecorator
    {

        private readonly PolygonCollider2D _polygonCollider2D;

        public ApplyCameraRestriction(PolygonCollider2D polygonCollider2D)
        {
            _polygonCollider2D = polygonCollider2D;
        }
        
        public async UniTask<MarinaResponseContext> SendAsync(MarinaRequestContext context, CancellationToken token, Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next)
        {
            //Before response comeback
            MarinaResponseContext responseContext = await next(context, token);
            //after response comeback
            
            //Get all min Max Points of map
            Vector2[] PlotMinMaxPoints = responseContext.RuntimeUserMap.GetallWorldPlotMinMaxPoints2D();
           
            
            //Generate Points with No duplicates
            var sortedPoints = PlotMinMaxPoints.RemoveEntryWithXDuplicates(3).ToArray();
            
            //find the average vertex
            Vector2 average = Vector2.zero;
            for (int i = 0; i < sortedPoints.Length; i++)
            {
                average += sortedPoints[i];
            }
            average /= sortedPoints.Length;
            
            //Get points sorted by the polar angle of the average
            var sortedPointsByPolarAngle = sortedPoints.SortByPolarAngle(average).ToArray();
            
            //This should generate a generated polygon 2d.
            _polygonCollider2D.points = sortedPointsByPolarAngle;
            return responseContext;
        }
    }
}