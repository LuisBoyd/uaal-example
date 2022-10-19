using System;
using Newtonsoft.Json.Linq;

namespace DataStructures
{
    public class Player
    {
        public string m_email { get; private set; }
        public DateTime m_lastlogin { get; private set; }
        public int m_currentCurrency { get; private set; }
        public int m_currentPremiumCurrency { get; private set; }

        public Player(string json)
        {
            JObject playerjson = JObject.Parse(json);
            if (playerjson != null)
            {
                m_email = playerjson["result"]["Name"].ToString();
                m_currentCurrency = playerjson["result"]["Money"].ToObject<int>();
                m_currentPremiumCurrency = playerjson["result"]["PremiumCurrency"].ToObject<int>();

                m_lastlogin = DateTime.ParseExact(playerjson["result"]["LastLogin"].ToString(), 
                    "yyyy-MM-dd", null);
            }
        }
    }
}