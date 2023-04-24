using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using VContainer.Unity;
using Timer = System.Timers.Timer;

namespace Core.Services.persistence
{
    public abstract class BaseSaver<T> : ITickable, IDisposable where T : INotifyPropertyChanged
    {
        protected readonly FileStream _fileStream;
        protected readonly StreamWriter _streamWriter;
        protected readonly T Data;
        protected readonly InternalSetting _setting;
        protected readonly NetworkClient _client;
        
        protected bool WrittenChange { get; set; }
        protected bool ShouldWriteChange { get; set; }
        protected bool CanWrite { get; set; }
        
        /// <summary>
        /// RemoteSaveTimeElapsed time before data is sent to a remote source (Milliseconds)
        /// </summary>
        protected float RemoteSaveTimeElapsed { get; private set; } 

        protected BaseSaver(T data,NetworkClient client, InternalSetting setting)
        {
            CanWrite = true;
            WrittenChange = false;
            ShouldWriteChange = false;
            try
            {
                Data = data;
                _setting = setting;
                if(setting.UserDataLocalSavePath == null)
                    throw new NullReferenceException();
                _fileStream = File.Create(_setting.UserDataLocalSavePath);
                _streamWriter = new StreamWriter(_fileStream);
                _client = client;
                Data.PropertyChanged += On_PropertyChange;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                CanWrite = false;
                throw;
            }
        }
        private void On_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            ShouldWriteChange = true;
        }

        public void Dispose()
        {
            Data.PropertyChanged -= On_PropertyChange;
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Data));
            _streamWriter.Write(Convert.ToBase64String(data));
            _streamWriter.Flush();
            _streamWriter.Close();
            _fileStream.Close();
            _streamWriter.Dispose();
            _fileStream.Dispose();
        }

        public void Tick()
        {
            if(!CanWrite) return;
            if (RemoteSaveTimeElapsed >= _setting.RemoteSaveTimeMilliSeconds)
            {
                WriteToRemoteLocation().Forget(ExceptionHandler);
                RemoteSaveTimeElapsed = 0f; //reset timer
            }
            else
            {
                RemoteSaveTimeElapsed += (Time.deltaTime * 1000f); //Convert Time.delta Time to milliseconds
            }

            if (WrittenChange)
            {
                ShouldWriteChange = false;
                WrittenChange = false;
            }
        }

        private void ExceptionHandler(Exception e)
        {
            Debug.LogError(e.Message);
        }
        protected abstract UniTask WriteToRemoteLocation();
    }
}