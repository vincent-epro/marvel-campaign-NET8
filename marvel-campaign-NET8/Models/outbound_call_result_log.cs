using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class outbound_call_result_log
{
    public int LogID { get; set; }

    public int? Call_Lead_Id { get; set; }

    public int? Batch_Id { get; set; }

    public int? Form_Id { get; set; }

    public int? Customer_Id { get; set; }

    public int? Attempt { get; set; }

    public int? Conn_Id { get; set; }

    public int? Agent_Id { get; set; }

    public DateTime? Transaction_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }

    public string? Call_Status { get; set; }

    public string? Call_Reason { get; set; }

    public DateTime? Callback_Time { get; set; }

    public string? Reply_Conn_Id { get; set; }

    public string? Reply_Details { get; set; }

    public string? Opt_Out { get; set; }

    public string? Remark { get; set; }
}
