using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.Serialization;

namespace Core.Services
{

    public interface IHttpClient
    {
        public Task<T> Get<T>(string endPoint);
        public Task<T> Post<T>(string endPoint, object payload);
    }
    
    public class HttpClient : IHttpClient
    {
        public  async Task<T> Get<T>(string endPoint)
        {
            var getRequest = CreateRequest(endPoint);
            getRequest.SendWebRequest();

            while (!getRequest.isDone) await Task.Delay(10);
            return SerializationUtility.DeserializeValue<T>(getRequest.downloadHandler.data, DataFormat.JSON);
        }

        public  async Task<T> Post<T>(string endPoint, object payload)
        {
            var postRequest = CreateRequest(endPoint, RequestType.POST, payload);
            postRequest.SendWebRequest();

            while (!postRequest.isDone) await Task.Delay(10);
            return SerializationUtility.DeserializeValue<T>(postRequest.downloadHandler.data, DataFormat.JSON);
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