using System;
using Core.Services.Network;
using Core3.MonoBehaviors;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using UnityEngine;
using VContainer;

namespace Core.Services.Gameplay
{
    public abstract class BaseBuyService : BaseMonoBehavior, IBuyService
    {
        [Header("BroadCasting On")]
        [SerializeField]protected EventRelay OnSuccessfulBuyEvent;
        [SerializeField]protected EventRelay OnFailedBuyEvent;
        
        
        

        #region Other Interacting Services
        protected  NetworkClient _networkClient{ get; private set; }
        protected User _user { get; private set; }
        #endregion


        [Inject]
        protected void InjectServices(NetworkClient networkClient, User user)
        {
            _user = user;
            _networkClient = networkClient;
        }

        public abstract void Buy(int itemID);

        public abstract bool CanBuy(int itemID);
        public abstract UniTask BuyAsync(int itemID);
        public abstract UniTask<bool> CanBuyAsync(int itemID);
        

    }
}