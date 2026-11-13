using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Fundoo.Api.Requests.Notes;
using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Handlers;
using Fundoo.Application.Features.Notes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/notes")]
public sealed class NotesController(
    ICreateNoteHandler createNoteHandler,
    IValidator<CreateNoteCommand> createNoteValidator,
    IGetNotesHandler getNotesHandler,
    IValidator<GetNotesQuery> getNotesValidator,
    IGetTrashedNotesHandler getTrashedNotesHandler,
    IValidator<GetTrashedNotesQuery> getTrashedNotesValidator,
    IGetArchivedNotesHandler getArchivedNotesHandler,
    IValidator<GetArchivedNotesQuery> getArchivedNotesValidator,
    IGetNoteByIdHandler getNoteByIdHandler,
    IValidator<GetNoteByIdQuery> getNoteByIdValidator,
    IUpdateNoteHandler updateNoteHandler,
    IValidator<UpdateNoteCommand> updateNoteValidator,
    ITrashNoteHandler trashNoteHandler,
    IValidator<TrashNoteCommand> trashNoteValidator,
    IRestoreNoteHandler restoreNoteHandler,
    IValidator<RestoreNoteCommand> restoreNoteValidator,
    ISetNoteArchiveStatusHandler setNoteArchiveStatusHandler,
    IValidator<SetNoteArchiveStatusCommand> setNoteArchiveStatusValidator,
    ISetNoteLabelAssociationHandler setNoteLabelAssociationHandler,
    IValidator<SetNoteLabelAssociationCommand> setNoteLabelAssociationValidator) : ControllerBase
{
    [HttpPut("{noteId:guid}/labels/{labelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> AttachLabel(
        [FromRoute] Guid noteId,
        [FromRoute] Guid labelId,
        CancellationToken cancellationToken)
    {
        return SetNoteLabelAssociation(noteId, labelId, isAttached: true, cancellationToken);
    }

    [HttpDelete("{noteId:guid}/labels/{labelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> RemoveLabel(
        [FromRoute] Guid noteId,
        [FromRoute] Guid labelId,
        CancellationToken cancellationToken)
    {
        return SetNoteLabelAssociation(noteId, labelId, isAttached: false, cancellationToken);
    }

    private async Task<IActionResult> SetNoteLabelAssociation(
        Guid noteId,
        Guid labelId,
        bool isAttached,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new SetNoteLabelAssociationCommand(
            noteId,
            labelId,
            userId,
            isAttached);
        var validationResult = await setNoteLabelAssociationValidator.ValidateAsync(
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

        var updated = await setNoteLabelAssociationHandler.HandleAsync(
            command,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpGet("archived")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetArchivedNotes(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var query = new GetArchivedNotesQuery(userId);
        var validationResult = await getArchivedNotesValidator.ValidateAsync(
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

        var notes = await getArchivedNotesHandler.HandleAsync(query, cancellationToken);
        return Ok(notes);
    }

    [HttpPatch("{noteId:guid}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> ArchiveNote(
        [FromRoute] Guid noteId,
        CancellationToken cancellationToken)
    {
        return SetNoteArchiveStatus(noteId, isArchived: true, cancellationToken);
    }

    [HttpPatch("{noteId:guid}/unarchive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> UnarchiveNote(
        [FromRoute] Guid noteId,
        CancellationToken cancellationToken)
    {
        return SetNoteArchiveStatus(noteId, isArchived: false, cancellationToken);
    }

    private async Task<IActionResult> SetNoteArchiveStatus(
        Guid noteId,
        bool isArchived,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new SetNoteArchiveStatusCommand(noteId, userId, isArchived);
        var validationResult = await setNoteArchiveStatusValidator.ValidateAsync(
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

        var updated = await setNoteArchiveStatusHandler.HandleAsync(
            command,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{noteId:guid}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreNote(
        [FromRoute] Guid noteId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new RestoreNoteCommand(noteId, userId);
        var validationResult = await restoreNoteValidator.ValidateAsync(
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

        var restored = await restoreNoteHandler.HandleAsync(command, cancellationToken);
        return restored ? NoContent() : NotFound();
    }

    [HttpGet("trash")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTrashedNotes(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var query = new GetTrashedNotesQuery(userId);
        var validationResult = await getTrashedNotesValidator.ValidateAsync(
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

        var notes = await getTrashedNotesHandler.HandleAsync(query, cancellationToken);
        return Ok(notes);
    }

    [HttpDelete("{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TrashNote(
        [FromRoute] Guid noteId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new TrashNoteCommand(noteId, userId);
        var validationResult = await trashNoteValidator.ValidateAsync(
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

        var trashed = await trashNoteHandler.HandleAsync(command, cancellationToken);
        return trashed ? NoContent() : NotFound();
    }

    [HttpPut("{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNote(
        [FromRoute] Guid noteId,
        [FromBody] UpdateNoteRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new UpdateNoteCommand(
            noteId,
            userId,
            request.Title,
            request.Content);
        var validationResult = await updateNoteValidator.ValidateAsync(
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

        var result = await updateNoteHandler.HandleAsync(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNoteById(
        [FromRoute] Guid noteId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var query = new GetNoteByIdQuery(noteId, userId);
        var validationResult = await getNoteByIdValidator.ValidateAsync(
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

        var note = await getNoteByIdHandler.HandleAsync(query, cancellationToken);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNotes(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var query = new GetNotesQuery(userId);
        var validationResult = await getNotesValidator.ValidateAsync(
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

        var notes = await getNotesHandler.HandleAsync(query, cancellationToken);
        return Ok(notes);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNote(
        [FromBody] CreateNoteRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var command = new CreateNoteCommand(userId, request.Title, request.Content);
        var validationResult = await createNoteValidator.ValidateAsync(
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

        var result = await createNoteHandler.HandleAsync(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }
}
