using System;

namespace AI
{
    public class AgentAIeventArgs : EventArgs
    {
        public AiAgent agentArg;
        public AgentAIeventArgs(AiAgent arg) => this.agentArg = arg;
    }
}