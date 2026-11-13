using FluentValidation;
using Fundoo.Application.Features.Labels.Queries;

namespace Fundoo.Application.Features.Labels.Validators;

public sealed class GetLabelsQueryValidator : AbstractValidator<GetLabelsQuery>
{
    public GetLabelsQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}
