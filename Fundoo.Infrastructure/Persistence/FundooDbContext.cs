using Fundoo.Domain.Entities;
using Fundoo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.Infrastructure.Persistence;

public sealed class FundooDbContext(DbContextOptions<FundooDbContext> options)
    : IdentityDbContext<FundooUser>(options)
{
    public DbSet<Note> Notes => Set<Note>();

    public DbSet<Label> Labels => Set<Label>();

    public DbSet<NoteLabel> NoteLabels => Set<NoteLabel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(note => note.Id);

            entity.Property(note => note.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(note => note.Content)
                .IsRequired();

            entity.Property(note => note.UserId)
                .IsRequired();

            entity.HasOne<FundooUser>()
                .WithMany()
                .HasForeignKey(note => note.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(note => new { note.UserId, note.IsTrashed, note.IsArchived });
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(label => label.Id);

            entity.Property(label => label.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(label => label.NormalizedName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(label => label.UserId)
                .IsRequired();

            entity.HasOne<FundooUser>()
                .WithMany()
                .HasForeignKey(label => label.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(label => new { label.UserId, label.NormalizedName })
                .IsUnique();
        });

        modelBuilder.Entity<NoteLabel>(entity =>
        {
            entity.HasKey(noteLabel => new { noteLabel.NoteId, noteLabel.LabelId });

            entity.HasOne(noteLabel => noteLabel.Note)
                .WithMany(note => note.NoteLabels)
                .HasForeignKey(noteLabel => noteLabel.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(noteLabel => noteLabel.Label)
                .WithMany(label => label.NoteLabels)
                .HasForeignKey(noteLabel => noteLabel.LabelId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
