using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class field
{
    public string Field_Category { get; set; } = null!;

    public string Field_Name { get; set; } = null!;

    public string? Field_Display { get; set; }

    public string? Field_Tag { get; set; }

    public string? Field_Type { get; set; }
}
