using System.Collections.Generic;
using LloydsRegister.API.Models;

namespace LloydsRegister.API
{
    public class ManagingAgentDataStore
    {
        public static ManagingAgentDataStore Current { get; } = new ManagingAgentDataStore();

        public List<ManagingAgentDto> ManagingAgents { get; set; }

        public ManagingAgentDataStore()
        {
            // init dummy data
            ManagingAgents = new List<ManagingAgentDto>()
            {
                new ManagingAgentDto()
                {
                    AgentCode = "0001A",
                    AgentName = "MA01"
                },
                new ManagingAgentDto()
                {
                    AgentCode = "0002B",
                    AgentName = "MA02"
                },
                new ManagingAgentDto()
                {
                    AgentCode = "0003C",
                    AgentName = "MA03"
                },
                new ManagingAgentDto()
                {
                    AgentCode = "0004D",
                    AgentName = "MA04"
                },
            };
        }
    }
}
