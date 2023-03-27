namespace RCRCoreLib.Core.UI.UISystem
{
    public class BuildingCardViewMenu : UIMenuRoot
    {
        public override void Open()
        {
            
        }

        public override void Close()
        {
           
        }

        public override void Activate()
        {
            this.gameObject.SetActive(true);
            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public override void DeActivate()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
           
        }
    }
}