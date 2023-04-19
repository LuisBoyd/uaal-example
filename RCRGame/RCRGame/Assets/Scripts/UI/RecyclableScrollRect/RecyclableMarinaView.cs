using System;
using System.Collections.Generic;
using Core.Services.Network;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
using UnityEngine;
using VContainer;
using System.Linq;

namespace UI.RecyclableScrollRect
{
    public class RecyclableMarinaView : BaseMonoBehavior, IRecyclableScrollRectDataSource
    {
        private User _user;
        private NetworkClient _networkClient;
        private List<UserMariana> _userMarianas;
        private List<Mariana> _allMarinaInENGWALES;

        [SerializeField] 
        private RecyclableScrollRect _recyclableScrollRect;


        [Inject]
        private void InjectUser(User userData, NetworkClient networkClient)
        {
            _user = userData;
            _networkClient = networkClient;
        }
        
        private async void Awake()
        {
            _userMarianas = new List<UserMariana>();
            _recyclableScrollRect.DataSource = this;
            try
            {
                //Get's all user owened marina's
                _userMarianas = await _networkClient.PostAsync<List<UserMariana>>("GetUserMarinas.php", new
                {
                    UserID = _user.User_id
                });
                _allMarinaInENGWALES = await _networkClient.GetAsync<List<Mariana>>("marians.php", null);
            }
            catch (Exception e)
            {
                throw new OperationCanceledException();
            }
        }
        
        public int GetItemCount()
        {
            return _allMarinaInENGWALES.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var item = cell as MarianaCell;
            if (DoesMarinaBelongToUser(index))
            {
                item.ConfigureCell(_allMarinaInENGWALES[index],_userMarianas.First(m => _allMarinaInENGWALES[index].pointOfInterestId == m.marinaId), index);
            }
            else
            {
                item.ConfigureCell(_allMarinaInENGWALES[index], index);
            }
        }

        private bool DoesMarinaBelongToUser(int index)
        {
            if (_userMarianas.Any(m => m.marinaId == _allMarinaInENGWALES[index].pointOfInterestId))
            {
                return true;
            }
            return false;
        }
    }
}