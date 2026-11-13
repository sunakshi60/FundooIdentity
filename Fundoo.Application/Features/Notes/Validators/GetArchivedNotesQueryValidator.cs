using FluentValidation;
using Fundoo.Application.Features.Notes.Queries;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class GetArchivedNotesQueryValidator : AbstractValidator<GetArchivedNotesQuery>
{
    public GetArchivedNotesQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}
