using RCR.Patterns;

namespace RCR.Systems
{
    public class GameLogicSystem : SubSystem
    {

        private GameBuildingSystem m_buildingSystem;
        
        public GameLogicSystem()
        {
            m_buildingSystem = new GameBuildingSystem();
        }
        
        public override void Perform()
        {
            throw new System.NotImplementedException();
        }
    }
}