using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using Pathfinding;
using UnityEngine;

namespace Core.Services.Marina
{
    public class ApplyAStarGrid : IAsyncMarinaDecorator
    {

        private AstarPath _astarPath;
        private AstarData _astarData;
        private GridGraph _gridGraph;

        private const float NodeSize = 0.707f;
        private const float RotateAngle = 45f;
        private const float IsometricAngle = 60f;

        public ApplyAStarGrid(AstarPath astarPath)
        {
            _astarPath = astarPath;
            _astarData = _astarPath.data;
            _gridGraph = _astarData.AddGraph(typeof(GridGraph)) as GridGraph;
        }
        
        public async UniTask<MarinaResponseContext> SendAsync(MarinaRequestContext context, CancellationToken token, Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next)
        {
            var responseContext = await next(context, token);
            
            _gridGraph.SetGridShape(InspectorGridMode.IsometricGrid);
            _gridGraph.isometricAngle = IsometricAngle;
            _gridGraph.rotation = new Vector3(-RotateAngle,270, 90);
            Debug.Log(_gridGraph.rotation);
            //_gridGraph.is
            _gridGraph.center = context.IsometricTilemap.localBounds.center;
            int width = context.IsometricTilemap.cellBounds.xMax - context.IsometricTilemap.cellBounds.xMin;
            int depth = context.IsometricTilemap.cellBounds.yMax - context.IsometricTilemap.cellBounds.yMin;
            _gridGraph.SetDimensions(width, depth, NodeSize);
            _astarPath.Scan();
            Debug.Log("Finished Scan");
            return responseContext;
        }
    }
}