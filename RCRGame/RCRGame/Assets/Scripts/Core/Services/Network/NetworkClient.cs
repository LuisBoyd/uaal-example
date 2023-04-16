using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Utility;

namespace Core.Services.Network
{
    public class NetworkClient : IAsyncDecorator
    {
        private readonly Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next;
        private readonly IAsyncDecorator[] _decorators;
        private readonly TimeSpan timeout;
        private readonly IProgress<float> _progress;
        private readonly string basePath;

        public NetworkClient(string basePath, TimeSpan timeout, params IAsyncDecorator[] decorators) : this(basePath,
            timeout, null, decorators)
        {
            
        }

        public NetworkClient(string basePath, TimeSpan timeout, IProgress<float> progress,
            params IAsyncDecorator[] decorators)
        {
            this.next = InvokeRecursive;

            this.basePath = basePath;
            this.timeout = timeout;
            this._progress = progress;
            this._decorators = new IAsyncDecorator[decorators.Length + 1];
            Array.Copy(decorators, this._decorators, decorators.Length);
            this._decorators[^1] = this;
        }

        public async UniTask<T> PostAsync<T>(string path, T value, CancellationToken token = default)
        {
            var request = new RequestContext(basePath, path, value, timeout, _decorators);
            var response = await InvokeRecursive(request, token);
            return response.GetResponseAs<T>();
        }

        UniTask<ResponseContext> InvokeRecursive(RequestContext context, CancellationToken token)
        {
            return context.GetNextDecorator().sendAsync(context, token, next);
        }

        async UniTask<ResponseContext> IAsyncDecorator.sendAsync(RequestContext context, CancellationToken token,
            Func<RequestContext, CancellationToken, UniTask<ResponseContext>> _)
        {
            // This is sample, use only POST
            // If you want to maximize performance, customize uploadHandler, downloadHandler
            // send JSON in body as parameter
            var data = JsonConvert.SerializeObject(context.Value);
            var formData = new Dictionary<string, string> { { "body", data } };

            using (var req = UnityWebRequest.Post(basePath + context.Path, formData))
            {
                var header = context.GetRawHeaders();
                if (header != null)
                {
                    foreach (var item in header)
                    {
                        req.SetRequestHeader(item.Key, item.Value);
                    }
                }
                // You can process Timeout by CancellationTokenSource.CancelAfterSlim(extension of UniTask)
                var linkToken = CancellationTokenSource.CreateLinkedTokenSource(token);
                linkToken.CancelAfterSlim(timeout);
                try
                {
                    await req.SendWebRequest().ToUniTask(progress: _progress, cancellationToken: linkToken.Token);
                }
                catch (OperationCanceledException)
                {
                    if (!token.IsCancellationRequested)
                    {
                        throw new TimeoutException();
                    }
                }
                finally
                {
                    // stop CancelAfterSlim's loop
                    if (!linkToken.IsCancellationRequested)
                    {
                        linkToken.Cancel();
                    }
                }
                // Get response items first because UnityWebRequest is disposed in end of this method.
                // If you want to avoid allocating repsonse/header if no needed, think another way,
                return new ResponseContext(req.downloadHandler.text, req.responseCode, req.GetResponseHeaders());
            }
        }
    }
}