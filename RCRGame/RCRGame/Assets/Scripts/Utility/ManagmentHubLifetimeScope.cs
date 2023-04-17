using DefaultNamespace.Core.models;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utility
{
    public class ManagmentHubLifetimeScope : LifetimeScope
    {
        [SerializeField] 
        private MarianaListView _listView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_listView);
            builder.RegisterEntryPoint<MarianaCollection>();
        }
    }
}