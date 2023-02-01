using System.Threading.Tasks;

namespace RCR.Settings.NewScripts.DataStorage.Interfaces
{
    public interface IFileReader
    {
        public string Location { get; }
        Task<string> Read(string FileContents = null);
    }
}