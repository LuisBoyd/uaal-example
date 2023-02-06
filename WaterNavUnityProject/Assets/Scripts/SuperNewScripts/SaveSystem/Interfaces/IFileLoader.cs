using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts.SaveSystem.Interfaces
{
    public interface IFileLoader<T>
    {
        public bool CanRead { get; }
        UniTask<OperationResult<T>> ReadFromFile(string fileLocation);

       UniTask<bool> ReadFromJson(TextReader txtReader, ref T obj);
    }
}