using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            IEnumerable<Line> _plotlines = responseContext.RuntimeUserMap.GetAllWorldPlotLines();
            _plotlines = _plotlines.RemoveDuplicateLines();

            foreach (Line plotLine in _plotlines)
            {
                Debug.DrawLine(plotLine._startPoint, plotLine._endPoint, Color.red, 100f);
            }
  
            
            return responseContext;
        }
        
    }
}