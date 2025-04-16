using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class case_reminder
{
    public int Reminder_Id { get; set; }

    public int Case_No { get; set; }

    public string? Is_Read { get; set; }

    public string? Remarks { get; set; }

    public DateTime? Scheduled_Time { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }
}
