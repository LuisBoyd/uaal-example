using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Core3.MonoBehaviors
{
    public class MessagePromptView : BaseMonoBehavior
    {
        [SerializeField] private Button okButton = default;
        [SerializeField] private Button closeButton = default;

        private UniTaskCompletionSource<PromptResult> taskCompletion;
        
        //await until Button Clicked
        public UniTask<PromptResult> WaitUntilClicked => taskCompletion.Task;

        private void Start()
        {
            taskCompletion = new UniTaskCompletionSource<PromptResult>();

            okButton.onClick.AddListener(() =>
            {
                taskCompletion.TrySetResult(PromptResult.Ok);
                Debug.Log(taskCompletion.Task.Status);
            });
            
            closeButton.onClick.AddListener(() =>
            {
                taskCompletion.TrySetResult(PromptResult.Cancel);
                Debug.Log(taskCompletion.Task.Status);
            });
        }

        //To Be Safe
        private void OnDestroy()
        {
            taskCompletion.TrySetResult(PromptResult.Cancel);
        }
    }
}