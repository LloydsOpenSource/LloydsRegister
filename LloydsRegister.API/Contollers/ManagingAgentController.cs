using System.Linq;
using LloydsRegister.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost]
        public IActionResult CreateManagingAgent(
            [FromBody] ManagingAgentCreateDto managingAgent)
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

        [HttpPut("{agentCode}")]
        public IActionResult UpdateManagingAgent(string agentCode,
            [FromBody] ManagingAgentUpdateDto managingAgent)
        {
            if (managingAgent == null)
            {
                return BadRequest();
            }

            if(agentCode == managingAgent.AgentName)
            {
                ModelState.AddModelError("agentName", "The provided Agent Name should not be the same as the Agent Code.");
            }

            if(managingAgent.AgentCode != null && managingAgent.AgentCode != agentCode)
            {
                ModelState.AddModelError("agentCode", "The Agent Code should not be changed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var agent = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
            if (agent == null)
            {
                return NotFound();
            }

            agent.AgentName = managingAgent.AgentName;

            return NoContent();
        }

        [HttpPatch("{agentCode}")]
        public IActionResult PartiallyUpdateManagingAgent(string agentCode,
            [FromBody] JsonPatchDocument<ManagingAgentUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var agentFromStore = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
            if (agentFromStore == null)
            {
                return NotFound();
            }

            var agentToPatch = new ManagingAgentUpdateDto()
            {
                AgentCode = agentFromStore.AgentCode,
                AgentName = agentFromStore.AgentName
            };

            patchDoc.ApplyTo(agentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(agentCode == agentToPatch.AgentName)
            {
                ModelState.AddModelError("agentName", "The provided Agent Name should not be the same as the Agent Code.");
            }

            if(agentToPatch.AgentCode != null && agentToPatch.AgentCode != agentCode)
            {
                ModelState.AddModelError("agentCode", "The Agent Code should not be changed.");
            }

            if(agentToPatch.AgentCode == null)
            {
                ModelState.AddModelError("agentCode", "The Agent Code should not be removed.");
            }

            TryValidateModel(agentToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            agentFromStore.AgentName = agentToPatch.AgentName;

            return NoContent();
        }

    }
}
