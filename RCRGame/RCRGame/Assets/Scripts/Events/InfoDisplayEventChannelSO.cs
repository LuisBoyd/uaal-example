using UnityEngine;

namespace DefaultNamespace.Events
{
    [CreateAssetMenu(menuName = "RCR/Events/Info Display Event Channel")]
    public class InfoDisplayEventChannelSO : EventRelayThree<long, string, Color>
    {
    }
}