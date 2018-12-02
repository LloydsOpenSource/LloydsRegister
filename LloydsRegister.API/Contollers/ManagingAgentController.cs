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
                var errorTrace = Guid.NewGuid();
                _logger.LogCritical("Exception while getting all Managing Agents.  Error trace: {errorTrace}, Exception: {exception}", e);
                return StatusCode(500, $"A problem happed while handling your request.  Error trace: {errorTrace}, Date and time: {DateTime.UtcNow}");
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
                    _logger.LogInformation("Managing Agent {agentCode} was not found when getting Managing Agents.", agentCode);
                    return NotFound();
                }

                return Ok(agentFromStore);
            }
            catch (Exception e)
            {
                var errorTrace = Guid.NewGuid();
                _logger.LogCritical("Exception while getting Managing Agent.  Error trace: {errorTrace}, Exception: {exception}", errorTrace, e);
                return StatusCode(500, $"A problem happed while handling your request.  Error trace: {errorTrace}, Date and time: {DateTime.UtcNow}");
            }
        }

        [HttpPost]
        public IActionResult CreateManagingAgent(
            [FromBody] ManagingAgentCreateDto managingAgent)
        {
            try
            {
                var validator = new ManagingAgentCreateDtoValidator();

                if (managingAgent == null)
                {
                    _logger.LogInformation("{logName}: Create Managing Agent called with a null body content.", "Object Validation");
                    return BadRequest();
                }

                var validationResult = validator.Validate(managingAgent);
                if (!validationResult.IsValid)
                {
                    foreach(var failure in validationResult.Errors)
                    {
                        _logger.LogInformation("{logName}: Create Managing Agent failed with an invalid {object} of {attemptedValue}.  The error message was: {error}", "Object Validation", failure.PropertyName, failure.AttemptedValue, failure.ErrorMessage);
                    }
                    return BadRequest(validationResult);
                }

                var agentCode = ManagingAgentDataStore.Current.ManagingAgents.Select(s => s.AgentCode).FirstOrDefault(ma => ma == managingAgent.AgentCode);
                if (agentCode != null)
                {
                    _logger.LogInformation("{logName}: Create Managing Agent called with {object} of {attemptedValue} which already exists.", "Object Validation", "AgentCode", agentCode);
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
            catch (Exception e)
            {
                var errorTrace = Guid.NewGuid();
                _logger.LogCritical("Exception while creating a Managing Agent.  Error trace: {errorTrace}  Exception: {exception}", errorTrace, e);
                return StatusCode(500, $"A problem happed while handling your request.  Error trace: {errorTrace}, Date and time: {DateTime.UtcNow}");
            }
        }

        [HttpPut("{agentCode}")]
        public IActionResult UpdateManagingAgent(string agentCode,
            [FromBody] ManagingAgentUpdateDto managingAgent)
        {
            try
            {
                var validator = new ManagingAgentUpdateDtoValidator();

                if (managingAgent == null)
                {
                    _logger.LogInformation("Update Managing Agent called with a null body content.");
                    return BadRequest();
                }

                managingAgent.AgentCodeParameter = agentCode;
                var validationResult = validator.Validate(managingAgent);

                if (!validationResult.IsValid)
                {
                    foreach(var failure in validationResult.Errors)
                    {
                        _logger.LogInformation("{logName}: Update Managing Agent failed with an invalid {object} of {attemptedValue}.  The error message was: {error}", "Object Validation", failure.PropertyName, failure.AttemptedValue, failure.ErrorMessage);
                    }
                    return BadRequest(validationResult);
                }
                
                var agent = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
                if (agent == null)
                {
                    _logger.LogInformation("Managing Agent {agentCode} was not found when updating Managing Agent.", agentCode);
                    return NotFound();
                }

                agent.AgentName = managingAgent.AgentName;

                return NoContent();

            }
            catch (Exception e)
            {
                var errorTrace = Guid.NewGuid();
                _logger.LogCritical("Exception while updating a Managing Agent.  Error trace: {errorTrace}  Exception: {exception}", errorTrace, e);
                return StatusCode(500, $"A problem happed while handling your request.  Error trace: {errorTrace}, Date and time: {DateTime.UtcNow}");
            }
        }

        [HttpPatch("{agentCode}")]
        public IActionResult PartiallyUpdateManagingAgent(string agentCode,
            [FromBody] JsonPatchDocument<ManagingAgentUpdateDto> patchDoc)
        {
            try
            {
                var validator = new ManagingAgentUpdateDtoValidator();

                if (patchDoc == null)
                {
                    _logger.LogInformation("Partial update of Managing Agent called with a null body content.");
                    return BadRequest();
                }

                var agentFromStore = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
                if (agentFromStore == null)
                {
                    _logger.LogInformation("Managing Agent {agentCode} was not found when partially updating Managing Agent.", agentCode);
                    return NotFound();
                }

                var agentToPatch = new ManagingAgentUpdateDto()
                {
                    AgentCode = agentFromStore.AgentCode,
                    AgentName = agentFromStore.AgentName,
                    AgentCodeParameter = agentCode
                };

                patchDoc.ApplyTo(agentToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Partial update of Managing Agent with Agent Code {agentCode} called with an invalid model state.  Keys: {keys}, Model State: {@modelState}", agentCode, ModelState.Keys, ModelState);
                    return BadRequest(ModelState);
                }

                var validationResult = validator.Validate(agentToPatch);

                if (!validationResult.IsValid)
                {
                    foreach(var failure in validationResult.Errors)
                    {
                        _logger.LogInformation("{logName}: Partial update of Managing Agent failed with an invalid {object} of {attemptedValue}.  The error message was: {error}", "Object Validation", failure.PropertyName, failure.AttemptedValue, failure.ErrorMessage);
                    }
                    return BadRequest(validationResult);
                }

                agentFromStore.AgentName = agentToPatch.AgentName;

                return NoContent();

            }
            catch (Exception e)
            {
                var errorTrace = Guid.NewGuid();
                _logger.LogCritical("Exception while partially updating a Managing Agent.  Error trace: {errorTrace}  Exception: {exception}", errorTrace, e);
                return StatusCode(500, $"A problem happed while handling your request.  Error trace: {errorTrace}, Date and time: {DateTime.UtcNow}");
            }
        }

        [HttpDelete("{agentCode}")]
        public IActionResult DeleteManagingAgent(string agentCode)
        {
            try
            {
                var agentFromStore = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.AgentCode == agentCode);
                if (agentFromStore == null)
                {
                    _logger.LogInformation("Managing Agent {agentCode} was not found when deleting Managing Agent.", agentCode);
                    return NotFound();
                }

                ManagingAgentDataStore.Current.ManagingAgents.Remove(agentFromStore);

                return NoContent();

            }
            catch (Exception e)
            {
                var errorTrace = Guid.NewGuid();
                _logger.LogCritical("Exception while partially updating a Managing Agent.  Error trace: {errorTrace}  Exception: {exception}", errorTrace, e);
                return StatusCode(500, $"A problem happed while handling your request.  Error trace: {errorTrace}, Date and time: {DateTime.UtcNow}");
            }

        }

    }
}
