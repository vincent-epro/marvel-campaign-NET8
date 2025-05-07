using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_batch
{
    public int B_Id { get; set; }

    public string? Batch_Code { get; set; }

    public string? Campaign_Code { get; set; }

    public DateTime? Batch_Start_Date { get; set; }

    public DateTime? Batch_End_Date { get; set; }

    public string? Batch_Status { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }
}
