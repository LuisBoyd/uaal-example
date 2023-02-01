using System.Threading.Tasks;

namespace RCR.Settings.NewScripts.DataStorage.Interfaces
{
    public interface ILoader<T> where T : new()
    {
        public IFileReader FileReader { get; }

        public Task<T> ReconstructObject();
    }
}