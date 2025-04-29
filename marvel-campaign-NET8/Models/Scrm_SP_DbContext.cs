using System;
using System.Collections.Generic;
using marvel_campaign_NET8.Controllers;
using Microsoft.EntityFrameworkCore;

namespace marvel_campaign_NET8.Models;

public partial class Scrm_SP_DbContext : DbContext
{
    public Scrm_SP_DbContext(DbContextOptions<Scrm_SP_DbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerJourney> CustomerJourney_sp { get; set; }

    public DbSet<Dashboard_CallNature> Dashboard_CallNature_sp { get; set; }

    public DbSet<Dashboard_Agent_CallNature> Dashboard_Agent_CallNature_sp { get; set; }

    public DbSet<OutboundBatchAssignment> OutboundBatchAssignment_sp { get; set; }

    public DbSet<OutboundBatchAssignment_Agent> OutboundBatchAssignment_Agent_sp { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerJourney>().HasNoKey();

        modelBuilder.Entity<Dashboard_CallNature>().HasNoKey();

        modelBuilder.Entity<Dashboard_Agent_CallNature>().HasNoKey();

        modelBuilder.Entity<OutboundBatchAssignment>().HasNoKey();

        modelBuilder.Entity<OutboundBatchAssignment_Agent>().HasNoKey();


        OnModelCreatingPartial(modelBuilder);

    }
            

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}



public class CustomerJourney
{
    public string? Call_Type { get; set; }
    public int? Conn_Id { get; set; }
    public DateTime? Inbound_Time { get; set; }
    public string? Type_Details { get; set; }
    public string? Reply_Type { get; set; }
    public string? Reply_Conn_Id { get; set; }
    public DateTime? Reply_Time { get; set; }
    public string? Reply_Details { get; set; }
    public DateTime? Updated_Time { get; set; }
    public int? Case_No { get; set; }
    public string? Details { get; set; }
}

public class Dashboard_CallNature
{
    public string? Description { get; set; }
}

public class Dashboard_Agent_CallNature
{
    public int? Agent_Id { get; set; }
    public int? Complaint { get; set; }
    public int? Compliment { get; set; }
    public int? Enquiry { get; set; }
    public int? Feedback { get; set; }
    public int? Subtotal { get; set; }
}

public class OutboundBatchAssignment
{
    public int? Total { get; set; }
    public int? Assigned { get; set; }
    public int? Unassigned { get; set; }
}

public class OutboundBatchAssignment_Agent
{
    public string? SellerID { get; set; }
    public int? Agent_Id { get; set; }
    public string? AgentName { get; set; }
    public int? Assigned { get; set; }
    public int? Unused { get; set; }
}