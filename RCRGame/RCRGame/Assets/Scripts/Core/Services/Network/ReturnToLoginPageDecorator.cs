using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.Network
{
    public class ReturnToLoginPageDecorator : IAsyncDecorator
    {
        public async UniTask<ResponseContext> sendAsync(RequestContext context, CancellationToken token, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
        {
            try
            {
                return await next(context, token);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException opEx:
                        // Canceling is an expected process, so it goes through as is
                        throw;
                        break;
                    case UnityWebRequestException Uwe:
                        // It's useful to use a status code to handle things like revert to the title or retry exceptions
                        // if (uwe.ResponseCode) { }...
                        break;
                }

                // The only time to show a message for server exception is when debugging.
                var result = await PromptDialog.Instance.showAsync(ex.Message);
                
                // OK or Cnacel or anything
                if (result == PromptResult.Ok)
                {
                    Debug.Log("Pressed OK");
                }
                else if (result == PromptResult.Cancel)
                {
                    Debug.Log("Pressed Cancel");
                }
                
                // Do not await the scene load!
                // If use await, the process will return to the caller and continued.
                // so use Forget.
                SceneManager.LoadSceneAsync("LoginScreen").ToUniTask().Forget();

                // Finally throw OperationCanceledException, caller receive canceled.
                throw new OperationCanceledException();
            }
        }
    }
}