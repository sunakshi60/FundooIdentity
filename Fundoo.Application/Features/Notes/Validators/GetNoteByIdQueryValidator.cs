using FluentValidation;
using Fundoo.Application.Features.Notes.Queries;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class GetNoteByIdQueryValidator : AbstractValidator<GetNoteByIdQuery>
{
    public GetNoteByIdQueryValidator()
    {
        RuleFor(query => query.NoteId)
            .NotEmpty();

        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}
