namespace Fundoo.Api.Requests.Notes;

public sealed record UpdateNoteRequest(
    string Title,
    string? Content);
