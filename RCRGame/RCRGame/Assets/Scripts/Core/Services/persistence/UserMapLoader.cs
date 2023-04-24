using System;
using System.Collections.Generic;
using System.Linq;
using Core.models;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using UnityEngine;

namespace Core.Services.persistence
{
    public class UserMapLoader : BaseLoader<UserMap>
    {
        private readonly UserMariana _userMariana;

        public UserMapLoader(UserMariana userMariana,string localPath, NetworkClient client) : base(localPath,  client)
        {
            _userMariana = userMariana;
        }

        protected override async UniTask<DateTime> GetRemoteLastTimeModifiedUtc()
        {
            try
            {
                return await _networkClient.PostAsync<DateTime>("GetUserMapLastSaved.php",
                    new
                    {
                        UserID = _userMariana.system_user_id,
                        MarinaID = _userMariana.marinaId
                    });
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
        }

        protected override async UniTask<UserMap> LoadFromRemoteLocation()
        {
            //TODO in PHP have a safe fail for accidental deletion of plot's of land.
            UserMap loadedMap = new UserMap();
            List<Plot> user_plots = await _networkClient.PostAsync<List<Plot>>("GetPlayerMapPlots.php",
                new
                {
                    UserID = _userMariana.system_user_id,
                    MarinaID = _userMariana.marinaId
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
            loadedMap.UserMariana = _userMariana;
            return loadedMap;
        }

        protected override UniTask UploadMostRecent(UserMap mostRecent)
        {
            throw new NotImplementedException();
        }

        public override async UniTask<UserMap> LoadMostRecent()
        {
            if (DoesLocalExist())
            {
                DateTime localLastModified = GetLocalLastTimeModifiedUtc();
                DateTime remoteLastModified = await GetRemoteLastTimeModifiedUtc();
                Int32 dateTimecomparision = DateTime.Compare(localLastModified, remoteLastModified);
                if (dateTimecomparision > 0)
                {
                    UserMap value = await LoadFromLocalFile();
                    await UploadMostRecent(value);
                    return value;
                }
                else
                {
                    return await LoadFromRemoteLocation();
                }
            }
            return await LoadFromRemoteLocation();
        }
    }
}