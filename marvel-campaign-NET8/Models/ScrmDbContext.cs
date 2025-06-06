﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace marvel_campaign_NET8.Models;

public partial class ScrmDbContext : DbContext
{
    public ScrmDbContext(DbContextOptions<ScrmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<call_filter> call_filters { get; set; }

    public virtual DbSet<call_history> call_histories { get; set; }

    public virtual DbSet<case_reminder> case_reminders { get; set; }

    public virtual DbSet<case_result> case_results { get; set; }

    public virtual DbSet<case_result_log> case_result_logs { get; set; }

    public virtual DbSet<contact_list> contact_lists { get; set; }

    public virtual DbSet<contact_list_log> contact_list_logs { get; set; }

    public virtual DbSet<def_market> def_markets { get; set; }

    public virtual DbSet<def_nationality> def_nationalities { get; set; }

    public virtual DbSet<def_profile> def_profiles { get; set; }

    public virtual DbSet<field> fields { get; set; }

    public virtual DbSet<field_option> field_options { get; set; }

    public virtual DbSet<new_case_no> new_case_nos { get; set; }

    public virtual DbSet<ob_assignment_history> ob_assignment_histories { get; set; }

    public virtual DbSet<ob_batch> ob_batches { get; set; }

    public virtual DbSet<ob_campaign> ob_campaigns { get; set; }

    public virtual DbSet<ob_header_mapping> ob_header_mappings { get; set; }

    public virtual DbSet<ob_input_form> ob_input_forms { get; set; }

    public virtual DbSet<ob_result> ob_results { get; set; }

    public virtual DbSet<ob_result_log> ob_result_logs { get; set; }

    public virtual DbSet<ob_sales_order> ob_sales_orders { get; set; }

    public virtual DbSet<ob_temp_upload> ob_temp_uploads { get; set; }

    public virtual DbSet<outbound_batch> outbound_batches { get; set; }

    public virtual DbSet<outbound_call_result> outbound_call_results { get; set; }

    public virtual DbSet<outbound_call_result_log> outbound_call_result_logs { get; set; }

    public virtual DbSet<outbound_input_form> outbound_input_forms { get; set; }

    public virtual DbSet<reply_details_history> reply_details_histories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<call_filter>(entity =>
        {
            entity.HasKey(e => e.Filter_Id);

            entity.ToTable("call_filter");

            entity.Property(e => e.Address_Line).HasMaxLength(500);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Filter_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.First_Name).HasMaxLength(200);
            entity.Property(e => e.Is_Valid)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Last_Name).HasMaxLength(200);
            entity.Property(e => e.Mobile_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Other_Phone_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

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

        modelBuilder.Entity<case_reminder>(entity =>
        {
            entity.HasKey(e => new { e.Reminder_Id, e.Case_No });

            entity.ToTable("case_reminder");

            entity.HasIndex(e => e.Created_By, "IX_Created_By");

            entity.HasIndex(e => e.Is_Read, "IX_Is_Read");

            entity.HasIndex(e => e.Scheduled_Time, "IX_Scheduled_Time");

            entity.Property(e => e.Reminder_Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Is_Read)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Scheduled_Time).HasColumnType("datetime");
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

        modelBuilder.Entity<contact_list>(entity =>
        {
            entity.HasKey(e => e.Customer_Id);

            entity.ToTable("contact_list");

            entity.HasIndex(e => new { e.Home_No, e.Office_No, e.Mobile_No, e.Fax_No, e.Other_Phone_No }, "IX_All_Phones");

            entity.HasIndex(e => e.Email, "IX_Email");

            entity.HasIndex(e => e.Is_Valid, "IX_Is_Valid");

            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.Address3).HasMaxLength(50);
            entity.Property(e => e.Address4).HasMaxLength(50);
            entity.Property(e => e.Agree_To_Disclose_Info)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Created_Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DOB).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Facebook_Id).HasMaxLength(100);
            entity.Property(e => e.Fax_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Full_Address).HasMaxLength(200);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Home_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ID_No)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Is_Valid)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Lang).HasMaxLength(50);
            entity.Property(e => e.Mobile_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name_Chi).HasMaxLength(250);
            entity.Property(e => e.Name_Eng).HasMaxLength(250);
            entity.Property(e => e.Occupation).HasMaxLength(50);
            entity.Property(e => e.Office_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Opened_Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Other_Phone_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Photo_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Updated_Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Wechat_Id).HasMaxLength(100);
            entity.Property(e => e.Whatsapp_Id).HasMaxLength(100);
        });

        modelBuilder.Entity<contact_list_log>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.ToTable("contact_list_log");

            entity.HasIndex(e => e.Customer_Id, "IX_Customer_Id");

            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.Address3).HasMaxLength(50);
            entity.Property(e => e.Address4).HasMaxLength(50);
            entity.Property(e => e.Agree_To_Disclose_Info)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.DOB).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Facebook_Id).HasMaxLength(100);
            entity.Property(e => e.Fax_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Full_Address).HasMaxLength(200);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Home_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ID_No)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Is_Valid)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Lang).HasMaxLength(50);
            entity.Property(e => e.Mobile_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name_Chi).HasMaxLength(250);
            entity.Property(e => e.Name_Eng).HasMaxLength(250);
            entity.Property(e => e.Occupation).HasMaxLength(50);
            entity.Property(e => e.Office_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Opened_Time).HasColumnType("datetime");
            entity.Property(e => e.Other_Phone_No)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Photo_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
            entity.Property(e => e.Wechat_Id).HasMaxLength(100);
            entity.Property(e => e.Whatsapp_Id).HasMaxLength(100);
        });

        modelBuilder.Entity<def_market>(entity =>
        {
            entity.HasKey(e => e.MarketID).HasName("PK_DefMarket");

            entity.ToTable("def_market");

            entity.Property(e => e.MarketLocation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MarketName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MarketName_TC).HasMaxLength(50);
            entity.Property(e => e.isValid)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("Y")
                .IsFixedLength();
        });

        modelBuilder.Entity<def_nationality>(entity =>
        {
            entity.HasKey(e => e.NationalityID).HasName("PK_DefNationality");

            entity.ToTable("def_nationality");

            entity.Property(e => e.NationalityID).ValueGeneratedNever();
            entity.Property(e => e.NationalityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NationalityName_TC).HasMaxLength(100);
            entity.Property(e => e.isValid)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("Y")
                .IsFixedLength();
        });

        modelBuilder.Entity<def_profile>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_DefProfile");

            entity.ToTable("def_profile");

            entity.Property(e => e.Profile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Profile_TC).HasMaxLength(50);
            entity.Property(e => e.isValid)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("Y")
                .IsFixedLength();
        });

        modelBuilder.Entity<field>(entity =>
        {
            entity.HasKey(e => new { e.Field_Category, e.Field_Name });

            entity.ToTable("field");

            entity.Property(e => e.Field_Category).HasMaxLength(50);
            entity.Property(e => e.Field_Name).HasMaxLength(50);
            entity.Property(e => e.Field_Display).HasMaxLength(50);
            entity.Property(e => e.Field_Tag).HasMaxLength(50);
            entity.Property(e => e.Field_Type).HasMaxLength(50);
        });

        modelBuilder.Entity<field_option>(entity =>
        {
            entity.HasKey(e => new { e.Field_Name, e.Field_Option }).HasName("PK_Field_Option");

            entity.ToTable("field_option");

            entity.Property(e => e.Field_Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Field_Option).HasMaxLength(50);
        });

        modelBuilder.Entity<new_case_no>(entity =>
        {
            entity.HasKey(e => e.Case_No);

            entity.ToTable("new_case_no");

            entity.Property(e => e.Time_Stamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_assignment_history>(entity =>
        {
            entity.HasKey(e => e.A_Id);

            entity.ToTable("ob_assignment_history");

            entity.Property(e => e.Created_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_batch>(entity =>
        {
            entity.HasKey(e => e.B_Id);

            entity.ToTable("ob_batch");

            entity.Property(e => e.Batch_Code).HasMaxLength(100);
            entity.Property(e => e.Batch_End_Date).HasColumnType("datetime");
            entity.Property(e => e.Batch_Start_Date).HasColumnType("datetime");
            entity.Property(e => e.Batch_Status).HasMaxLength(20);
            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_campaign>(entity =>
        {
            entity.HasKey(e => e.Campaign_Id);

            entity.ToTable("ob_campaign");

            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.Campaign_Description).HasMaxLength(1000);
            entity.Property(e => e.Campaign_Status).HasMaxLength(20);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Form_Name).HasMaxLength(200);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_header_mapping>(entity =>
        {
            entity.HasKey(e => e.H_Id);

            entity.ToTable("ob_header_mapping");

            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.Check_Type).HasMaxLength(20);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.DB_Field_Name).HasMaxLength(100);
            entity.Property(e => e.Excel_Field_Name).HasMaxLength(200);
        });

        modelBuilder.Entity<ob_input_form>(entity =>
        {
            entity.HasKey(e => e.Form_Id);

            entity.ToTable("ob_input_form");

            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Form_Name).HasMaxLength(200);
            entity.Property(e => e.Form_Status).HasMaxLength(20);
        });

        modelBuilder.Entity<ob_result>(entity =>
        {
            entity.HasKey(e => e.Call_Id);

            entity.ToTable("ob_result");

            entity.HasIndex(e => e.Agent_Id, "IX_Agent_Id");

            entity.HasIndex(e => e.Batch_Code, "IX_Batch_Code");

            entity.HasIndex(e => e.Campaign_Code, "IX_Campaign_Code");

            entity.Property(e => e.Batch_Code).HasMaxLength(100);
            entity.Property(e => e.Call_Reason).HasMaxLength(200);
            entity.Property(e => e.Call_Status).HasMaxLength(200);
            entity.Property(e => e.Callback_Time).HasColumnType("datetime");
            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.DOB).HasColumnType("datetime");
            entity.Property(e => e.First_Name).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.Home_No).HasMaxLength(100);
            entity.Property(e => e.Join_Date).HasColumnType("datetime");
            entity.Property(e => e.Last_Name).HasMaxLength(100);
            entity.Property(e => e.Mobile_No).HasMaxLength(100);
            entity.Property(e => e.Office_No).HasMaxLength(100);
            entity.Property(e => e.Opt_Out).HasMaxLength(20);
            entity.Property(e => e.Other_Phone_No).HasMaxLength(100);
            entity.Property(e => e.Plan_Code).HasMaxLength(100);
            entity.Property(e => e.Product_Code).HasMaxLength(100);
            entity.Property(e => e.Reply_Conn_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Transaction_Time).HasColumnType("datetime");
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_result_log>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.ToTable("ob_result_log");

            entity.HasIndex(e => e.Call_Id, "IX_Call_Id");

            entity.Property(e => e.Batch_Code).HasMaxLength(100);
            entity.Property(e => e.Call_Reason).HasMaxLength(200);
            entity.Property(e => e.Call_Status).HasMaxLength(200);
            entity.Property(e => e.Callback_Time).HasColumnType("datetime");
            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.DOB).HasColumnType("datetime");
            entity.Property(e => e.First_Name).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.Home_No).HasMaxLength(100);
            entity.Property(e => e.Join_Date).HasColumnType("datetime");
            entity.Property(e => e.Last_Name).HasMaxLength(100);
            entity.Property(e => e.Mobile_No).HasMaxLength(100);
            entity.Property(e => e.Office_No).HasMaxLength(100);
            entity.Property(e => e.Opt_Out).HasMaxLength(20);
            entity.Property(e => e.Other_Phone_No).HasMaxLength(100);
            entity.Property(e => e.Plan_Code).HasMaxLength(100);
            entity.Property(e => e.Product_Code).HasMaxLength(100);
            entity.Property(e => e.Reply_Conn_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Transaction_Time).HasColumnType("datetime");
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_sales_order>(entity =>
        {
            entity.HasKey(e => e.Sa_Id);

            entity.ToTable("ob_sales_order");

            entity.HasIndex(e => e.Call_Id, "IX_Call_Id");

            entity.Property(e => e.Batch_Code).HasMaxLength(100);
            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Order_Status).HasMaxLength(20);
            entity.Property(e => e.Plan_Code).HasMaxLength(100);
            entity.Property(e => e.Price).HasMaxLength(100);
            entity.Property(e => e.Product_Code).HasMaxLength(100);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<ob_temp_upload>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ob_temp_upload");

            entity.HasIndex(e => e.Campaign_Code, "IX_Campaign_Code");

            entity.Property(e => e.Campaign_Code).HasMaxLength(100);
            entity.Property(e => e.DOB).HasMaxLength(1000);
            entity.Property(e => e.First_Name).HasMaxLength(1000);
            entity.Property(e => e.Gender).HasMaxLength(1000);
            entity.Property(e => e.Home_No).HasMaxLength(1000);
            entity.Property(e => e.Join_Date).HasMaxLength(1000);
            entity.Property(e => e.Last_Name).HasMaxLength(1000);
            entity.Property(e => e.Mobile_No).HasMaxLength(1000);
            entity.Property(e => e.Office_No).HasMaxLength(1000);
            entity.Property(e => e.Other_Phone_No).HasMaxLength(1000);
        });

        modelBuilder.Entity<outbound_batch>(entity =>
        {
            entity.HasKey(e => e.Batch_Id);

            entity.ToTable("outbound_batch");

            entity.Property(e => e.Batch_Name).HasMaxLength(200);
            entity.Property(e => e.Batch_Status).HasMaxLength(20);
            entity.Property(e => e.Channel_Call).HasMaxLength(10);
            entity.Property(e => e.Channel_Email).HasMaxLength(10);
            entity.Property(e => e.Channel_SMS).HasMaxLength(10);
            entity.Property(e => e.Channel_Whatsapp).HasMaxLength(10);
            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Email_Delivery_Status).HasMaxLength(50);
            entity.Property(e => e.Email_Delivery_Time).HasColumnType("datetime");
            entity.Property(e => e.Email_Subject).HasMaxLength(1000);
            entity.Property(e => e.SMS_Delivery_Status).HasMaxLength(50);
            entity.Property(e => e.SMS_Delivery_Time).HasColumnType("datetime");
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
            entity.Property(e => e.Whatsapp_Delivery_Status).HasMaxLength(50);
            entity.Property(e => e.Whatsapp_Delivery_Time).HasColumnType("datetime");
            entity.Property(e => e.Whatsapp_Tp_ID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<outbound_call_result>(entity =>
        {
            entity.HasKey(e => e.Call_Lead_Id);

            entity.ToTable("outbound_call_result");

            entity.HasIndex(e => e.Agent_Id, "IX_Agent_Id");

            entity.HasIndex(e => e.Batch_Id, "IX_Batch_Id");

            entity.HasIndex(e => e.Customer_Id, "IX_Customer_Id");

            entity.HasIndex(e => e.Opt_Out, "IX_Opt_Out");

            entity.Property(e => e.Call_Reason).HasMaxLength(200);
            entity.Property(e => e.Call_Status).HasMaxLength(200);
            entity.Property(e => e.Callback_Time).HasColumnType("datetime");
            entity.Property(e => e.Opt_Out).HasMaxLength(20);
            entity.Property(e => e.Reply_Conn_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Transaction_Time).HasColumnType("datetime");
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<outbound_call_result_log>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.ToTable("outbound_call_result_log");

            entity.HasIndex(e => e.Call_Lead_Id, "IX_Call_Lead_Id");

            entity.Property(e => e.Call_Reason).HasMaxLength(200);
            entity.Property(e => e.Call_Status).HasMaxLength(200);
            entity.Property(e => e.Callback_Time).HasColumnType("datetime");
            entity.Property(e => e.Opt_Out).HasMaxLength(20);
            entity.Property(e => e.Reply_Conn_Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Transaction_Time).HasColumnType("datetime");
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<outbound_input_form>(entity =>
        {
            entity.HasKey(e => e.Form_Id);

            entity.ToTable("outbound_input_form");

            entity.Property(e => e.Created_Time).HasColumnType("datetime");
            entity.Property(e => e.Form_Name).HasMaxLength(200);
            entity.Property(e => e.Form_Status).HasMaxLength(20);
        });

        modelBuilder.Entity<reply_details_history>(entity =>
        {
            entity.HasKey(e => e.R_Id);

            entity.ToTable("reply_details_history");

            entity.HasIndex(e => e.Reply_Details, "IX_Reply_Details");

            entity.HasIndex(e => e.Reply_Type, "IX_Reply_Type");

            entity.Property(e => e.Reply_Details)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Reply_Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Updated_Time).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
