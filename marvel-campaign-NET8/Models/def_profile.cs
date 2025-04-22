using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class def_profile
{
    public int ID { get; set; }

    public string? Profile { get; set; }

    public string? Profile_TC { get; set; }

    public string isValid { get; set; } = null!;
}
