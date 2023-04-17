using System;
using System.Collections.Generic;
using Core.Services.Network;
using DefaultNamespace.Core.Enum;

namespace DefaultNamespace.Core.requests
{
    public class RequestContext
    {
        private int decoratorIndex;
        private readonly IAsyncDecorator[] _decorators;
        private Dictionary<string, string> headers;
        
        public string BasePath { get; }
        public string Path { get; }
        public object Value { get; }
        public TimeSpan Timeout { get; }
        public DateTimeOffset TimeStamp { get; private set; }
        
        public RequestType RequestType { get; }

        public IDictionary<string, string> RequestHeader
        {
            get
            {
                if (headers == null)
                    headers = new Dictionary<string, string>();
                return headers;
            }
        }

        public RequestContext(RequestType requestType ,string basePath, string path, object value, TimeSpan timeout, IAsyncDecorator[] filters)
        {
            this.RequestType = requestType;
            this.decoratorIndex = -1;
            this._decorators = filters;
            this.BasePath = basePath;
            this.Path = path;
            this.Value = value;
            this.Timeout = timeout;
            this.TimeStamp = DateTimeOffset.Now;
        }

        internal Dictionary<string, string> GetRawHeaders() => headers;
        internal IAsyncDecorator GetNextDecorator() => _decorators[++decoratorIndex];

        public void Reset(IAsyncDecorator currentFilter)
        {
            decoratorIndex = Array.IndexOf(_decorators, currentFilter);
            if(headers != null)
                headers.Clear();
            TimeStamp = DateTimeOffset.Now;
        }
    }
}