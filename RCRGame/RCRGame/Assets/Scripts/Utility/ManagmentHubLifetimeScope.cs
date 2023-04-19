using DefaultNamespace.Core.models;
using UI;
using UI.RecyclableScrollRect;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utility
{
    public class ManagmentHubLifetimeScope : LifetimeScope
    {
        [SerializeField] 
        private MarianaListView _listView;
        [SerializeField] 
        private RecyclableMarinaView _recyclableMarinaView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_recyclableMarinaView);
        }
    }
}