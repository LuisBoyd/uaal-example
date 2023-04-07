using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

namespace editor
{
#if ODIN_INSPECTOR
	/// <summary>
	/// Data structure that is used to serialize the objects. It also stores type information so we can cast it back.
	/// </summary>
	[Serializable]
	public class SerializedObject
	{
		[SerializeField]
		public Type objectType;
		public dynamic serializedObject;
	}
	
	/// <summary>
	/// A windows which provides interface to de-/se-rialize some object to JSON,
	/// bytes, nodes.
	/// </summary>
	public class DataSerializer : OdinEditorWindow
	{
		[Title("Data Serializer", "Is it 'data is' or 'data are'?", TitleAlignments.Centered), PropertyOrder(-10)]
		[OnInspectorGUI, DetailedInfoBox("Can be used to serialize or deserialize objects.", "It can be save files to various formats and load them back. \nThat file then can be used as a data source for other things. \nSave folder tells where the serialised file will be save with name SaveFileName.\nFilePath is used for loading from files.\nUnity's JSON format uses the JsonUtility which is a normal JSON file. While Odin's JSON format is proprietary but can save more information. You need to use Odin to load it back again though.")]
		public void InspectorInfoBox() {}
		/// <summary>
		/// Folder in which we save the new files.
		/// </summary>
		[ShowInInspector, FolderPath(RequireExistingPath = true)]
		[HorizontalGroup("Saving", width:0.75f), LabelWidth(100)]
        [OnValueChanged("UpdateFilePathFromFileName")]
		public string SaveFolder = string.Empty;
		
		/// <summary>
		/// Name of the new file to save to.
		/// </summary>
		[HorizontalGroup("Saving")]
		[HideLabel, LabelText("/"), LabelWidth(10)]
        [OnValueChanged("UpdateFilePathFromFileName")]
		public string SaveFileName = string.Empty;

        private void UpdateFilePathFromFileName() => filePath = CurrentFilePath;

        /// <summary>
		/// Format of the file/serialized object we want to save/load to/from.
		/// </summary>
		// [HorizontalGroup("File"), PropertyOrder(0)]
		[HorizontalGroup("Format")]
		[OnValueChanged("UpdateFilePathFromFileName")]
		public DataFormat dataFormat = DataFormat.JSON;

        public enum JsonFormat
		{
			Newtonsoft,
			Unity,
			Odin,
		}

        [HorizontalGroup("Format"), LabelWidth(100)]
		[ShowIf("IsJsonFormat")]
		[OnValueChanged("UpdateFilePathFromFileName")]
		public JsonFormat jsonFormat = JsonFormat.Newtonsoft;

        private bool IsJsonFormat => dataFormat == DataFormat.JSON;

        /// <summary>
		/// Possible file extension allowed to select in the filePath field.
		/// </summary>
		private string FileExtensions
		{
			get
			{
				var extensions = string.Empty;
				var formats = (DataFormat[]) Enum.GetValues(typeof(DataFormat));
				for (var index = 0; index < formats.Length; index++)
				{
					DataFormat format = ((DataFormat[]) Enum.GetValues(typeof(DataFormat)))[index];
					extensions += (ExtensionForFormat(format));
					if (index != (formats.Length - 1))
						extensions += ", ";
				}

				return extensions;
			}
		}

        /// <summary>
		/// Returns current file path based on values of other fields i.e. save folder, file name,
		/// data format, json type.
		/// </summary>
		private string CurrentFilePath => Path.Combine(
			SaveFolder,
			SaveFileName
			+ (dataFormat != DataFormat.JSON
				? string.Empty
				: jsonFormat == JsonFormat.Unity
					? ".u"
					: jsonFormat == JsonFormat.Odin
						? ".o"
						: string.Empty)
			+ $".{ExtensionForFormat(dataFormat)}");

        /// <summary>
        /// Path to the file that is being saved/loaded.
        /// </summary>
        // [HorizontalGroup("File", width:0.75f)]
        [Sirenix.OdinInspector.FilePath(RequireExistingPath = true, AbsolutePath = true, Extensions = "$FileExtensions", ParentFolder = "$SaveFolder"), PropertyOrder(0)]
        // [BoxGroup("Files", GroupName = "Loading")]
        public string filePath = string.Empty;

        [SerializeField, HideInInspector]
		/// <summary>
		/// The model that is loaded/will be saved.
		/// </summary>
		//TODO: cannot change value type by copying new value in it. 
		private dynamic serializedModel = null;

