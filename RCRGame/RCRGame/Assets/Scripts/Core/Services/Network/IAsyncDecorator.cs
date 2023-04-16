using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;

namespace Core.Services.Network
{
    public interface IAsyncDecorator
    {
        UniTask<ResponseContext> sendAsync(RequestContext context, CancellationToken token,
            Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next);
    }
}