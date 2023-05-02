using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.models;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using RuntimeModels;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Services.Marina
{
    public class MarinaBuildPipeline : IAsyncMarinaDecorator
    {
        private readonly Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next;
        private readonly IAsyncMarinaDecorator[] _decorators;
        private readonly InternalSetting Setting;
        private readonly NetworkClient _networkClient;
        private readonly UserMariana _userMariana;

        public MarinaBuildPipeline(InternalSetting setting,NetworkClient networkClient, UserMariana userMariana, params IAsyncMarinaDecorator[] decorators)
        {
            this.next = InvokeRecursive;
            this._networkClient = networkClient;
            this._userMariana = userMariana;
            this.Setting = setting;
            this._decorators = new IAsyncMarinaDecorator[decorators.Length + 1];
            Array.Copy(decorators, this._decorators, decorators.Length);
            this._decorators[^1] = this;
        }

        public async UniTask<RuntimeUserMap> BuildMarina(Tilemap tilemap,Tilemap outOfViewTilemap,int marinaID,int UserID, CancellationToken token = default)
        {
            var request = new MarinaRequestContext(tilemap, outOfViewTilemap,marinaID, UserID, _decorators);
            var response = await InvokeRecursive(request, token);
            return response.RuntimeUserMap;
        }

        UniTask<MarinaResponseContext> InvokeRecursive(MarinaRequestContext context, CancellationToken token)
        {
            return context.GetNextDecorator().SendAsync(context, token, next);
        }

        async UniTask<MarinaResponseContext> IAsyncMarinaDecorator.SendAsync(MarinaRequestContext context,
            CancellationToken token, Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> _)
        {
            UserMap loadedMap = null;
            var linkToken = CancellationTokenSource.CreateLinkedTokenSource(token);
            try
            {
                loadedMap = await LoadFromRemoteLocation(context.MarinaID,
                    context.UserID);

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
            finally
            {
                if (!linkToken.IsCancellationRequested)
                {
                    linkToken.Cancel();
                }
            }
            return new MarinaResponseContext(true, false, new RuntimeUserMap(loadedMap, context.IsometricTilemap));
        }
        
        private async UniTask<UserMap> LoadFromRemoteLocation(int marinaID, int userID)
        {
            UserMap loadedMap = new UserMap();
            loadedMap.UserMariana = new UserMariana();
            List<Plot> user_plots = await _networkClient.PostAsync<List<Plot>>("GetPlayerMapPlots.php",
                new
                {
                    UserID = userID,
                    MarinaID = marinaID
                });
            int[] plotIds = user_plots.Select(plot => plot.Id).ToArray();
            List<Structure> user_structures = await _networkClient.PostAsync<List<Structure>>(
                "GetPlayerMapStructures.php",
                new
                {
                    Ids = plotIds
                });
            loadedMap.Plots = user_plots;
            loadedMap.Structures = user_structures;
            loadedMap.UserMariana.system_user_id = _userMariana.system_user_id;
            loadedMap.UserMariana.marinaId = _userMariana.marinaId;
            return loadedMap;
        }

    }
}