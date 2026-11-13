namespace Fundoo.Api.Requests.Notes;

public sealed record CreateNoteRequest(
    string Title,
    string? Content);
