using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BelotApp.Models;

public partial class BelotNotesContext : DbContext
{
    public BelotNotesContext()
    {
    }

    public BelotNotesContext(DbContextOptions<BelotNotesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GameResult> GameResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.;Database=BelotNotes;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameResu__3214EC076DB218BB");

            entity.Property(e => e.Combination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
