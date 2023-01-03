namespace BuildingComponents
{
    public abstract class Upgrade
    {
        public int UpgradeLevel
        {
            get => m_upgradeLevel;
        }
        protected int m_upgradeLevel;

        public Upgrade(int level)
        {
            this.m_upgradeLevel = level;
        }

        public abstract void Level_Up();
        public abstract float Calculate_Output(float current, int intake);

        public abstract void reset_level();
    }
}