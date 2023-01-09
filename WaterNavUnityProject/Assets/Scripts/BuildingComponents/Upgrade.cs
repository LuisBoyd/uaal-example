namespace BuildingComponents
{
    public abstract class Upgrade
    {
        public int UpgradeLevel
        {
            get => m_upgradeLevel;
        }
        protected int m_upgradeLevel;

        public int Prestigelevel
        {
            get => m_prestigelevel;
        }
        protected int m_prestigelevel;
        public Upgrade(int level)
        {
            this.m_upgradeLevel = level;
        }

        public void UpgradePrestiage() //Only use when Building is Evolving
        {
            m_prestigelevel++;
            m_upgradeLevel = 1;
        }

        public abstract void Level_Up();
        public abstract float Calculate_Output(float current, int intake);

        public abstract void reset_level();
    }
}