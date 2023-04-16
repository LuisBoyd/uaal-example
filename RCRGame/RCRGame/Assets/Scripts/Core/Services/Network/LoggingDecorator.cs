using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Core.Services.Network
{
    public class LoggingDecorator : IAsyncDecorator
    {
        public async UniTask<ResponseContext> sendAsync(RequestContext context, CancellationToken token, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                Debug.Log($"Start network request {context.Path}");
                var response = await next(context, token);

                Debug.Log(
                    $"Complete network request : {context.Path}, Elapsed: {stopwatch.Elapsed} Size: {response.GetText().Length}"); //length of string

                return response;
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                    Debug.Log($"Request Canceled: {context.Path}");
                else if (ex is TimeoutException)
                    Debug.Log($"Request Timeout: {context.Path}");
                else if (ex is UnityWebRequestException webex)
                {
                    switch (webex.Result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                            Debug.Log(
                                $"Request Connection Error : {context.Path} Code: {webex.ResponseCode} Message: {webex.Message}");
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.Log(
                                $"Request Internal Server Error : {context.Path} Code: {webex.ResponseCode} Message: {webex.Message}");
                            break;
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.Log(
                                $"Request Data Processing Error : {context.Path} Code: {webex.ResponseCode} Message: {webex.Message}");
                            break;
                    }
                }

                throw;
            }
            finally
            {
                
            }
        }
    }
}