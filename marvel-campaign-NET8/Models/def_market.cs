using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class def_market
{
    public int MarketID { get; set; }

    public string? MarketName { get; set; }

    public string? MarketName_TC { get; set; }

    public int Sort { get; set; }

    public string isValid { get; set; } = null!;

    public string? MarketLocation { get; set; }

    public int RptSort { get; set; }
}
