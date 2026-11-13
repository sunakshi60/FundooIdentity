using Fundoo.Infrastructure.Identity;
using Fundoo.Infrastructure.Email;
using Fundoo.Infrastructure.Persistence;
using Fundoo.Infrastructure.Services.Authentication;
using Fundoo.Infrastructure.Authentication.Google;
using Fundoo.Infrastructure.Authentication.Tokens;
using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Handlers;
using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Repositories;
using Fundoo.Application.Features.Notes.Services;
using Fundoo.Application.Features.Notes.Validators;
using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Handlers;
using Fundoo.Application.Features.Labels.Queries;
using Fundoo.Application.Features.Labels.Repositories;
using Fundoo.Application.Features.Labels.Services;
using Fundoo.Application.Features.Labels.Validators;
using Fundoo.Application.Features.Authentication.Handlers;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Infrastructure.Persistence.Repositories;
using Fundoo.Infrastructure.Services.Notes;
using Fundoo.Infrastructure.Services.Labels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundoo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FundooDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'FundooDatabase' is not configured.");
        }

        services.AddDbContext<FundooDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddDataProtection();

        services.Configure<SmtpOptions>(
            configuration.GetSection(SmtpOptions.SectionName));
        services.Configure<GoogleOptions>(
            configuration.GetSection(GoogleOptions.SectionName));
        services.Configure<FundooTokenOptions>(
            configuration.GetSection(FundooTokenOptions.SectionName));

        services
            .AddIdentityCore<FundooUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<FundooDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<IGoogleIdTokenValidator, GoogleIdTokenValidator>();
        services.AddScoped<IGoogleSignInService, GoogleSignInService>();
        services.AddSingleton<IFundooAccessTokenIssuer, FundooAccessTokenIssuer>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IRegisterUserHandler, RegisterUserHandler>();
        services.AddScoped<ILoginUserHandler, LoginUserHandler>();
        services.AddScoped<IGoogleSignInHandler, GoogleSignInHandler>();
        services.AddScoped<IForgotPasswordHandler, ForgotPasswordHandler>();
        services.AddScoped<IResetPasswordHandler, ResetPasswordHandler>();
        services.AddScoped<IValidator<CreateNoteCommand>, CreateNoteCommandValidator>();
        services.AddScoped<ICreateNoteHandler, CreateNoteHandler>();
        services.AddScoped<IValidator<UpdateNoteCommand>, UpdateNoteCommandValidator>();
        services.AddScoped<IUpdateNoteHandler, UpdateNoteHandler>();
        services.AddScoped<IValidator<TrashNoteCommand>, TrashNoteCommandValidator>();
        services.AddScoped<ITrashNoteHandler, TrashNoteHandler>();
        services.AddScoped<IValidator<RestoreNoteCommand>, RestoreNoteCommandValidator>();
        services.AddScoped<IRestoreNoteHandler, RestoreNoteHandler>();
        services.AddScoped<IValidator<SetNoteArchiveStatusCommand>, SetNoteArchiveStatusCommandValidator>();
        services.AddScoped<ISetNoteArchiveStatusHandler, SetNoteArchiveStatusHandler>();
        services.AddScoped<IValidator<SetNoteLabelAssociationCommand>, SetNoteLabelAssociationCommandValidator>();
        services.AddScoped<ISetNoteLabelAssociationHandler, SetNoteLabelAssociationHandler>();
        services.AddScoped<IValidator<CreateLabelCommand>, CreateLabelCommandValidator>();
        services.AddScoped<IValidator<GetLabelsQuery>, GetLabelsQueryValidator>();
        services.AddScoped<IValidator<RenameLabelCommand>, RenameLabelCommandValidator>();
        services.AddScoped<IValidator<DeleteLabelCommand>, DeleteLabelCommandValidator>();
        services.AddScoped<ICreateLabelHandler, CreateLabelHandler>();
        services.AddScoped<IGetLabelsHandler, GetLabelsHandler>();
        services.AddScoped<IRenameLabelHandler, RenameLabelHandler>();
        services.AddScoped<IDeleteLabelHandler, DeleteLabelHandler>();
        services.AddScoped<ILabelService, LabelService>();
        services.AddScoped<ILabelRepository, LabelRepository>();
        services.AddScoped<IValidator<GetNotesQuery>, GetNotesQueryValidator>();
        services.AddScoped<IGetNotesHandler, GetNotesHandler>();
        services.AddScoped<IValidator<GetTrashedNotesQuery>, GetTrashedNotesQueryValidator>();
        services.AddScoped<IGetTrashedNotesHandler, GetTrashedNotesHandler>();
        services.AddScoped<IValidator<GetArchivedNotesQuery>, GetArchivedNotesQueryValidator>();
        services.AddScoped<IGetArchivedNotesHandler, GetArchivedNotesHandler>();
        services.AddScoped<IValidator<GetNoteByIdQuery>, GetNoteByIdQueryValidator>();
        services.AddScoped<IGetNoteByIdHandler, GetNoteByIdHandler>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<INoteRepository, NoteRepository>();

        return services;
    }
}