		/// <summary>
		/// The model that is loaded/will be saved.
		/// We need to use this property format bcz we want to stop the inspector from rendering it
		/// while it changes types. It can change type bcz user can paste any type of value in this field.
		/// </summary>
		[ShowInInspector]
		[ShowIf("_variableWontChange")]
		public dynamic SerializedModel
		{
			get => serializedModel;
			set
			{
				_variableWontChange = false;
				serializedModel = null;
				EditorApplication.delayCall += () =>
											   {
												   serializedModel = value;
												   _variableWontChange = true;
											   };
			}
		}

		/// <summary>
		/// Flag to stop dynamic serialized object from drawing while it is being changed.
		/// </summary>
		private bool _variableWontChange = true;

		/// <summary>
		/// Reference to instance of this window.
		/// </summary>
		[NonSerialized]
		private static DataSerializer _instance;

        [ShowInInspector, MultiLineProperty(20), FoldoutGroup("FileContent", true, 111), HideLabel]
        public string FileContent
        {
            get
            {
                if (dataFormat != DataFormat.Binary)
                {
                    if (FilePathValid)
                    {
                        try
                        {
                            var fileContent = File.ReadAllLines(filePath);
                            
                            return fileContent.Aggregate((sum, next) => sum = $"{sum}{Environment.NewLine}{next}");

                        }
                        catch (Exception exception)
                        {
                            return $"Threw exception when trying to read file: {exception}";
                        }
                        
                    }
                    else
                    {
                        return $"File doesn't exist on path: {filePath}";
                    }
                }

                return "Binary data type selected. Cannot display content.";
            }
        }
		
		
		/// <summary>
		/// Open window. Initialize default values.
		/// </summary>
		[MenuItem("Tools/Serializer &#Z")]
		private static void OpenWindow()
		{
			_instance = GetWindow<DataSerializer>();
			_instance.Show();
			_instance.SaveFolder = Path.Combine(Application.dataPath, "Serialized Assets");
			_instance.SaveFileName = string.Empty;
		}

        protected override void Initialize()
        {
            base.Initialize();
            _instance = this;
        }

        /// <summary>
		/// Save the model to file.
		/// </summary>
		/// <exception cref="NotImplementedException">Threw when specified format is not supported.</exception>
		[HorizontalGroup("Buttons"), Button(ButtonSizes.Large)]
		[ShowIf("ObjectValid")]
		public void SaveToFile()
		{
			if (SerializedModel == null) return;

			var luSerializedObject = new SerializedObject {objectType = SerializedModel.GetType(), serializedObject = SerializedModel};
			 
			switch (dataFormat)
			{
				case DataFormat.Binary:
					byte[] fileContent = SerializationUtility.SerializeValue(luSerializedObject, dataFormat);
					WriteToFile(CurrentFilePath, fileContent);
					break;
				case DataFormat.JSON:
					dynamic jsonContent;
					if (jsonFormat == JsonFormat.Newtonsoft)
					{
						jsonContent = JsonConvert.SerializeObject(luSerializedObject, luSerializedObject.GetType(), new JsonSerializerSettings{Formatting = Formatting.Indented});
					}
					else if (jsonFormat == JsonFormat.Unity)
					{
						jsonContent = JsonUtility.ToJson(luSerializedObject, true);
					}
					else if (jsonFormat == JsonFormat.Odin)
					{
						jsonContent = SerializationUtility.SerializeValue(luSerializedObject, dataFormat);
					}
					else
					{
						throw new InvalidEnumArgumentException();
					}

                    WriteToFile(CurrentFilePath, jsonContent);
					break;
				case DataFormat.Nodes:
                    //todo: fix this. it throws the exception.
                    var nodeContent = SerializationUtility.SerializeValue(luSerializedObject, DataFormat.Nodes, new SerializationContext());
                    WriteToFile(CurrentFilePath, nodeContent);
                    break;
				default:
					throw new NotImplementedException($"{dataFormat}: not implemented for saving.");
			}

			filePath = CurrentFilePath;

		}

		/// <summary>
		/// A delayed call to load function. Allows us to change type of variable while it is changing.
		/// It stops the variable from being drawing in inspector. If variable changes type while being drawed then
		/// it'll throw exceptions and won't recover. 
		/// </summary>
		[HorizontalGroup("Buttons"), Button(ButtonSizes.Large, Name = "Load From File")]
		[ShowIf("FilePathValid")]
		public void LoadFromFileDelayed()
		{
			serializedModel = null;
			_variableWontChange = false;
			EditorApplication.delayCall += LoadFromFile;
		}

		/// <summary>
		/// Load the serialized model from file.
		/// </summary>
		/// <exception cref="NotImplementedException">Thrown when file format is not supported.</exception>
		private void LoadFromFile()
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return;
			}
		
