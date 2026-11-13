using FluentValidation;
using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class SetNoteArchiveStatusCommandValidator
    : AbstractValidator<SetNoteArchiveStatusCommand>
{
    public SetNoteArchiveStatusCommandValidator()
    {
        RuleFor(command => command.NoteId)
            .NotEmpty();

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
