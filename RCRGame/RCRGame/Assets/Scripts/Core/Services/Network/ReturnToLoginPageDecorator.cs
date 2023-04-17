using System;
using System.Threading;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.Network
{
    public class ReturnToLoginPageDecorator : IAsyncDecorator
    {
        private readonly SceneSO _loginPageSo;
        private readonly LoadEventChannelSO _loadEventChannelSo;
        public ReturnToLoginPageDecorator(SceneSO loginPageSo, LoadEventChannelSO loadEventChannelSo)
        {
            _loginPageSo = loginPageSo;
            _loadEventChannelSo = loadEventChannelSo;
        }
        public async UniTask<ResponseContext> sendAsync(RequestContext context, CancellationToken token, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
        {
            try
            {
                return await next(context, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    throw;
                }
                if (e is UnityWebRequestException uwe)
                {
                    if (uwe.ResponseCode == 500)
                    {
                        // The only time to show a message for server exception is when debugging.
                        var result = await PromptDialog.Instance.showAsync(uwe.Message);

                        // OK or Cnacel or anything
                        if (result == PromptResult.Ok)
                        {
                           
                        }
                        else if (result == PromptResult.Cancel)
                        {
                           
                        }

                        // Do not await the scene load!
                        // If use await, the process will return to the caller and continued.
                        // so use Forget.
                        //SceneManager.LoadSceneAsync("LoginScreen").ToUniTask().Forget();
                        _loadEventChannelSo.RaiseEvent(_loginPageSo, false);
                        throw new OperationCanceledException();
                    }
                }
                throw;
            }
        }
    }
}