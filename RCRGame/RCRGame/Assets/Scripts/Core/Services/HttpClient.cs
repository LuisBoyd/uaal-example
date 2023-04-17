using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.Serialization;
using Utility.Logging;
using ILogger = UnityEngine.ILogger;

namespace Core.Services
{

    public interface IHttpClient
    {
        public UniTask<T> Get<T>(string endPoint);
        public UniTask<T> Post<T>(string endPoint, object payload);
    }
    
    public class HttpClient : IHttpClient
    {
        private readonly InternalSetting _internalSetting;
        private readonly RuntimeLogger _logger;
        public HttpClient(InternalSetting setting)
        {
            _internalSetting = setting;
        }
        
        public  async UniTask<T> Get<T>(string endPoint)
        {
            try
            {
                var getRequest = CreateRequest(endPoint);
                await getRequest.SendWebRequest();
                if (!HandleResponseCode(getRequest))
                {
                    //Response Failed we pass back json string from the HTTP request. in php on the server.
                    return JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
                }
                //Response Success we pass back json string from the HTTP request. in php on the server.
                return JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
            }
            catch (UnityWebRequestException e)
            {
                _logger.LogWarning("[Warning]", e.Message);
                return default;
            }
        }

        public  async UniTask<T> Post<T>(string endPoint, object payload)
        {
            try
            {
                var postRequest = CreateRequest(endPoint, RequestType.POST, payload);
                await postRequest.SendWebRequest();
                if (!HandleResponseCode(postRequest))
                {
                    //Response Failed we pass back json string from the HTTP request. in php on the server.
                    return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
                }
                //Response Success we pass back json string from the HTTP request. in php on the server.
                return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
                //return SerializationUtility.DeserializeValue<T>(postRequest.downloadHandler.data, DataFormat.JSON);
            }
            catch (UnityWebRequestException e)
            {
                _logger.LogWarning("[Warning]", e.Message);
                return default;
            }
        }


        private  UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data  = null)
        {
            var request = new UnityWebRequest(path, type.ToString());

            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            request.timeout = _internalSetting.DefaultRequestTimeOut;
            Debug.Log(_internalSetting.DefaultRequestTimeOut);
            return request;
        }

        private bool HandleResponseCode(UnityWebRequest webRequest)
        {
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    _logger.LogError($"There was a connection Error {webRequest.error}", webRequest);
                    return false;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    _logger.LogError($"There was a protocol Error {webRequest.error}", webRequest);
                    return false;
                    break;
                case UnityWebRequest.Result.InProgress:
                    _logger.LogError($"undefined InPorgress error should not happen as we await the process", webRequest);
                    return false;
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    _logger.LogError($"There was a data processing Error {webRequest.error}", webRequest);
                    return false;
                    break;
                case UnityWebRequest.Result.Success:
                    return true;
                    break;
                default:
                    _logger.LogError($"Undefined Error for networking", webRequest);
                    return false;
                    break;
            }
        }

        private static void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key,value);
        }
        
        
        public enum RequestType
        {
            GET = 0,
            POST = 1
        }
    }
}