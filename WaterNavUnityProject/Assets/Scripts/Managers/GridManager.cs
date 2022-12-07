using System.Collections;
using DataStructures;
using Interfaces;
using RCR.BaseClasses;
using RCR.Enums;

namespace RCR.Managers
{
    public class GridManager : Singelton<GridManager>, Iinitializer
    {
        private GridCellValue[] m_grid;
        
        public IEnumerator Process_Init(CoroutineToken tkn)
        {
            throw new System.NotImplementedException();
        }

        private void CreateGrid()
        {
            
        }

        public void Init_CleanUp()
        {
            throw new System.NotImplementedException();
        }
    }
}