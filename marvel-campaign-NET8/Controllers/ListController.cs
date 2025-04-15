using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;

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
    public class ListController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;

        public ListController(ScrmDbContext context)
        {
            _scrme = context;
        }


        // Get Call History
        [Route("GetCallHistory")]
        [HttpPost]
        public IActionResult GetCallHistory([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int agentId = Convert.ToInt32((data["Updated_By"] ?? "-1").ToString());

                    return Content(Get_CRMCallHistory(agentId).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
                }
                else
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.Not_Auth_Desc });
                }
            }
            catch (Exception)
            {
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
            }
        }


        private JObject Get_CRMCallHistory(int agentId)
        {
            // obtain linq result lists of table call_history ordered by Updated_Time
            var _all_call_histories = (from _h in _scrme.call_histories
                                       where _h.Is_Saved != "Y"
                                       select _h);

            if (agentId != -1)
            {
                _all_call_histories = _all_call_histories.Where(_h => _h.Updated_By == agentId).OrderBy(_h => _h.Updated_Time);
            }
            else
            {
                _all_call_histories = _all_call_histories.OrderBy(_h => _h.Updated_Time);
            }

            // declare a list of json objects containing the each row of data
            List<JObject> _call_history_list = new List<JObject>();

            // declare a json object to contain all rows of data
           // JObject allJsonResults = new JObject(); //old

            // iterate through each row of data in call_historu
            foreach (var _history_item in _all_call_histories)
            {
                // declare a temp json object to store each column of data
                JObject tempJson = new JObject();

                // tempJson.RemoveAll(); // clear the temp object

                // iterate through each column of the _agent_item
                foreach (PropertyInfo property in _history_item.GetType().GetProperties())
                {
                    // add all column names and values to temp, except "Is_Saved"
                    if (property.Name != "Is_Saved")
                    {
                        tempJson.Add(new JProperty(property.Name, property.GetValue(_history_item)));
                    }
                }

                _call_history_list.Add(tempJson);
            }

            // set up _all_results json data
            JObject allJsonResults = new JObject()
            {
                new JProperty("result", AppOutp.OutputResult_SUCC),
                new JProperty("details", _call_history_list)
            };

            // return all results in json format
            return allJsonResults;
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


        // Get Case Log
        [Route("GetCaseLog")]
        [HttpPost]
        public IActionResult GetCaseLog([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int caseNo = Convert.ToInt32((data["Case_No"] ?? "-1").ToString());
                    string validityType = (data["Is_Valid"] ?? "").ToString();

                    List<case_result_log> _list_case_log = GetCRM_CaseLog(caseNo, validityType);

                    if (_list_case_log != null)
                    {
                        // return successful get and display the list of data
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_case_log });
                    }
                    else
                    {
                        // return unsuccessful get
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "case log does not exist" });
                    }

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

        private List<case_result_log> GetCRM_CaseLog(int caseNo, string validityType)
        {
            // obtain data from table "case_result_log"
            var _logs = (from _log in _scrme.case_result_logs
                         where _log.Case_No == caseNo
                         select _log);

            // filter out results by validity and [order by Created_Time in descending order]
            switch (validityType)
            {
                case "Y":
                    _logs = _logs.Where(_l => _l.Is_Valid == "Y").OrderByDescending(_l => _l.Updated_Time);
                    break;

                case "N":
                    _logs = _logs.Where(_l => _l.Is_Valid == "N").OrderByDescending(_l => _l.Updated_Time);
                    break;

                case "all":
                default:
                    {
                        _logs = _logs.Where(_l => _l.Is_Valid == "Y" || _l.Is_Valid == "N").OrderByDescending(_l => _l.Updated_Time);
                        break;
                    }
            }


            return _logs.ToList();

        }



    }
}
