﻿using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json.Nodes;

namespace marvel_campaign_NET8.Controllers
{

    public class FieldDetails
    {
        // Public properties for external access
        public string Field_Name { get; private set; }
        public string Field_Display { get; private set; }
        public string Field_Tag { get; private set; }
        public string Field_Type { get; private set; }
        public List<string> Field_Options { get; private set; }

        public FieldDetails(string name, string display, string tag, string type, List<string> options)
        {
            Field_Name = name;
            Field_Display = display;
            Field_Tag = tag;
            Field_Type = type;
            Field_Options = options;
        }
    }

    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class OtherController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;

        public OtherController(ScrmDbContext context)
        {
            _scrme = context;
        }


        // Get Fields
        [Route("GetFields")]
        [HttpPost]
        public IActionResult GetFields([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {

                    // declare a dictionary object where key = Field_Category, value = FieldDetails's values
                    //Dictionary<string, List<FieldDetails>> tableFields = new Dictionary<string, List<FieldDetails>>(); //old

                    // obtain the fields of the given list
                    Dictionary<string, List<FieldDetails>> tableFields = GetCRM_Fields(data);


                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = tableFields });
                }
                else
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.Not_Auth_Desc });
                }

            }
            catch (Exception)
            {
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = "invalid parameters" });
            }
        }

        private Dictionary<string, List<FieldDetails>> GetCRM_Fields(JsonObject data)
        {
            List<string>? listArray = JsonConvert.DeserializeObject<List<string>>(data!["listArr"]!.ToJsonString()) ?? new List<string>();

            // obtain linq result lists of table Field
            List<field> _linq_field = (from _f in _scrme.fields
                                       select _f).ToList();

            // obtain linq result lists of table Field_Option
            List<field_option> _linq_field_option = (from _f in _scrme.field_options
                                                     select _f).ToList();

            // declare a dictionary object where key = Field_Category, value = FieldDetails's values
            Dictionary<string, List<FieldDetails>> tableFieldsDict = new Dictionary<string, List<FieldDetails>>();

            // iterate through the 3 field categories
            foreach (string _field_category in listArray)
            {
                // declare new FieldDetails class object as List
                List<FieldDetails> _list_field_details = new List<FieldDetails>();

                // retrieve rows from a particular field category
                IOrderedEnumerable<field> _field_record = _linq_field.Where(_f => _f.Field_Category == _field_category).OrderBy(_f => _f.Field_Display);


                // iterate through the rows from that field category
                foreach (field _field_item in _field_record)
                {
                    // retrieve rows from a particular field name
                    IEnumerable<field_option> _option_record = _linq_field_option.Where(_o => (_o.Field_Name == _field_item.Field_Name));

                    // create a temporary list for Field_Options
                    List<string> temp = new List<string>();

                    // obtain field options if there are any
                    if (_field_item.Field_Tag == "select")
                    {
                        // iterate through each option in the table Field_Option
                        foreach (field_option _option in _option_record)
                        {
                            // add the option value to temp
                            temp.Add(_option.Field_Option);
                        }
                    }
                    else
                    {
                        // for tags other than "select", add "N/A" to the list
                        temp.Add("N/A");
                    }

                    // declare and initialize FieldDetails class object with required arguments
                    FieldDetails _fd = new FieldDetails(
                        _field_item.Field_Name,
                        _field_item.Field_Display ?? string.Empty,
                        _field_item.Field_Tag ?? string.Empty,
                        _field_item.Field_Type ?? string.Empty,
                        temp
                    );

                    // append the object to the list
                    _list_field_details.Add(_fd);
                }



                // add the category and its details to the dictionary
                tableFieldsDict.Add(_field_category, _list_field_details);
            }

            return tableFieldsDict;
        }


        // Get Photo
        [Route("GetPhoto")]
        [HttpPost]
        public IActionResult GetPhoto([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Content(Get_CRMPhoto(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
                }
                else
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.Not_Auth_Desc });
                }
            }
            catch (Exception err)
            {
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = err.Message });
            }
        }

        private JObject Get_CRMPhoto(JsonObject data)
        {
           // JObject photoJson = new JObject(); // declare json object

            int customerId = Convert.ToInt32((data["Customer_Id"] ?? "-1").ToString());

            if (customerId == 0)
            {
                   JObject photoJson = new JObject() // no customer
                    {
                        new JProperty("result", AppOutp.OutputResult_FAIL),
                        new JProperty("details", "invalid parameters")
                    };

                return photoJson;
            }
            else
            {
                // declare table item and obtain the row of data with the given customer id
                contact_list? _contact = (from _c in _scrme.contact_lists
                                              where _c.Customer_Id == customerId
                                              select _c).SingleOrDefault();

                if (_contact != null)
                {
                    JObject jsonResults = new JObject()
                    {   // add column data to json object
                        new JProperty("Photo_Type", _contact.Photo_Type),
                        new JProperty("Photo_Content", Convert.ToBase64String(_contact.Photo))
                    };

                    JObject photoJson = new JObject() // add to overall json object
                    {
                        new JProperty("result", AppOutp.OutputResult_SUCC),
                        new JProperty("details", jsonResults)
                    };

                    return photoJson;
                }
                else
                {
                    JObject photoJson = new JObject() // add to overall json object
                    {
                        new JProperty("result", AppOutp.OutputResult_FAIL),
                        new JProperty("details", "customer does not exist")
                    };

                    return photoJson;
                }
            }
            
        }






    }
}
