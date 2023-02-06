using System;
using System.IO;
using Cysharp.Threading.Tasks;
using RCR.Settings.SuperNewScripts.SaveSystem.Interfaces;
using UnityEngine;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileSavers
{
    public abstract class Saver<T> : IFileSaver<T>
    {
        

        protected Saver()
        {
           
        }

        public virtual bool CanWrite
        {
            get => true;

        }


        public virtual async UniTaskVoid WriteToJson(TextWriter txtWriter, T value)
        {
        }
       

        public virtual async UniTaskVoid WriteToFile(string location, T value)
        {
            if(!CanWrite)
                return;
            var fullPath = Path.Combine(Application.persistentDataPath, location);
            try
            {
                //Either Overwirte a current File or 
                using (StreamWriter writer = File.CreateText(fullPath))
                { 
                    WriteToJson(writer,value).Forget();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}