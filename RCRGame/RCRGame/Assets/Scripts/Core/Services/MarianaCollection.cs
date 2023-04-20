using System;
using System.Collections.Generic;
using System.Threading;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UI;
using UnityEngine;
using VContainer.Unity;

namespace DefaultNamespace.Core.models
{
    /// <summary>
    /// IasyncStartable Collected all the Marina Data from the DB then this class can pass all that information onto
    /// whatever needs it
    /// </summary>
    public class MarianaCollection : IAsyncStartable
    {
        public List<Mariana> ReadonlyMarinaList { get; private set; }
        
        //Injected Values
        private readonly User _user;
        private readonly NetworkClient _networkClient;
        private readonly EventRelay _onRetrievedUpdatedMarinaSet;

        public MarianaCollection(User userInfo, NetworkClient networkClient,
            EventRelay newMarinaSetNotifier)
        {
            _user = userInfo;
            _networkClient = networkClient;
            _onRetrievedUpdatedMarinaSet = newMarinaSetNotifier;
            ReadonlyMarinaList = new List<Mariana>();
        }
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            ReadonlyMarinaList = await GetUpdatedMarinaList();
            _onRetrievedUpdatedMarinaSet.RaiseEvent();
        }

        public async UniTask UpdateMarinaSet()
        {
            ReadonlyMarinaList = await GetUpdatedMarinaList();
            _onRetrievedUpdatedMarinaSet.RaiseEvent();
        }

        private async UniTask<List<Mariana>> GetUpdatedMarinaList()
        {
            List<Mariana> list = new List<Mariana>();
            try
            {
                list = await _networkClient.PostAsync<List<Mariana>>("GetAllOwnedMarinaList.php", new
                {
                    UserID = _user.User_id,
                });
            }
            catch (Exception e)
            {
                throw new OperationCanceledException();
            }
            return list;
        }
    }
}