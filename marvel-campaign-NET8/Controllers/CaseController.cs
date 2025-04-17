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
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
         //   DateTime scheduledTime = Convert.ToDateTime((data["Scheduled_Time"] ?? DateTime.MinValue).ToString()); //old

            // declare a dictionary object where key = fieldName, value = fieldValue 
            Dictionary<string, string> fieldsToBeAdded = new Dictionary<string, string>();

            // iterate through the form data and assign the field name and field value to dictionary
            foreach (var item in data)
            {
                string fieldName = item.Key;
                string fieldValue = item.Value?.ToString() ?? string.Empty;

                if (fieldName != "Case_No" && fieldName != "Agent_Id" && fieldName != "Scheduled_Time" && fieldName != "Token")
                {
                    fieldsToBeAdded.Add(fieldName, fieldValue); // add field items to dictionary
                }
            }

            // obtain the call filter record based on the filter id
            var _reminder = (from _r in _scrme.case_reminders
                             where _r.Case_No == caseNo
                             select _r).SingleOrDefault();

            // call filter exists in table
            if (_reminder != null)
            {

                // iterate through the dictionary and update the fields
                foreach (var fields in fieldsToBeAdded)
                {
                    // find the column name that matches with the field name in dictionary
                    PropertyInfo? properInfo = _reminder.GetType().GetProperty(fields.Key);
                    if (!fieldsToBeAdded.ContainsKey("Scheduled_Time"))
                    {
                        properInfo?.SetValue(_reminder, (fields.Value == null ? string.Empty : fields.Value));
                    }
                }

                if (data.ContainsKey("Scheduled_Time") && !string.IsNullOrEmpty(data["Scheduled_Time"]?.ToString()))
                {
                    _reminder.Scheduled_Time = Convert.ToDateTime((data["Scheduled_Time"] ?? "").ToString()); // set the time
                }

                _reminder.Created_By = agentId;
                _reminder.Created_Time = DateTime.Now;


            }
            else
            {
                // add a new row using the data above
                // declare db table items
                case_reminder _new_reminder_item = new case_reminder();

                // assign new agent record
                _new_reminder_item.Case_No = caseNo;

                _new_reminder_item.Is_Read = "N";
                _new_reminder_item.Remarks = fieldsToBeAdded["Remarks"];
                _new_reminder_item.Scheduled_Time = Convert.ToDateTime((data["Scheduled_Time"] ?? "").ToString());
                _new_reminder_item.Created_By = agentId;
                _new_reminder_item.Created_Time = DateTime.Now;

                // add the new row
                _scrme.case_reminders.Add(_new_reminder_item);
            }


            _scrme.SaveChanges(); // save changes to db

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
