using RCR.Settings;

namespace BuildingComponents
{
    public class TicketPriceUpgrade : Upgrade
    {
        private float TicketCost
        {
            get
            {
                return m_upgradeLevel * GameSettings.Base_Ticket_Cost;
            }
        }
        
        public override void Level_Up()
        {
            throw new System.NotImplementedException();
            //TODO: Would need to look at a max per level situation here so have something validating that this is fine to upgrade
            //Would probably be a validation check before this method is called
            m_upgradeLevel += 1;
        }
        
        public override float Calculate_Output(float current, int intake)
        {
            return current + (intake * TicketCost);
        }

        public override void reset_level()
        {
            m_upgradeLevel = 1;
        }

        public TicketPriceUpgrade(int level) : base(level)
        {
        }
    }
}