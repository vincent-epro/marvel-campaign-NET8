using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class call_history
{
    public int Conn_Id { get; set; }

    public int? Internal_Case_No { get; set; }

    public string? Call_Type { get; set; }

    public string? Type_Details { get; set; }

    public string? Is_Saved { get; set; }

    public string? Reply_Type { get; set; }

    public int? Reply_Conn_Id { get; set; }

    public string? Reply_Details { get; set; }

    public string? IVR_Info { get; set; }

    public int? Conference_Conn_Id { get; set; }

    public int? Updated_By { get; set; }

    public DateTime? Updated_Time { get; set; }
}
