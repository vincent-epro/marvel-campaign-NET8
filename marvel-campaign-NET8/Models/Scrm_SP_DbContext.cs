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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerJourney>().HasNoKey();

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

