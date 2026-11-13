namespace Fundoo.Application.Features.Labels.Commands;

public sealed record RenameLabelCommand(Guid LabelId, string UserId, string Name);
