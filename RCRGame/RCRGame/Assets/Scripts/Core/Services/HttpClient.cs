using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
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
        public HttpClient(InternalSetting setting, RuntimeLogger logger)
        {
            _internalSetting = setting;
            _logger = logger;
        }
        
        public  async UniTask<T> Get<T>(string endPoint)
        {
            try
            {
                var getRequest = CreateRequest(endPoint);
                await getRequest.SendWebRequest();
                return SerializationUtility.DeserializeValue<T>(getRequest.downloadHandler.data, DataFormat.JSON);
            }
            catch (Exception e)
            {
                _logger.LogWarning("[Warning]", e.Message);
                _logger.LogException(e);
            }
            return default;
        }

        public  async UniTask<T> Post<T>(string endPoint, object payload)
        {
            try
            {
                var postRequest = CreateRequest(endPoint, RequestType.POST, payload);
                await postRequest.SendWebRequest();
                return SerializationUtility.DeserializeValue<T>(postRequest.downloadHandler.data, DataFormat.JSON);
            }
            catch (Exception e)
            {
                _logger.LogWarning("[Warning]", e.Message);
               _logger.LogException(e);
            }
            return default;
        }


        private  UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data  = null)
        {
            var request = new UnityWebRequest(path, type.ToString());

            if (data != null)
            {
                var bodyRaw = SerializationUtility.SerializeValue(data, DataFormat.JSON);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            request.timeout = _internalSetting.DefaultRequestTimeOut;
            Debug.Log(_internalSetting.DefaultRequestTimeOut);
            return request;
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