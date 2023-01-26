using DataStructures;
using RCR.Enums;

namespace RCR.BaseClasses
{
    public delegate void DelegateNoArg();

    public delegate void SendCMD(string cmd);
    
    
    // Delegates on the native Bridge

    /// <summary>
    /// When the user has left a area surrounding a POI
    /// Exposed to API
    /// </summary>
    public delegate void OnOutOfPOIRange();

    /// <summary>
    /// OnVerificationRequest - Trigger Expected when user attempts to load instance of game.
    /// Exposed to API
    /// </summary>
    public delegate void OnVerificationRequest();

    /// <summary>
    /// Result of Verification Process
    /// </summary>
    public delegate void OnVerificationResult(bool result);

    public delegate void OnQuit();
    

    public delegate void OnObservableValueChanged<P, C>(P previousValue, C currentValue);
    


}