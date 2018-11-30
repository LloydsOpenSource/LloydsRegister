using System.ComponentModel.DataAnnotations;

namespace LloydsRegister.API.Models
{
    public class ManagingAgentUpdateDto
    {
        public string AgentCode { get; set; }

        [Required( ErrorMessage = "An Agent Name should be provided.")]
        [MaxLength(50, ErrorMessage = "The Agent Name should be less than 50 characters long.")]
        public string AgentName { get; set; }

    }
}
