using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json.Nodes;

namespace marvel_campaign_NET8.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;

        public CaseController(ScrmDbContext context)
        {
            _scrme = context;
        }


        // Update Case Reminder
        [Route("UpdateCaseReminder")]
        [HttpPut]
        public IActionResult UpdateCaseReminder([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_CaseReminder(data);
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = "updated case reminder" });
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

        private void UpdateCRM_CaseReminder(JsonObject data)
        {
            int caseNo = Convert.ToInt32((data["Case_No"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

            Dictionary<string, string> fieldsToBeAdded = ExtractFields(data);

            var _reminder = _scrme.case_reminders.SingleOrDefault(_r => _r.Case_No == caseNo);

            if (_reminder != null)
            {
                UpdateExistingReminder(_reminder, fieldsToBeAdded, data);
            }
            else
            {
                AddNewReminder(caseNo, agentId, fieldsToBeAdded, data);
            }

            _scrme.SaveChanges();
        }

        private static Dictionary<string, string> ExtractFields(JsonObject data)
        {
            return data
                .Where(item => item.Key != "Case_No" && item.Key != AppInp.InputAuth_Agent_Id && item.Key != AppInp.Input_Scheduled_Time && item.Key != "Token")
                .ToDictionary(item => item.Key, item => item.Value?.ToString() ?? string.Empty);
        }

        private static void UpdateExistingReminder(case_reminder _reminder, Dictionary<string, string> fieldsToBeAdded, JsonObject data)
        {
            foreach (var fields in fieldsToBeAdded)
            {
                PropertyInfo? properInfo = _reminder.GetType().GetProperty(fields.Key);
                if (!fieldsToBeAdded.ContainsKey(AppInp.Input_Scheduled_Time))
                {
                    properInfo?.SetValue(_reminder, fields.Value ?? string.Empty);
                }
            }

            if (data.ContainsKey(AppInp.Input_Scheduled_Time) && !string.IsNullOrEmpty(data[AppInp.Input_Scheduled_Time]?.ToString()))
            {
                _reminder.Scheduled_Time = Convert.ToDateTime((data[AppInp.Input_Scheduled_Time] ?? "").ToString());
            }

            _reminder.Created_By = Convert.ToInt32(data[AppInp.InputAuth_Agent_Id]?.ToString() ?? "-1");
            _reminder.Created_Time = DateTime.Now;
        }

        private void AddNewReminder(int caseNo, int agentId, Dictionary<string, string> fieldsToBeAdded, JsonObject data)
        {
            case_reminder _new_reminder_item = new case_reminder
            {
                Case_No = caseNo,
                Is_Read = "N",
                Remarks = fieldsToBeAdded["Remarks"],
                Scheduled_Time = Convert.ToDateTime((data[AppInp.Input_Scheduled_Time] ?? "").ToString()),
                Created_By = agentId,
                Created_Time = DateTime.Now
            };

            _scrme.case_reminders.Add(_new_reminder_item);
        }



        // Get Case Reminder
        [Route("GetCaseReminder")]
        [HttpPost]
        public IActionResult GetCaseReminder([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = GetCRM_CaseReminder(data) });
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


        private List<case_reminder> GetCRM_CaseReminder(JsonObject data)
        {
            int caseNo = Convert.ToInt32((data["Case_No"] ?? "-1").ToString());

            string isRead = (data["Is_Read"] ?? "").ToString();
            string isOverdue = (data["Is_Overdue"] ?? "").ToString();

            int agentId = Convert.ToInt32((data["To_Check_Id"] ?? "-1").ToString());


            // obtain results from case enquiry nature
            var _reminder = from _r in _scrme.case_reminders
                            select _r;

            // return results in list or null
            if (_reminder.Count() > 0)
            {
                // search by case no
                if (caseNo != -1)
                {
                    _reminder = _reminder.Where(_r => _r.Case_No == caseNo);
                }

                // search by isRead
                if (isRead != string.Empty)
                {
                    _reminder = _reminder.Where(_r => _r.Is_Read == isRead);
                }

                // search by overdue
                if (isOverdue == "Y")
                {
                    _reminder = _reminder.Where(_r => _r.Scheduled_Time < DateTime.Now);
                }
                else if (isOverdue == "N")
                {
                    _reminder = _reminder.Where(_r => _r.Scheduled_Time >= DateTime.Now);
                }

                // search by agent id
                if (agentId != -1)
                {
                    _reminder = _reminder.Where(_r => _r.Created_By == agentId);
                }
                              
            }

            return _reminder.ToList();

        }




    }
}
