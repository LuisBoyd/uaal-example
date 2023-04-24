using System;
using Core.models;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Services.Marina
{
    public class MarinaMapService : BaseMonoBehavior
    {
        [Title("Listening To Events", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private IntEventChannelSO _visitMarinaChannel;

        [Title("Broadcasting On Events", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private LoadEventChannelSO _sceneLoadChannel;
        
        [Title("Configurations", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private SceneSO _gameplayScene;
        
        private User _user;
        private UserMariana _userMariana;
        private InternalSetting _setting;
        

        [Inject]
        private void InjectValues(User user, UserMariana userMariana, InternalSetting internalSetting)
        {
            _user = user;
            _userMariana = userMariana;
            _setting = internalSetting;
        }
        private void OnEnable()
        {
            _visitMarinaChannel.onEventRaised += VisitMarina;
        }
        private void OnDisable()
        {
            _visitMarinaChannel.onEventRaised -= VisitMarina;
        }

        public void VisitMarina(int marinaID)
        {
            _userMariana.system_user_id = _user.User_id;
            _userMariana.marinaId = marinaID;
            _setting.UserMapDataLocalSavePath = Application.persistentDataPath + $"/{marinaID}usermapdata.txt";
            _sceneLoadChannel.RaiseEvent(_gameplayScene, true);
        }

    }
}