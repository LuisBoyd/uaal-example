using RCR.Patterns;

namespace RCR.Systems
{
    public class GameFacade<T1,T2>  : BaseFacade 
        where T1 : SubSystem, new()
        where T2 : SubSystem, new()
    {
        private T1 m_gameLogicSystem;
        private T2 m_gameVisualSystem;

        public GameFacade()
        {
            m_gameLogicSystem = new T1();
            m_gameVisualSystem = new T2();
        }

        public void Update()
        {
            m_gameLogicSystem.Perform();
            m_gameVisualSystem.Perform();
        }
    }
}