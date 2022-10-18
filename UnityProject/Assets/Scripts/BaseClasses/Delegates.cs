using DataStructures;

namespace RCR.BaseClasses
{
    public delegate void DelegateNoArg();

    public delegate void SendCMD(string cmd);

    public delegate void OnPlayerJoin(Player player);

    public delegate void OnPlayerLeave(Player player);
}