using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStructures;
using RCR.BaseClasses;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RCR.Managers
{
    [DefaultExecutionOrder(-3)]
    public class NetworkManager : Singelton<NetworkManager>
    {
        public IEnumerator PutRequest(string phpRequestName,
            IDictionary<string, string> Headers = null,
            Action<bool, string> JsonResponseCallback = null)
        {
            if (string.IsNullOrEmpty(phpRequestName))
                yield break;
            WWWForm form = new WWWForm();
            if (Headers != null)
                foreach (var KeyPair in Headers)
                {
                    form.AddField(KeyPair.Key, KeyPair.Value);
                }

            using (UnityWebRequest webRequest =
                   UnityWebRequest.Post($"https://waternav.co.uk/WaterNavGame/{phpRequestName}.php", form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    if (JsonResponseCallback != null)
                        JsonResponseCallback(false, $"Error with {webRequest.error}");
                    yield break;
                }

                JObject jsonResponse = JObject.Parse(webRequest.downloadHandler.text);

                if (JsonResponseCallback != null)
                    JsonResponseCallback(true, jsonResponse.ToString());
            }
        }

        public IEnumerator PutRequest(string phpRequestName, CoroutineToken tkn,
            IDictionary<string, string> Headers = null,
            Action<bool, string, CoroutineToken> JsonResponseCallback = null)
        {
            if (string.IsNullOrEmpty(phpRequestName))
                yield break;
            WWWForm form = new WWWForm();
            if (Headers != null)
                foreach (var KeyPair in Headers)
                {
                    form.AddField(KeyPair.Key, KeyPair.Value);
                }

            using (UnityWebRequest webRequest =
                   UnityWebRequest.Post($"https://waternav.co.uk/WaterNavGame/{phpRequestName}.php", form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    tkn.Cancel();
                    yield break;
                }

                JObject jsonResponse = JObject.Parse(webRequest.downloadHandler.text);

                if (JsonResponseCallback != null)
                    JsonResponseCallback(true, jsonResponse.ToString(), tkn);
            }
        }
    }
}