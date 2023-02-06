using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RCR.BaseClasses;
using RCR.Settings.SuperNewScripts.DontDestroyOnLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RCR.Settings.SuperNewScripts
{
    public class GameLoader: Singelton<GameLoader>
    {
        //1. Load all releveant Assets with Addressables
        //2. Load all relevant data from local files based on passed in location
        //3. Load Player Data from the cloud.
        
        //Fails if any of these can't be completed and displays some error.
        
        //TODO Later on Move all Addressable functionality into it's own object and player data and such since then I can destroy this object

        #region UI-Values
        //Read from outside to get indication on Game Loading Progress.
        private Slider ProgressSlider;
        public float ProgressValue { get; private set; }
        #endregion

        #region SceneInstance

        private Scene CurrentScene;
        [SerializeField] 
        private AssetReference _SceneToLoad;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            ProgressSlider = FindObjectOfType<Slider>();
            CurrentScene = SceneManager.GetActiveScene();
            AddresablesSystem.GetInstance();
        }

        private async UniTaskVoid Start()
        {
            ProgressSlider.value = 0;
            UniTask[] startUpTasks = new UniTask[] //I would like to replace with a system that reports like all data coming in but it can wait
            {
                AddresablesSystem.Instance.Start(),
                
                //This One Last
                AddresablesSystem.Instance.LoadSceneAsync(_SceneToLoad, LoadSceneMode.Additive, true )
            };

            await WhenAllWithProgress(startUpTasks, UpdateProgressValue);
            await SceneManager.UnloadSceneAsync(CurrentScene);
        }

        private void UpdateProgressValue(float value)
        {
            ProgressValue = value;
            ProgressSlider.value = ProgressValue;
        }

        private async UniTask WhenAllWithProgress(UniTask[] tasks, Action<float> onProgress)
        {
            float progress = 0f;
            while (progress < 1f)
            {
                await UniTask.WaitUntilValueChanged(tasks, i => i.Count(t => t.Status == UniTaskStatus.Succeeded));
                await UniTask.SwitchToMainThread();
                progress = (float) tasks.Count(t => t.Status == UniTaskStatus.Succeeded) / tasks.Length;
                onProgress.Invoke(progress);
            }
        }

        
        //NGL gonna wanna UNIT test this one
        /// <summary>
        /// Called on the Native platform to feed information into unity
        /// </summary>
        /// <param name="NativeAppInfo"></param>
        private void AcceptNativeAppInfo(string NativeAppInfo)
        {
            JObject AppInfo = JObject.Parse(NativeAppInfo);
            if (!AppInfo.IsValid(GameConstants.Schema))
            {
                Debug.LogError("Passed in Json was not formatted properly");
                return;
            }
            //Set the LocationID
            InitialData.SetLocationID( AppInfo["LocationID"] != null ? AppInfo["LocationID"].ToString() : String.Empty);
        }

    }
}