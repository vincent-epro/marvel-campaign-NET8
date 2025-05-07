using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_sales_order
{
    public int Sa_Id { get; set; }

    public string? Batch_Code { get; set; }

    public string? Campaign_Code { get; set; }

    public int? Call_Id { get; set; }

    public string? Product_Code { get; set; }

    public string? Plan_Code { get; set; }

    public string? Price { get; set; }

    public string? Order_Status { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }
}
