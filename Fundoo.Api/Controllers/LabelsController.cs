using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Fundoo.Api.Requests.Labels;
using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Handlers;
using Fundoo.Application.Features.Labels.Queries;
using Fundoo.Application.Features.Labels.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/labels")]
public sealed class LabelsController(
    ICreateLabelHandler createLabelHandler,
    IValidator<CreateLabelCommand> createLabelValidator,
    IGetLabelsHandler getLabelsHandler,
    IValidator<GetLabelsQuery> getLabelsValidator,
    IRenameLabelHandler renameLabelHandler,
    IValidator<RenameLabelCommand> renameLabelValidator,
    IDeleteLabelHandler deleteLabelHandler,
    IValidator<DeleteLabelCommand> deleteLabelValidator) : ControllerBase
{
    [HttpPut("{labelId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RenameLabel(
        [FromRoute] Guid labelId,
        [FromBody] RenameLabelRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new RenameLabelCommand(labelId, userId, request.Name);
        var validationResult = await renameLabelValidator.ValidateAsync(
            command,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return CreateValidationProblem(validationResult);
        }

        var result = await renameLabelHandler.HandleAsync(command, cancellationToken);

        return result.Status switch
        {
            RenameLabelStatus.Succeeded => Ok(result.Label),
            RenameLabelStatus.DuplicateName => Conflict(new
            {
                message = "A label with this name already exists."
            }),
            _ => NotFound()
        };
    }

    [HttpDelete("{labelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLabel(
        [FromRoute] Guid labelId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new DeleteLabelCommand(labelId, userId);
        var validationResult = await deleteLabelValidator.ValidateAsync(
            command,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return CreateValidationProblem(validationResult);
        }

        var deleted = await deleteLabelHandler.HandleAsync(command, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateLabel(
        [FromBody] CreateLabelRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new CreateLabelCommand(userId, request.Name);
        var validationResult = await createLabelValidator.ValidateAsync(
            command,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray());

            return BadRequest(new ValidationProblemDetails(errors));
        }

        var result = await createLabelHandler.HandleAsync(command, cancellationToken);

        if (!result.Succeeded)
        {
            return Conflict(new { message = result.ErrorMessage });
        }

        return StatusCode(StatusCodes.Status201Created, result.Label);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetLabels(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var query = new GetLabelsQuery(userId);
        var validationResult = await getLabelsValidator.ValidateAsync(
            query,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray());

            return BadRequest(new ValidationProblemDetails(errors));
        }

        var labels = await getLabelsHandler.HandleAsync(query, cancellationToken);
        return Ok(labels);
    }

    private BadRequestObjectResult CreateValidationProblem(FluentValidation.Results.ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        return BadRequest(new ValidationProblemDetails(errors));
    }
}
