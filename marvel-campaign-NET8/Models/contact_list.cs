using System;
using System.Collections.Generic;

namespace marvel_campaign_NET8.Models;

public partial class contact_list
{
    public int Customer_Id { get; set; }

    public DateTime? Opened_Time { get; set; }

    public int? Opened_By { get; set; }

    public DateTime? Created_Time { get; set; }

    public int? Created_By { get; set; }

    public DateTime? Updated_Time { get; set; }

    public int? Updated_By { get; set; }

    public string? ID_No { get; set; }

    public string? Name_Chi { get; set; }

    public string? Name_Eng { get; set; }

    public string? Gender { get; set; }

    public string? Title { get; set; }

    public string? Lang { get; set; }

    public DateTime? DOB { get; set; }

    public string? Home_No { get; set; }

    public string? Office_No { get; set; }

    public string? Mobile_No { get; set; }

    public string? Fax_No { get; set; }

    public string? Other_Phone_No { get; set; }

    public string? Email { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string? Address4 { get; set; }

    public string? Full_Address { get; set; }

    public string? Occupation { get; set; }

    public byte[]? Photo { get; set; }

    public string? Photo_Type { get; set; }

    public string? Wechat_Id { get; set; }

    public string? Facebook_Id { get; set; }

    public string? Whatsapp_Id { get; set; }

    public int? Nationality_Id { get; set; }

    public int? Market_Id { get; set; }

    public int? Profile_Id { get; set; }

    public string? Agree_To_Disclose_Info { get; set; }

    public string? Is_Valid { get; set; }
}
