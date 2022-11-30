using System;
using System.Threading.Tasks;

namespace RCR.Interfaces
{
    public interface IObjectLoader<T>
    {
        public Task<T> LoadObject(Uri phpRequest);
    }
}