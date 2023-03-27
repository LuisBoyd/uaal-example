
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.Input;
namespace RCRCoreLib.Core.Input
{
    public interface IInteractable
    {
        public bool IsInteractable { get; set; }
        public IInteractable Self { get; }
        void SetUpListeners()
        {
            IsInteractable = true;
            EventManager.Instance.AddListener<SetInputState>(On_SetInputState);
        }

        void On_SetInputState(SetInputState evnt)
        {
            IsInteractable = evnt.state;
        }

        void RemoveListeners()
        {
            EventManager.Instance.RemoveListener<SetInputState>(On_SetInputState);
        }
    }
}