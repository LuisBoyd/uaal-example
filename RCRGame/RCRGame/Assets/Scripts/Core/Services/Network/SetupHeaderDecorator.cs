using System;
using System.Threading;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using UnityEngine;

namespace Core.Services.Network
{
    public class SetupHeaderDecorator : IAsyncDecorator
    {
        private readonly InternalSetting _setting;

        public SetupHeaderDecorator(InternalSetting setting)
        {
            _setting = setting;
        }
        
        public async UniTask<ResponseContext> sendAsync(RequestContext context, CancellationToken token, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
        {
            context.RequestHeader["x-app-timestamp"] = context.TimeStamp.ToString();

            var response = await next(context, token);
            return response;
        }
    }
}