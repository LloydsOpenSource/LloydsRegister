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
                    Code = "0001A",
                    Name = "MA01"
                },
                new ManagingAgentDto()
                {
                    Code = "0002B",
                    Name = "MA02"
                },
                new ManagingAgentDto()
                {
                    Code = "0003C",
                    Name = "MA03"
                },
                new ManagingAgentDto()
                {
                    Code = "0004D",
                    Name = "MA04"
                },
            };
        }
    }
}
