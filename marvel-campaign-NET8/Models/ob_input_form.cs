using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_input_form
{
    public int Form_Id { get; set; }

    public string? Form_Name { get; set; }

    public string? Form_Status { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public string? Form_Details { get; set; }
}
