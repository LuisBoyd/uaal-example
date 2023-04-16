using System.Collections.Generic;
using Newtonsoft.Json;
using Utility;
using VContainer;

namespace DefaultNamespace.Core.response
{
    public class ResponseContext
    {
        private readonly string text;
        public long StatusCode { get; }
        
        public Dictionary<string, string> ResponseHeaders { get; }

        public ResponseContext(string text, long statusCode, Dictionary<string, string> responseHeaders)
        {
            this.text = text;
            this.StatusCode = statusCode;
            this.ResponseHeaders = responseHeaders;
        }

        public string GetText() => text;

        [Inject]
        public T GetResponseAs<T>() 
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}