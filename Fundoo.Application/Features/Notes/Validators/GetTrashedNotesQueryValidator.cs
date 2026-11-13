using FluentValidation;
using Fundoo.Application.Features.Notes.Queries;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class GetTrashedNotesQueryValidator : AbstractValidator<GetTrashedNotesQuery>
{
    public GetTrashedNotesQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}
