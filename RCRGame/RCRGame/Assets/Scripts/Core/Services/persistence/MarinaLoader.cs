using System;
using System.Collections.Generic;
using Core.models;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.persistence
{
    public class MarinaLoader : BaseLoader<Map>
    {

        private readonly int _userID;
        private readonly int _marianaID;
        public MarinaLoader(int userID,int marianaID,NetworkClient client,string localPath, string remotePath) : base(localPath, remotePath,client)
        {
            _userID = userID;
            _marianaID = marianaID;
        }

        protected override async UniTask<DateTime> GetRemoteLastTimeModifiedUtc()
        {
            try
            {
                return await _networkClient.PostAsync<DateTime>("GetUserMapLastSaved.php",
                    new
                    {
                        UserID = _userID,
                        MarinaID = _marianaID
                    });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }

        protected override async UniTask<Map> LoadFromRemoteLocation()
        {
            Map loadedMap = new Map();
            try
            {
                loadedMap.Plots = await _networkClient.PostAsync<List<Plot>>("GetPlayerMap.php",
                    new
                    {
                        MarinaID = _marianaID,
                        UserID = _userID
                    });

                return loadedMap;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }

        protected override UniTask UploadMostRecent(Map mostRecent)
        {
            throw new NotImplementedException();
        }

        public override UniTask<Map> LoadMostRecent()
        {
            throw new NotImplementedException();
        }
    }
}