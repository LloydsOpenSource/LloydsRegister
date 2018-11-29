using System.Linq;
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

        [HttpGet("{code}")]
        public IActionResult GetManagingAgent(string code)
        {
            var maToReturn = ManagingAgentDataStore.Current.ManagingAgents.FirstOrDefault(ma => ma.Code == code);
            if (maToReturn == null)
            {
                return NotFound();
            }

            return Ok(maToReturn);
        }
    }
}
