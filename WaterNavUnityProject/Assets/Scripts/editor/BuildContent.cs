

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
namespace editor
{
    /// <summary>
    /// Internal means this class can only be called from files within the same assembly as it
    /// Builds addresable content as well as custom Pre-Build functionality e.g export map sections to server
    /// </summary>
    internal class BuildContent
    {
        public static string build_script = "Assets/AddressableAssetsData/DataBuilders" +
                                            "/BuildScriptPackedMode.asset";

        public static string settings_asset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";

        public static string profile_name = "Default";

        public static string serilizedMapFolder = "SerializedMapSections";
        private static AddressableAssetSettings settings;

        static void getSettingsObject(string settingsAsset)
        {
            settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                settingsAsset) as AddressableAssetSettings;

            if (settings == null)
            {
                Debug.LogError($"{settingsAsset} Couldn't be found or is not" +
                               $" A settings object");
            }
        }

        static void SetProfile(string profile)
        {
            string profileId = settings.profileSettings.GetProfileId(profile);
            if (string.IsNullOrEmpty(profileId))
            {
                Debug.LogWarning($"Could not find a profile named {profile}" +
                                 $" Using Current Profile Instead");
            }
            else
            {
                settings.activeProfileId = profileId;
            }
        }

        static void setBuilder(IDataBuilder builder)
        {
            int index = settings.DataBuilders.IndexOf((ScriptableObject)builder);

            if (index > 0)
                settings.ActivePlayerDataBuilderIndex = index;
            else
            {
                Debug.LogWarning($"{builder} must be added to the" +
                                 $" DataBuilder list before it can be made" +
                                 $" active. Using last run builder instead");
            }
        }

        static bool buildAddressableContent()
        {
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (!success)
            {
                Debug.LogError("Addressables build Error encountered: " +
                               result.Error);
            }

            return success;
        }

        [MenuItem("Window/Asset Management/Addressables/Build Addressables only")]
        public static bool BuildAddressables()
        {
            getSettingsObject(settings_asset);
            SetProfile(profile_name);
            IDataBuilder builderScript =
                AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script) as IDataBuilder;

            if (builderScript == null)
            {
                Debug.LogError(build_script + " couldn't be found or" +
                               " is not a build script");
                return false;
            }
            
            setBuilder(builderScript);
            return buildAddressableContent();

        }

        [MenuItem("Window/Asset Management/Addressables/Build Addressables and Send Map Data")]
        public async static Task<bool> BuildAddressablesAndMap()
        {
            IEnumerable<string> files;
            try
            {
               files = Directory.EnumerateFiles($"{Application.dataPath}/{serilizedMapFolder}").Where(s => s.EndsWith(".bmap"));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

            var client = new HttpClient();
            var apiUri = new Uri("https://waternav.co.uk/WaterNavGame/BuildTileMapContent.php"); //add t at end
            foreach (string file in files)
            {
              
                char[] result;
                StringBuilder builder = new StringBuilder();
                using (StreamReader streamReader = File.OpenText(file))
                {
                    result = new char[streamReader.BaseStream.Length];
                    await streamReader.ReadAsync(result, 0, (int)streamReader.BaseStream.Length);
                }
                
                foreach (char character in result)
                {
                    builder.Append(character);
                }
                
                //byte[] ReadBytes = Convert.FromBase64String(builder.ToString());
                //var byteContent = new ByteArrayContent(ReadBytes);
                var Base64StringByteContent = new StringContent(builder.ToString());
                var Filename = new StringContent(Path.GetFileName(file));

                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(Base64StringByteContent,"ByteData");
                multipartContent.Add(Filename, "FileName");

                var response = client.PostAsync(apiUri, multipartContent);
                await response;

                if (response.IsCanceled || !response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"Response was either Canceled or we got a invalid respose back {response.Result.StatusCode}");
                }
                
            }

            return false;
        }
    }
}
#endif