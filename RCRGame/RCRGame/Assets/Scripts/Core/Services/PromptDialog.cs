using Core3.MonoBehaviors;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;
using UnityEngine;

namespace Core.Services
{
    public class PromptDialog : Singelton<PromptDialog>
    {
        public  async UniTask<PromptResult> showAsync(string message)
        {
            //create prefab prompt view
            var view = await Resources.LoadAsync<GameObject>("UI/Prompt");
            GameObject viewObj = Instantiate(view, transform) as GameObject;
            await UniTask.NextFrame(); //need to wait until the next frame otherwise completion source does not get assigned
            var result = await viewObj.GetComponent<MessagePromptView>().WaitUntilClicked;
            Debug.Log("Passed result");
            return result;
        }
    }
}