using System.IO;
using Cysharp.Threading.Tasks;

namespace RCR.Settings.SuperNewScripts.SaveSystem.Interfaces
{
    public interface IFileSaver<T>
    {
        public bool CanWrite { get; }

        public UniTaskVoid WriteToJson(TextWriter txtWriter,  T value);
        UniTaskVoid WriteToFile(string location, T value);
        string ToString();
    }
}