using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;

namespace Core.Services.Marina
{
    public interface IAsyncMarinaDecorator
    {
        UniTask<MarinaResponseContext> SendAsync(MarinaRequestContext context,
            CancellationToken token,
            Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next);
    }
}