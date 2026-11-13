using FluentValidation;
using Fundoo.Application.Features.Notes.Queries;

namespace Fundoo.Application.Features.Notes.Validators;

public sealed class GetNotesQueryValidator : AbstractValidator<GetNotesQuery>
{
    public GetNotesQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}
