using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class def_nationality
{
    public int NationalityID { get; set; }

    public string? NationalityName { get; set; }

    public string? NationalityName_TC { get; set; }

    public int? MarketID { get; set; }

    public int? ProfileID { get; set; }

    public int Sort { get; set; }

    public string isValid { get; set; } = null!;
}
