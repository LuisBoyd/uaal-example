using Newtonsoft.Json;
using UnityEngine;

namespace Systems.PersistantData.Models
{
    public class UserGameData : BaseDataModel
    {
        [JsonRequired]
        [SerializeField] 
        private string[] TestingStringArray;

        [JsonRequired]
        [SerializeField] 
        private int TestInt;

        [JsonRequired] 
        [SerializeField] 
        private Vector3[] TestVec3;

        protected override void Awake()
        {
            base.Awake();
            _fileLocation = "UserGameData.json";
        }
    }
}