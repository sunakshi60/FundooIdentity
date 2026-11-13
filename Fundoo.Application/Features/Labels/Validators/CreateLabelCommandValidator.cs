using FluentValidation;
using Fundoo.Application.Features.Labels.Commands;

namespace Fundoo.Application.Features.Labels.Validators;

public sealed class CreateLabelCommandValidator : AbstractValidator<CreateLabelCommand>
{
    public CreateLabelCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Label name is required.")
            .MaximumLength(100)
            .WithMessage("Label name cannot exceed 100 characters.");
    }
}
