using System.ComponentModel.DataAnnotations;

namespace LloydsRegister.API.Models
{
    public class ManagingAgentCreateDto
    {
        [Required(ErrorMessage = "An Agent Code should be provided.")]
        [RegularExpression("^\\d{4}[A-Z]$", ErrorMessage = "The Agent Code should be four numbers followed by a single upper case letter.")]
        public string AgentCode { get; set; }

        [Required( ErrorMessage = "An Agent Name should be provided.")]
        [MaxLength(50, ErrorMessage = "The Agent Name should be less than 50 characters long.")]
        public string AgentName { get; set; }

    }
}
