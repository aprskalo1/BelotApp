using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BelotApp.Models;

namespace BelotApp.Models;

public partial class NotesDbContext : DbContext
{
    public NotesDbContext()
    {
    }

    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameResult> GameResults { get; set; }

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=.;Database=BelotNotes;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Game__3214EC07F15C6A7B");

            entity.ToTable("Game");

            entity.Property(e => e.PlayedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TeamOneName).HasMaxLength(50);
            entity.Property(e => e.TeamTwoName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<GameResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameResu__3214EC076DB218BB");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Game).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__GameResul__GameI__74AE54BC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<BelotApp.Models.AspNetUsers> AspNetUser { get; set; } = default!;
}
