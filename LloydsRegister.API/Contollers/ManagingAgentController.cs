using System;
using System.Linq;
using LloydsRegister.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LloydsRegister.API.Contollers
{
    [Route("api/managingagents")]
    public class ManagingAgentController : Controller
    {

        private ILogger<ManagingAgentController> _logger;

        public ManagingAgentController(ILogger<ManagingAgentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetManagingAgents()
        {
            try
            {
                return Ok(ManagingAgentDataStore.Current.ManagingAgents);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Exception while getting all Managing Agents.  Exception: {exception}", e);
                return StatusCode(500, "A problem happed while handling your request.");
            }
        }

        [HttpGet("{agentCode}", Name = "GetManagingAgent")]
        public IActionResult GetManagingAgent(string agentCode)
        {
            try
            {
                var agentFromStore = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
                if (agentFromStore == null)
                {
                    _logger.LogInformation("Managing Agent {agentCode} was not found when accessing Managing Agents.", agentCode);
                    return NotFound();
                }

                return Ok(agentFromStore);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Exception while getting Managing Agent with Agent Code {agentCode}.  Exception: {exception}", agentCode, e);
                return StatusCode(500, "A problem happed while handling your request.");
            }
        }

        [HttpPost]
        public IActionResult CreateManagingAgent(
            [FromBody] ManagingAgentCreateDto managingAgent)
        {
            if (managingAgent == null)
            {
                _logger.LogInformation("Create Managing Agent called with a null body content.");
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
                _logger.LogInformation("Create Managing Agent called with Agent Code {agentCode} which already exists.", agentCode);
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

        [HttpDelete("{agentCode}")]
        public IActionResult DeleteManagingAgent(string agentCode)
        {
            var agentFromStore = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
            if (agentFromStore == null)
            {
                return NotFound();
            }

            ManagingAgentDataStore.Current.ManagingAgents.Remove(agentFromStore);

            return NoContent();

        }

    }
}
