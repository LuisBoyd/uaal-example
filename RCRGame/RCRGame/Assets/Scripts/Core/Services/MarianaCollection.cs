using System.Collections.Generic;
using System.Threading;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using VContainer.Unity;

namespace DefaultNamespace.Core.models
{
    public class MarianaCollection : IAsyncStartable
    {
        private readonly NetworkClient _networkClient;
        private readonly MarianaListView _view;
        private readonly IList<Mariana> _marianas;

        public MarianaCollection(NetworkClient client, MarianaListView view)
        {
            _networkClient = client;
            _view = view;
            _marianas = new List<Mariana>();
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var response = await _networkClient.GetAsync<IList<Mariana>>("marians.php", null, cancellation);
            Debug.Log(response.Count);
        }
    }
}