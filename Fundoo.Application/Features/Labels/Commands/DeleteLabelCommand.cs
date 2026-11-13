namespace Fundoo.Application.Features.Labels.Commands;

public sealed record DeleteLabelCommand(Guid LabelId, string UserId);
