using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class call_filter
{
    public int Filter_Id { get; set; }

    public string? Filter_Type { get; set; }

    public string? First_Name { get; set; }

    public string? Last_Name { get; set; }

    public string? Title { get; set; }

    public string? Mobile_No { get; set; }

    public string? Other_Phone_No { get; set; }

    public string? Email { get; set; }

    public string? Address_Line { get; set; }

    public string? Remark { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }

    public string? Is_Valid { get; set; }
}
