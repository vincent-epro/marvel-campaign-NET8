using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class case_result
{
    public int Internal_Case_No { get; set; }

    public int? Case_No { get; set; }

    public int? Customer_Id { get; set; }

    public int? Attempt { get; set; }

    public int? Conn_Id { get; set; }

    public int? Opened_By { get; set; }

    public DateTime? Opened_Time { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }

    public string? Call_Type { get; set; }

    public string? Type_Details { get; set; }

    public DateTime? Inbound_Time { get; set; }

    public string? Call_Link_File { get; set; }

    public string? Call_Nature { get; set; }

    public string? Details { get; set; }

    public string? Status { get; set; }

    public string? Remark { get; set; }

    public int? Escalated_To { get; set; }

    public string? Reply_Type { get; set; }

    public string? Reply_Details { get; set; }

    public DateTime? Reply_Time { get; set; }

    public string? Reply_Conn_Id { get; set; }

    public string? Reply_Call_Result { get; set; }

    public int? Conference_Conn_Id { get; set; }

    public string? Case_Flag { get; set; }

    public string? IVR_Info { get; set; }

    public string? Long_Call { get; set; }

    public string? Long_Call_Reason { get; set; }

    public string? Ticket_Id { get; set; }

    public string? Is_Junk_Mail { get; set; }

    public string? Is_Valid { get; set; }
}
