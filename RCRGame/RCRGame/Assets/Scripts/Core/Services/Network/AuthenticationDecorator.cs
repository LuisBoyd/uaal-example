using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using UnityEngine;

namespace Core.Services.Network
{
    public class AuthenticationDecorator : IAsyncDecorator
    {
        public async UniTask<ResponseContext> sendAsync(RequestContext context, CancellationToken token, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
        {
            try
            {
                return await next(context, token);
            }
            catch (Exception e)
            {
                // ignored
                throw;
            }

            return null;
        }
    }
}