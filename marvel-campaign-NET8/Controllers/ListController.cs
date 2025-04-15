using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace marvel_campaign_NET8.Controllers
{
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
