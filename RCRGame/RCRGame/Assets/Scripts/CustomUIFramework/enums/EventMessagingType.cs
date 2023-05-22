namespace CustomUIFramework.enums
{
    /// <summary>
    /// How is the UI going to send messages via the SObjects event system (custom implementation)
    /// or the unity action system.
    /// </summary>
    public enum EventMessagingType
    {
        SObjects = 0,
        UnityAction = 1
    }
}