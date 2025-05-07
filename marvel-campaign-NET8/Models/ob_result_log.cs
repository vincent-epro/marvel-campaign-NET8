using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class ob_result_log
{
    public int LogID { get; set; }

    public int? Call_Id { get; set; }

    public string? Batch_Code { get; set; }

    public string? Campaign_Code { get; set; }

    public int? Attempt { get; set; }

    public int? Conn_Id { get; set; }

    public int? Agent_Id { get; set; }

    public DateTime? Transaction_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }

    public string? First_Name { get; set; }

    public string? Last_Name { get; set; }

    public string? Gender { get; set; }

    public DateTime? DOB { get; set; }

    public string? Mobile_No { get; set; }

    public string? Office_No { get; set; }

    public string? Home_No { get; set; }

    public string? Other_Phone_No { get; set; }

    public DateTime? Join_Date { get; set; }

    public string? Call_Status { get; set; }

    public string? Call_Reason { get; set; }

    public string? Product_Code { get; set; }

    public string? Plan_Code { get; set; }

    public DateTime? Callback_Time { get; set; }

    public string? Reply_Conn_Id { get; set; }

    public string? Reply_Details { get; set; }

    public string? Opt_Out { get; set; }

    public string? Remark { get; set; }
}
