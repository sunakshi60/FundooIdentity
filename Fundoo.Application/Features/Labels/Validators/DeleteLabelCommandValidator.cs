using FluentValidation;
using Fundoo.Application.Features.Labels.Commands;

namespace Fundoo.Application.Features.Labels.Validators;

public sealed class DeleteLabelCommandValidator : AbstractValidator<DeleteLabelCommand>
{
    public DeleteLabelCommandValidator()
    {
        RuleFor(command => command.LabelId)
            .NotEmpty();

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
