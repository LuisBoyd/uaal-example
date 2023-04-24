using System;
using System.Collections.Generic;
using Core.Services.Network;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
using UnityEngine;
using VContainer;
using System.Linq;
using Core.Services.Gameplay;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.RecyclableScrollRect
{
    public class RecyclableMarinaView : BaseMonoBehavior, IRecyclableScrollRectDataSource
    {
        [SerializeField]
            RecyclableScrollRect _recyclableScrollRect;

            private MarianaCollection _collection;
            private MarinaBuyService _marinaBuyService;
            private List<Mariana> _MutableMarianaList;
            private bool FilterOwned = false;
            private bool FilterUnowned = false;
            private string _currentSearchString;

            [SerializeField]
            private int _dataLength;

            [Header("Listening To")]
            [SerializeField] private EventRelay OnMarinaSetUpdated;
            [SerializeField]private StringEventChannelSO OnTextSearchFilterChanged;
            [SerializeField] private BoolEventChannelSO OnFilterOwnedChanged;
            [SerializeField] private BoolEventChannelSO OnFilterUnownedChanged;
            [SerializeField] private EventRelay OnSuccessfulMarinaBuy;
            
            private void Awake()
            {
                _MutableMarianaList = new List<Mariana>();
                _recyclableScrollRect.DataSource = this;
            }

            private void OnEnable()
            {
                OnMarinaSetUpdated.onEventRaised += OnNewMarinaDataSet;
                OnTextSearchFilterChanged.onEventRaised += FilterResults;
                OnFilterOwnedChanged.onEventRaised += ChangeOwnedFilter;
                OnFilterUnownedChanged.onEventRaised += ChangeUnownedFilter;
                OnSuccessfulMarinaBuy.onEventRaised += OnNewMarinaDataSet;
            }

            private void OnDisable()
            {
                OnMarinaSetUpdated.onEventRaised -= OnNewMarinaDataSet;
                OnTextSearchFilterChanged.onEventRaised -= FilterResults;
                OnFilterOwnedChanged.onEventRaised -= ChangeOwnedFilter;
                OnFilterUnownedChanged.onEventRaised -= ChangeUnownedFilter;
                OnSuccessfulMarinaBuy.onEventRaised -= OnNewMarinaDataSet;
            }
        
            [Inject]
            private void InjectUser(MarianaCollection collection)
            {
                _collection = collection;
            }

            private void ChangeOwnedFilter(bool value)
            {
                FilterOwned = value;
                FilterResults(_currentSearchString);
            }

            private void ChangeUnownedFilter(bool value)
            {
                FilterUnowned = value;
                FilterResults(_currentSearchString);
            }

            #region Event-Stuff
            private void FilterResults(string value)
            {
                _currentSearchString = value;
                if (string.IsNullOrEmpty(_currentSearchString))
                {
                    if (FilterOwned && !FilterUnowned)
                    {
                        _MutableMarianaList = _collection.ReadonlyMarinaList.Where(x => x.OwnStatus == true).ToList();
                    }
                    else if (FilterUnowned && !FilterOwned)
                    {
                        _MutableMarianaList = _collection.ReadonlyMarinaList.Where(x => x.OwnStatus == false).ToList();
                    }
                    else
                    {
                        _MutableMarianaList = _collection.ReadonlyMarinaList;
                    }
                    _recyclableScrollRect.ReloadData();
                    return;
                }
                
                if (FilterOwned && !FilterUnowned)
                {
                    _MutableMarianaList = _collection.ReadonlyMarinaList.Where(x => x.OwnStatus == true && x.Name.ToLower().StartsWith(_currentSearchString.ToLower())).ToList();
                }
                else if (FilterUnowned && !FilterOwned)
                {
                    _MutableMarianaList = _collection.ReadonlyMarinaList.Where(x => x.OwnStatus == false && x.Name.ToLower().StartsWith(_currentSearchString.ToLower())).ToList();
                }
                else
                {
                    _MutableMarianaList = _collection.ReadonlyMarinaList.Where(x => x.Name.ToLower().StartsWith(_currentSearchString.ToLower())).ToList();
                }
                _recyclableScrollRect.ReloadData();
            }

            private void OnNewMarinaDataSet()
            {
                _currentSearchString = String.Empty;
                FilterOwned = false;
                FilterUnowned = false;
                _MutableMarianaList = _collection.ReadonlyMarinaList;
                _recyclableScrollRect.ReloadData();
            }
            #endregion
            
            #region DATA-SOURCE
        
            /// <summary>
            /// Data source method. return the list length.
            /// </summary>
            public int GetItemCount()
            {
                return _MutableMarianaList.Count;
            }
        
            /// <summary>
            /// Data source method. Called for a cell every time it is recycled.
            /// Implement this method to do the necessary cell configuration.
            /// </summary>
            public void SetCell(ICell cell, int index)
            {
                //Casting to the implemented Cell
                var item = cell as MarianaCell;
                item.ConfigureCell(_MutableMarianaList[index], index);
            }
        
            #endregion
    }
}