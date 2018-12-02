using FluentValidation;

namespace LloydsRegister.API.Models
{
    public class ManagingAgentUpdateDto
    {
        public string AgentCode { get; set; }

        public string AgentName { get; set; }

        public string AgentCodeParameter { get; set; }

    }

    public class ManagingAgentUpdateDtoValidator : AbstractValidator<ManagingAgentUpdateDto>
    {
        public ManagingAgentUpdateDtoValidator()
        {
            // Agent Name Rules
            RuleFor(x => x.AgentName).NotNull().NotEmpty().WithMessage("An Agent Name should be provided.");
            RuleFor(x => x.AgentName).Length(1, 50).WithMessage("The Agent Name should be less than 50 characters long.");

            // Agent Code Parameter
            RuleFor(x => x.AgentCodeParameter).NotEqual(x => x.AgentName).WithMessage("The provided Agent Name should not be the same as the Agent Code.");
            RuleFor(x => x.AgentCode).Equal(x => x.AgentCodeParameter).When(x => x.AgentCode != null).WithMessage("The Agent Code should not be changed.");
        }
    }
}
