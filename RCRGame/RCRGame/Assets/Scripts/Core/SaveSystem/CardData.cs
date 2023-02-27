using System.Collections.Generic;

namespace RCRCoreLib.Core.SaveSystem
{
    
    public class CardData : Data
    {
        public Dictionary<int, int> CardIDownedAmount
            = new Dictionary<int, int>();
    }
}