using FluentValidation;

namespace LloydsRegister.API.Models
{
    public class ManagingAgentCreateDto
    {
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
    }

    public class ManagingAgentCreateDtoValidator : AbstractValidator<ManagingAgentCreateDto>
    {
        public ManagingAgentCreateDtoValidator()
        {
            // Agent Code Rules
            RuleFor(x => x.AgentCode).NotNull().WithMessage("An Agent Code should be provided.");
            RuleFor(x => x.AgentCode).NotEqual(x => x.AgentName).WithMessage("The provided Agent Name should not be the same as the Agent Code.");
            RuleFor(x => x.AgentCode).Matches("^\\d{4}[A-Z]$").WithMessage("The Agent Code should be four numbers followed by a single upper case letter.");

            // Agent Name Rules
            RuleFor(x => x.AgentName).NotNull().NotEmpty().WithMessage("An Agent Name should be provided.");
            RuleFor(x => x.AgentName).Length(1, 50).WithMessage("The Agent Name should be less than 50 characters long.");
        }
    }
}
