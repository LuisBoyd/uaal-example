using RCR.Settings;

namespace BuildingComponents
{
    public class LengthOfQueueUpgrade: Upgrade
    {
        public int Queue_Capcity
        {
            get
            {
                return m_upgradeLevel * GameSettings.Base_Queue_Length;
            }
        }
        
        public LengthOfQueueUpgrade(int level) : base(level)
        {
        }

        public override void Level_Up()
        {
            m_upgradeLevel += 1;
        }

        public override float Calculate_Output(float current, int intake)
        {
            return current;
        }

        public override void reset_level()
        {
            m_upgradeLevel = 1;
        }
    }
}