			var fileFormat = InferFileTypeFromFilePath(filePath);
			SerializedObject serializedObject = null;
			switch (fileFormat)
			{
				case DataFormat.Binary:
				case DataFormat.Nodes:		
					byte[] fileContent = File.ReadAllBytes(filePath);
					serializedObject = SerializationUtility.DeserializeValue<SerializedObject>(fileContent, fileFormat);
					break;
				case DataFormat.JSON:
					jsonFormat = filePath.EndsWith($".u{ExtensionForFormat(DataFormat.JSON)}")
						? JsonFormat.Unity
						: filePath.EndsWith($".u{ExtensionForFormat(DataFormat.JSON)}")
							? JsonFormat.Odin
							: JsonFormat.Newtonsoft;
					if (jsonFormat == JsonFormat.Unity)
					{
						string jsonContent = File.ReadAllText(filePath);
						serializedObject = JsonUtility.FromJson<SerializedObject>(jsonContent);
						
					}
					else if (jsonFormat == JsonFormat.Odin)
					{
						byte[] jsonContent = File.ReadAllBytes(filePath);
						serializedObject = SerializationUtility.DeserializeValue<SerializedObject>(jsonContent, fileFormat);
					}else if (jsonFormat == JsonFormat.Newtonsoft)
					{
						string jsonContent = File.ReadAllText(filePath);
						serializedObject = JsonConvert.DeserializeObject<SerializedObject>(jsonContent);

					}
					else
					{
						_variableWontChange = true;
						throw new NotImplementedException("JSON format not supported.");
					}
					break;
				default:
					throw new NotImplementedException($"{fileFormat}: not implemented for loading.");
					
			}

			if (serializedObject.objectType == null)
			{
				Debug.LogError(
					"The file did not have any type info. The object cannot be converted and shown. File was not saved properly.");
			}
			else
			{
				try
				{
//					serializedModel = Convert.ChangeType(
//						serializedObject.serializedObject,
//						serializedObject.objectType);
					serializedModel =
						((JObject)serializedObject.serializedObject).ToObject(serializedObject.objectType);
				}
				finally
				{
					_variableWontChange = true;
				}
			}

			_variableWontChange = true;
		}

		/// <summary>
		/// Gets extension for the format supplied.
		/// </summary>
		/// <param name="format">Data format</param>
		/// <returns>Extension for that data format.</returns>
		/// <exception cref="NotImplementedException">Thrown when data format is not implemented.</exception>
		private static string ExtensionForFormat(DataFormat format)
		{
			switch (format)
			{
				case DataFormat.Binary: return "byte";
				case DataFormat.JSON: return "json";
				case DataFormat.Nodes: return "nodes";
				default: throw new NotImplementedException("No extension specified for type. Please add here.");
			}
		}

		/// <summary>
		/// Tries to guess file type using it's extension. Uses ends with on file path.
		/// </summary>
		/// <param name="filePath">Path to file.</param>
		/// <returns>DataFormat related to that extension.</returns>
		private static DataFormat InferFileTypeFromFilePath(string filePath)
		{
			var formats = (DataFormat[]) Enum.GetValues(typeof(DataFormat));
			foreach (var format in formats)
			{
				if (filePath.EndsWith($".{ExtensionForFormat(format)}"))
				{
					return format;
				}
			}
			
			Debug.LogWarning("Could not find a format that matches the file path extension. Using format set in editor.");
			return _instance.dataFormat;
		}

		/// <summary>
		/// Used to show/hide load button. Cannot load if file path is not valid. 
		/// </summary>
		private bool FilePathValid => File.Exists(filePath);

		/// <summary>
		/// Used to show/hide save button. Cannot save if value is not set.
		/// </summary>
		private bool ObjectValid => SerializedModel != null && string.IsNullOrEmpty(SaveFileName) == false;

        /// <summary>
        /// Write data to file based on data type.
        /// </summary>
        /// <param name="filePath">Path to write to.</param>
        /// <param name="fileData">Data to write.</param>
        public void WriteToFile(string filePath, dynamic fileData)
        {
            CreateParentDirectoryIfRequired(filePath);
            if (fileData is byte[] bytesData)
            {
                File.WriteAllBytes(CurrentFilePath, bytesData);
            }else if (fileData is string jsonString)
            {
                File.WriteAllText(CurrentFilePath, jsonString);
            }
        }
        
        /// <summary>
        /// Creates a new directory at the file/directory path supplied if it does not exist. 
        /// </summary>
        /// <param name="filePath">File/Directory Path</param>
        /// <exception cref="ArgumentException">If argument is not a path then it'll throw this exception.</exception>
        public static void CreateParentDirectoryIfRequired(string filePath)
        {
            var dirName = Path.GetDirectoryName(filePath);
            if (dirName == null)
            {
                throw new ArgumentException($"Could not get directory name from given path. Path passed: {filePath}");
            }
            Directory.CreateDirectory(dirName);
        }
	}
#endif
}