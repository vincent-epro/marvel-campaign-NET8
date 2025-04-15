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

    public virtual DbSet<case_result> case_results { get; set; }

    public virtual DbSet<case_result_log> case_result_logs { get; set; }


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

        modelBuilder.Entity<case_result>(entity =>
        {
            entity.HasKey(e => e.Internal_Case_No);

            entity.ToTable("case_result");

            entity.HasIndex(e => e.Case_No, "IX_Case_No");

            entity.HasIndex(e => e.Conn_Id, "IX_Conn_Id");

            entity.HasIndex(e => e.Created_Time, "IX_Created_Time");

            entity.HasIndex(e => e.Customer_Id, "IX_Customer_Id");

            entity.HasIndex(e => e.Is_Valid, "IX_Is_Valid");

            entity.HasIndex(e => e.Updated_Time, "IX_Updated_Time");

            entity.Property(e => e.Call_Link_File).IsUnicode(false);
            entity.Property(e => e.Call_Nature)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Call_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Case_Flag)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created_Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Inbound_Time).HasColumnType("datetime");
            entity.Property(e => e.Is_Junk_Mail)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Is_Valid)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Long_Call)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Long_Call_Reason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Opened_Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Reply_Call_Result)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Conn_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Time).HasColumnType("datetime");
            entity.Property(e => e.Reply_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ticket_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type_Details).HasMaxLength(150);
            entity.Property(e => e.Updated_Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<case_result_log>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.ToTable("case_result_log");

            entity.HasIndex(e => e.Case_No, "IX_Case_No");

            entity.HasIndex(e => e.Conn_Id, "IX_Conn_Id");

            entity.HasIndex(e => e.Created_Time, "IX_Created_Time");

            entity.HasIndex(e => e.Customer_Id, "IX_Customer_Id");

            entity.HasIndex(e => e.Reply_Type, "IX_Reply_Type");

            entity.HasIndex(e => e.Type_Details, "IX_Type_Details");

            entity.HasIndex(e => e.Updated_Time, "IX_Updated_Time");

            entity.Property(e => e.Call_Link_File).IsUnicode(false);
            entity.Property(e => e.Call_Nature)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Call_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Case_Flag)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Inbound_Time).HasColumnType("datetime");
            entity.Property(e => e.Is_Junk_Mail)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Is_Valid)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Long_Call)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Long_Call_Reason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Opened_Time).HasColumnType("datetime");
            entity.Property(e => e.Reply_Call_Result)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Conn_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Time).HasColumnType("datetime");
            entity.Property(e => e.Reply_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ticket_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type_Details).HasMaxLength(150);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
