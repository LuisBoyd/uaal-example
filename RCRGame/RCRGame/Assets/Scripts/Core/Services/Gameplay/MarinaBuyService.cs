using System;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace Core.Services.Gameplay
{
    public class MarinaBuyService : BaseBuyService
    {

       [Header("Listening To")]
       [SerializeField]private IntEventChannelSO _buyEventChannel;
 

        #region Fields
        private UnityAction<int> BuyAction;
        #endregion
        

        private void OnEnable()
        {
            BuyAction = UniTaskHelper.UnityAction<int>(BuyAsync);
            _buyEventChannel.onEventRaised += BuyAction;
        }

        private void OnDisable()
        {
            _buyEventChannel.onEventRaised -= BuyAction;
        }

        public override void Buy(int itemID)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanBuy(int itemID)
        {
            throw new System.NotImplementedException();
        }

        public override async UniTask BuyAsync(int itemID)
        {
            if (!await CanBuyAsync(itemID))
            {
                OnFailedBuyEvent.RaiseEvent();
                return;
            }
            try
            {
                Mariana ToBuyMarina = await _networkClient.PostAsync<Mariana>("GetMarinaByID.php", new
                {
                    MarinaID = itemID
                });
                await _networkClient.PostAsync("BuyMarina.php", new
                {
                    MarinaID = itemID,
                    UserID = _user.User_id
                });
                _user.Freemium_Currency -= ToBuyMarina.BuyCost;
                await _networkClient.PostAsync("UpdateUserFreeCurrency.php", new
                {
                    UserID = _user.User_id,
                    FreemiumCurrency = _user.Freemium_Currency
                });
            }
            catch (Exception e)
            {
                return;
            }
            OnSuccessfulBuyEvent.RaiseEvent();
        }

        public override async UniTask<bool> CanBuyAsync(int itemID)
        {
           
            //1. need to find out how much the cost of the marina is.
            //2. compare that cost with how much the user currently has.
            //3. if it's more or equal allow them to buy it.
            //4. if it's less than do not allow them to buy it.
            //6. otherwise return true.
            
            //in the case of this service the itemID should be the Marina ID.
            //so I need to make a backend that looks up that id can get's its cost.
            try
            {
                Mariana RequestedMarina = await _networkClient.PostAsync<Mariana>("GetMarinaByID.php", new
                {
                    MarinaID = itemID
                });
                if (RequestedMarina.BuyCost <= _user.Freemium_Currency)
                    return true; //TODO the Marina should have a currency Type attached to it that way I can filter that cost aka premium currency cost
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
            return false;
        }
        
    }
}