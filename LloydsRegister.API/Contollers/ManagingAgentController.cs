using System.Linq;
using LloydsRegister.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LloydsRegister.API.Contollers
{
    [Route("api/managingagents")]
    public class ManagingAgentController : Controller
    {
        [HttpGet]
        public IActionResult GetManagingAgents()
        {
            return Ok(ManagingAgentDataStore.Current.ManagingAgents);
        }

        [HttpGet("{agentCode}", Name = "GetManagingAgent")]
        public IActionResult GetManagingAgent(string agentCode)
        {
            var maToReturn = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
            if (maToReturn == null)
            {
                return NotFound();
            }

            return Ok(maToReturn);
        }

        [HttpPost()]
        public IActionResult CreateManagingAgent(
            [FromBody] ManagingAgentForCreationDto managingAgent)
        {
            if (managingAgent == null)
            {
                return BadRequest();
            }

            if(managingAgent.AgentCode == managingAgent.AgentName)
            {
                ModelState.AddModelError("AgentName", "The provided Agent Name should not be the same as the Agent Code.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agentCode = ManagingAgentDataStore.Current.ManagingAgents.Select(s => s.AgentCode).FirstOrDefault(ma => ma == managingAgent.AgentCode);
            if (agentCode != null)
            {
                return BadRequest();
            }

            var finalManagingAgent = new ManagingAgentDto()
            {
                AgentCode = managingAgent.AgentCode,
                AgentName = managingAgent.AgentName
            };

            ManagingAgentDataStore.Current.ManagingAgents.Add(finalManagingAgent);

            return CreatedAtRoute("GetManagingAgent", new { agentCode = finalManagingAgent.AgentCode}, finalManagingAgent);
        }
    }
}
