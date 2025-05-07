using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_campaign
{
    public int Campaign_Id { get; set; }

    public string? Campaign_Code { get; set; }

    public int? Form_Id { get; set; }

    public string? Form_Name { get; set; }

    public string? Campaign_Description { get; set; }

    public string? Campaign_Status { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }
}
