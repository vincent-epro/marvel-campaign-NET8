using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_assignment_history
{
    public int A_Id { get; set; }

    public string? Assignment_Details { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }
}
