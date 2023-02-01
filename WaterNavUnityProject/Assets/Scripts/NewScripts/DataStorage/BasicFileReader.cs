using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RCR.Settings.NewScripts.DataStorage.Interfaces;
using UnityEngine.Device;

namespace RCR.Settings.NewScripts.DataStorage
{
    public class BasicFileReader: IFileReader
    {

        public IFileReader Wrapie;
        public BasicFileReader(IFileReader fileReader)
            => Wrapie = fileReader;

        public string Location { get; set; }

        public async Task<string> Read(string FileContents = null)
        {
            if (string.IsNullOrEmpty(FileContents))
            {
                Task<string> ExternalSourceHandle = ReadFromExternalSource();
                await ExternalSourceHandle;
                FileContents = ExternalSourceHandle.Result;
            }
            if (Wrapie != null)
            {
                Task<string> WrapieHandle = Wrapie.Read(FileContents);
                await WrapieHandle;
                return WrapieHandle.Result;
            }
            return FileContents;
        }

        public async Task<string> ReadFromExternalSource()
        {
            if(string.IsNullOrEmpty(Location))
                return null;
            char[] result;
            StringBuilder builder = new StringBuilder();
            var fullPath = Path.Combine(Application.persistentDataPath,
                Location);
            if(!File.Exists(fullPath))
                return null;
            
            using (StreamReader reader = File.OpenText(fullPath))
            {
                result = new char[reader.BaseStream.Length];
                await reader.ReadAsync(result,
                    0, (int)reader.BaseStream.Length);
            }
            
            foreach (char c in result)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    builder.Append(c);
            }
            return builder.ToString();

        }

    }
}