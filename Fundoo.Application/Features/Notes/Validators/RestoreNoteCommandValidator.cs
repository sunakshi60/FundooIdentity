using FluentValidation;
using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class RestoreNoteCommandValidator : AbstractValidator<RestoreNoteCommand>
{
    public RestoreNoteCommandValidator()
    {
        RuleFor(command => command.NoteId)
            .NotEmpty();

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
