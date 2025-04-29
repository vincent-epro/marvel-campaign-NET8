using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class outbound_batch
{
    public int Batch_Id { get; set; }

    public string? Batch_Name { get; set; }

    public string? Batch_Status { get; set; }

    public int? Form_Id { get; set; }

    public string? Channel_Call { get; set; }

    public string? Channel_Email { get; set; }

    public string? Channel_SMS { get; set; }

    public string? Channel_Whatsapp { get; set; }

    public int? Total_Leads { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }

    public string? Batch_Details { get; set; }

    public DateTime? SMS_Delivery_Time { get; set; }

    public string? SMS_Delivery_Status { get; set; }

    public string? SMS_Content { get; set; }

    public DateTime? Email_Delivery_Time { get; set; }

    public string? Email_Delivery_Status { get; set; }

    public string? Email_Subject { get; set; }

    public string? Email_Attachment_Link { get; set; }

    public string? Email_Body_Link { get; set; }

    public string? Whatsapp_Tp_ID { get; set; }

    public string? Whatsapp_Tp_Props { get; set; }

    public DateTime? Whatsapp_Delivery_Time { get; set; }

    public string? Whatsapp_Delivery_Status { get; set; }

    public string? Criteria { get; set; }
}
