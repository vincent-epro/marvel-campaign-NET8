using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class reply_details_history
{
    public int R_Id { get; set; }

    public int? Customer_Id { get; set; }

    public int? Case_No { get; set; }

    public string? Reply_Type { get; set; }

    public string? Reply_Details { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }
}
