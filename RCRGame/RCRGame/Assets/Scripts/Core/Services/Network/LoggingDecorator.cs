using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.Networking;
using Utility.Logging;
using Debug = UnityEngine.Debug;

namespace Core.Services.Network
{
    public class LoggingDecorator : IAsyncDecorator
    {
        
        private InfoDisplayEventChannelSO _infoDisplayEvent;
        
        public LoggingDecorator(InfoDisplayEventChannelSO infoDisplayEvent)
        {
            _infoDisplayEvent = infoDisplayEvent;
        }
        
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
                {
                    Debug.Log($"Request Canceled: {context.Path}");
                    throw;
                }

                if (ex is TimeoutException)
                {
                    Debug.Log($"Request Timeout: {context.Path}");
                    throw;
                }

                if (ex is UnityWebRequestException uwe)
                {
                    switch (uwe.Result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                            Debug.Log(
                                $"Request Connection Error : {context.Path} Code: {uwe.ResponseCode} Message: {uwe.Message}");
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.Log(
                                $"Request Internal Server Error : {context.Path} Code: {uwe.ResponseCode} Message: {uwe.Message}");
                            break;
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.Log(
                                $"Request Data Processing Error : {context.Path} Code: {uwe.ResponseCode} Message: {uwe.Message}");
                            break;
                    }
                    HandleErrorCodes(uwe);
                    throw;
                }
            }
            finally
            {
                
            }
            return null;
        }

        private void HandleErrorCodes(UnityWebRequestException ex)
        {
            switch (ex.ResponseCode)
            {
                case 401:
                    //The User has tried to access a unauthorized resource.
                    _infoDisplayEvent.RaiseEvent(ex.ResponseCode, ex.ResponseHeaders["Error"], Color.yellow);
                    break;
                case 500:
                    _infoDisplayEvent.RaiseEvent(ex.ResponseCode, ex.ResponseHeaders["Error"], Color.red);
                    break;
            }
        }
    }
}