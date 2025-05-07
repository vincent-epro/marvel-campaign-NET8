using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_header_mapping
{
    public int H_Id { get; set; }

    public int? Campaign_Id { get; set; }

    public string? Campaign_Code { get; set; }

    public int? Excel_Field_Order { get; set; }

    public string? Excel_Field_Name { get; set; }

    public string? DB_Field_Name { get; set; }

    public string? Check_Type { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }
}
