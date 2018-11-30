using System.ComponentModel.DataAnnotations;

namespace LloydsRegister.API.Models
{
    public class ManagingAgentForCreationDto
    {
        [Required(ErrorMessage = "You should provide an Agent Code value.")]
        [RegularExpression("^\\d{4}[A-Z]$", ErrorMessage = "Agent Code should be four numbers followed by a single upper case letter.")]
        public string AgentCode { get; set; }

        [MaxLength(50, ErrorMessage = "Agent Name should be less than 50 characters long.")]
        public string AgentName { get; set; }

    }
}
