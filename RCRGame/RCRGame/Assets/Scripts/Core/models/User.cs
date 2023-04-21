using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace.Core.models
{
    public class User : INotifyPropertyChanged
    {
        private int _user_id = 7; //Default value for testing TODO take out in production
        public int User_id
        {
            get => _user_id;
            set => SetField<int>(ref _user_id, value);
        }
        
        private string _username;
        [CanBeNull]
        public string Username
        {
            get => string.IsNullOrEmpty(_username) ? "" : _username;
            set => SetField<string>(ref _username, value);
        }

        private int _level;
        public int Level
        {
            get => _level;
            set => SetField<int>(ref _level, value);

        }
        private int _current_exp;
        public int Current_Exp
        {
            get => _current_exp;
            set => SetField<int>(ref _current_exp, value);

        }
        private int _freemium_currency;

        public int Freemium_Currency
        {
            get => _freemium_currency;
            set => SetField<int>(ref _freemium_currency, value);
        }
        private int _premium_currency;
        public int Premium_Currency
        {
            get => _premium_currency;
            set => SetField<int>(ref _premium_currency, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}