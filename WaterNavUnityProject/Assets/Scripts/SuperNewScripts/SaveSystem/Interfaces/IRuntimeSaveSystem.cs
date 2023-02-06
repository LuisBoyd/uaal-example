using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts.SaveSystem.Interfaces
{
    public interface IRuntimeSaveSystem<T>
    {
        IFileLoader<T> FileLoader { get; }
        IFileSaver<T> FileSaver { get; }

        T Value { get; }
        
        UniTaskVoid Save();
        UniTask<bool> Load( T overWriteObj);
        UniTask<bool> Load();
    }
}