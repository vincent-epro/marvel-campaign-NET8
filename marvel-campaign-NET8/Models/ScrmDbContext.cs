using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace marvel_campaign_NET8.Models;

public partial class ScrmDbContext : DbContext
{
    public ScrmDbContext()
    {
    }

    public ScrmDbContext(DbContextOptions<ScrmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<call_history> call_histories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<call_history>(entity =>
        {
            entity.HasKey(e => e.Conn_Id);

            entity.ToTable("call_history");

            entity.HasIndex(e => e.Is_Saved, "IX_Is_Saved");

            entity.HasIndex(e => e.Updated_By, "IX_Updated_By");

            entity.HasIndex(e => e.Updated_Time, "IX_Updated_Time");

            entity.Property(e => e.Conn_Id).ValueGeneratedNever();
            entity.Property(e => e.Call_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Is_Saved)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type_Details).HasMaxLength(150);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
