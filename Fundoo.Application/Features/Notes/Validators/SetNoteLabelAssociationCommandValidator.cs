using FluentValidation;
using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class SetNoteLabelAssociationCommandValidator
    : AbstractValidator<SetNoteLabelAssociationCommand>
{
    public SetNoteLabelAssociationCommandValidator()
    {
        RuleFor(command => command.NoteId)
            .NotEmpty();

        RuleFor(command => command.LabelId)
            .NotEmpty();

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
