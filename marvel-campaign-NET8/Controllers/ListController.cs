using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });
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
                new JProperty(AppOutp.OutputResult_Field, AppOutp.OutputResult_SUCC),
                new JProperty(AppOutp.OutputDetails_Field, _call_history_list)
            };

            // return all results in json format
            return allJsonResults;
        }


        // Save CallHistory
        [Route("SaveCallHistory")]
        [HttpPost]
        public IActionResult SaveCallHistory([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Content(SaveCRM_CallHistory(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
                }
                else
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.Not_Auth_Desc });
                }

            }
            catch (Exception)
            {
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });

            }
        }

        private JObject SaveCRM_CallHistory(JsonObject data)
        {
            var callData = new CallHistoryData(data);
            var result = new JObject();
            string connectionType = string.Empty;

            var callHistory = _scrme.call_histories.SingleOrDefault(h => h.Conn_Id == callData.ConnId);

            if (callHistory == null)
            {
                _scrme.call_histories.Add(CreateNewCallHistory(callData));
                connectionType = "catching a connection";
            }
            else if (callHistory.Is_Saved != "Y")
            {
                UpdateCallHistory(callHistory, callData);

                if (callData.InternalCaseNo != -1)
                {
                    connectionType = "opening an input form or creating a new reply connection";
                }
                
                if (!string.IsNullOrEmpty(callData.IsSaved))
                {
                    connectionType = "the save button is clicked. the call may be made twice if the reply connection is created in earlier time";
                }

            }
            else
            {
                connectionType = "Is_Saved is already Y, so there is no need to update this record (by Conn_Id) in this table anymore";
            }

            _scrme.SaveChanges();

            result.Add(AppOutp.OutputResult_Field, AppOutp.OutputResult_SUCC);
            result.Add(AppOutp.OutputDetails_Field, connectionType);
            return result;
        }

        private sealed class CallHistoryData
        {
            public int ConnId { get; }
            public string CallType { get; }
            public int UpdatedBy { get; }
            public int InternalCaseNo { get; }
            public string IsSaved { get; }
            public int ConferenceConnId { get; }
            public string ReplyType { get; }
            public string ReplyDetails { get; }
            public string TypeDetails { get; }
            public int ReplyConnId { get; }
            public string IvrInfo { get; }

            public CallHistoryData(JsonObject data)
            {
                ConnId = Convert.ToInt32((data["Conn_Id"] ?? "-1").ToString());
                CallType = (data["Call_Type"] ?? "").ToString();
                UpdatedBy = Convert.ToInt32((data["Updated_By"] ?? "-1").ToString());
                InternalCaseNo = Convert.ToInt32((data["Internal_Case_No"] ?? "-1").ToString());
                IsSaved = (data["Is_Saved"] ?? "").ToString();
                ConferenceConnId = Convert.ToInt32((data["Conference_Conn_Id"] ?? "-1").ToString());
                ReplyType = (data["Reply_Type"] ?? "").ToString();
                ReplyDetails = (data["Reply_Details"] ?? "").ToString();
                TypeDetails = (data["Type_Details"] ?? "").ToString();
                ReplyConnId = Convert.ToInt32((data["Reply_Conn_Id"] ?? "-1").ToString());
                IvrInfo = (data["IVR_Info"] ?? "").ToString();
            }
        }

        private static call_history CreateNewCallHistory(CallHistoryData data)
        {
            var callHistory = new call_history
            {
                Conn_Id = data.ConnId,
                Call_Type = data.CallType,
                Updated_By = data.UpdatedBy,
                Updated_Time = DateTime.Now,
                Reply_Type = data.ReplyType,
                Reply_Details = data.ReplyDetails,
                Type_Details = data.TypeDetails,
                IVR_Info = data.IvrInfo,
                Is_Saved = string.IsNullOrEmpty(data.IsSaved) ? "N" : data.IsSaved,
                Internal_Case_No = data.InternalCaseNo != -1 ? data.InternalCaseNo : null,
                Conference_Conn_Id = data.ConferenceConnId != -1 ? data.ConferenceConnId : null,
                Reply_Conn_Id = data.ReplyConnId != -1 ? data.ReplyConnId : null
            };
            return callHistory;
        }

        private static void UpdateCallHistory(call_history callHistory, CallHistoryData data)
        {
            callHistory.Reply_Type = data.ReplyType;
            callHistory.Reply_Details = data.ReplyDetails;
            callHistory.Updated_By = data.UpdatedBy;
            callHistory.Updated_Time = DateTime.Now;
            callHistory.Is_Saved = string.IsNullOrEmpty(data.IsSaved) ? "N" : data.IsSaved;
            if (data.InternalCaseNo != -1) callHistory.Internal_Case_No = data.InternalCaseNo;
            if (data.ConferenceConnId != -1) callHistory.Conference_Conn_Id = data.ConferenceConnId;
            if (data.ReplyConnId != -1) callHistory.Reply_Conn_Id = data.ReplyConnId;
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


        // Change Contact
        [Route("ChangeContact")]
        [HttpPut]
        public IActionResult ChangeContact([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string updateStatus = ChangeCRM_Contact(data);

                    if (updateStatus == AppOutp.STATUS_SUCC)
                    {
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = "changed contact" });
                    }
                    else
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "case does not exist" });
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

        private string ChangeCRM_Contact(JsonObject data)
        {
            int caseNo = Convert.ToInt32((data["Case_No"] ?? "-1").ToString());
            int customerId = Convert.ToInt32((data[AppInp.Input_Customer_Id] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

            // obtain results from case_result
            var _case_item = (from _case in _scrme.case_results
                              where _case.Case_No == caseNo
                              select _case
                             ).SingleOrDefault();

            // if there is a result
            if (_case_item != null)
            {
                // assign new values to the table fields
                _case_item.Customer_Id = customerId;
                _case_item.Updated_By = agentId;
                _case_item.Updated_Time = DateTime.Now;

                _scrme.SaveChanges();

                return AppOutp.STATUS_SUCC;
            }
            else
            {
                // return unsuccessful update
                return "fail";
            }
        }



        // Upload Photo
        [Route("UploadPhoto")]
        [HttpPost]
        public async Task<IActionResult> UploadPhoto()
        {
            try
            {
                string token = string.Empty;
                string tk_agentId = string.Empty;

                if (Request.Form.Files.Count == 0)
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = "No file was uploaded." });
                }
                var file = Request.Form.Files[0];

                int customerId = 0;
                int agentId = 0;

                foreach (var key in Request.Form.Keys)
                {
                    if (key == AppInp.Input_Customer_Id)
                    {
                        customerId = Convert.ToInt32(Request.Form[key]);
                    }
                    else if (key == AppInp.InputAuth_Agent_Id)
                    {
                        agentId = Convert.ToInt32(Request.Form[key]);
                        tk_agentId = Convert.ToString(Request.Form[key]);
                    }
                    else if (key == "Token")
                    {
                        // token = Request.Form[key]; //old
                        token = Convert.ToString(Request.Form[key]) ?? string.Empty;

                    }
                }


                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream); // Read file asynchronously
                        byte[] photo = memoryStream.ToArray();
                        string photoType = file.ContentType;

                        // Save the photo and obtain the save status
                        string saveStatus = SaveCRM_Photo(customerId, agentId, photo, photoType);

                        if (saveStatus == AppOutp.STATUS_SUCC)
                        {
                            return Ok(new { result = AppOutp.OutputResult_SUCC, details = "" });
                        }
                        else
                        {
                            return Ok(new { result = AppOutp.OutputResult_FAIL, details = "No such record." });
                        }
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

        private string SaveCRM_Photo(int customerId, int agentId, byte[] photo, string fileType)
        {
            // obtain the row of data with the given customer id
            contact_list? _customer = (from _c in _scrme.contact_lists
                                           where _c.Customer_Id == customerId
                                           select _c).SingleOrDefault();

            if (_customer == null)
            {
                return "fail";
            }
            else // contact exists
            {
                _customer.Photo = photo; // assign the file to Photo column in table
                _customer.Photo_Type = fileType; // assign the file type to Photo_Type column
                _customer.Updated_By = agentId;
                _customer.Updated_Time = DateTime.Now;

                CopyTo_ContactLog(_customer);

                _scrme.SaveChanges(); // save to database

                return AppOutp.STATUS_SUCC;
            }
        }

        void CopyTo_ContactLog(contact_list _contact_item)
        {
            // declare db table items
            contact_list_log _log_item = new contact_list_log();

            // iterate each column of the _contact_item
            foreach (PropertyInfo logColumn in _log_item.GetType().GetProperties())
            {
                // insert into all fields except LogID
                if (logColumn.Name != "LogID")
                {
                    // get the column name of case_result table
                    PropertyInfo? _contact_column = _contact_item.GetType().GetProperty(logColumn.Name);

                    // insert each case field value into log field
                    logColumn.SetValue(_log_item, _contact_column.GetValue(_contact_item));
                }

                // add new case result log record
                _scrme.contact_list_logs.Add(_log_item);
            }
        }


        // Add Customer Case
        [Route("AddCustomerCase")]
        [HttpPost]
        public IActionResult AddCustomerCase([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    List<case_result> _new_case = AddCRM_CustCase(data);

                    return Ok(new
                    {
                        result = AppOutp.OutputResult_SUCC,
                        details = new
                        {
                            // retrieve the customer_id and internal_case_no from the new case
                            Customer_Id = _new_case[0].Customer_Id,
                            Internal_Case_No = _new_case[0].Internal_Case_No
                        }
                    });

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

        private List<case_result> AddCRM_CustCase(JsonObject data)
        {

            // declare db table items
            contact_list _contact_item = new contact_list();
            case_result _case_item = new case_result();

            int connId = Convert.ToInt32((data["Conn_Id"] ?? "-1").ToString());
            int customerId = Convert.ToInt32((data[AppInp.Input_Customer_Id] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
            string callType = (data["Call_Type"] ?? "").ToString();
            string typeDetails = (data["Type_Details"] ?? "").ToString();

            // for new customer
            if (customerId == -1)
            {
                // assign new customer record
                _contact_item.Opened_Time = DateTime.Now;
                _contact_item.Opened_By = agentId;
                _contact_item.Created_Time = DateTime.Now;
                _contact_item.Created_By = agentId;
                _contact_item.Updated_Time = DateTime.Now;
                _contact_item.Updated_By = agentId;
                _contact_item.Is_Valid = "N";


                // add new customer record
                _scrme.contact_lists.Add(_contact_item);

                // save db changes
                _scrme.SaveChanges();

                customerId = _contact_item.Customer_Id;

            }

            // assign new case record
            _case_item.Customer_Id = customerId;
            _case_item.Conn_Id = connId;
            _case_item.Opened_By = agentId;
            _case_item.Opened_Time = DateTime.Now;
            _case_item.Created_By = agentId;
            _case_item.Created_Time = DateTime.Now;
            _case_item.Updated_By = agentId;
            _case_item.Updated_Time = DateTime.Now;
            _case_item.Call_Type = callType;
            _case_item.Is_Valid = "N";
            _case_item.Type_Details = typeDetails;

            // add new case record
            _scrme.case_results.Add(_case_item);

            // save db changes
            _scrme.SaveChanges();

            // obtain the new case from table "case_list"
            List<case_result> _linq_case = (from _c in _scrme.case_results
                                                      where _c.Internal_Case_No == _case_item.Internal_Case_No
                                                      select _c).ToList();

            return _linq_case;
        }


        // Update Customer
        [Route("UpdateCustomer")]
        [HttpPut]
        public IActionResult UpdateCustomer([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_Customer(data);
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = "updated contact list." });
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

        private void UpdateCRM_Customer(JsonObject data)
        {
            int customerId = Convert.ToInt32((data[AppInp.Input_Customer_Id] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

            var customerData = data["Customer_Data"] as JsonObject;

            if (customerData == null)
            {
                return; // Early exit if customer data is invalid
            }

            var customer = _scrme.contact_lists
                .SingleOrDefault(c => c.Customer_Id == customerId);

            if (customer == null)
            {
                return; // Early exit if customer not found
            }

            UpdateCustomerFields(customer, customerData);
            UpdateAuditFields(customer, agentId);

            if (customer.Is_Valid == "Y")
            {
                CopyTo_ContactLog(customer);
            }

            _scrme.SaveChanges();
        }

        private static void UpdateCustomerFields(contact_list customer, JsonObject customerData)
        {
            var fieldsToUpdate = BuildFieldsToUpdate(customerData);

            foreach (var field in fieldsToUpdate)
            {
                PropertyInfo? propertyInfo = customer.GetType().GetProperty(field.Key);
                propertyInfo?.SetValue(customer, field.Value);
            }
        }

        private static Dictionary<string, dynamic> BuildFieldsToUpdate(JsonObject customerData)
        {
            var fieldsToUpdate = new Dictionary<string, dynamic>();

            foreach (var item in customerData)
            {
                if (item.Key is AppInp.InputAuth_Agent_Id or "Token")
                {
                    continue;
                }

                var fieldValue = item.Value?.ToString();
                PropertyInfo? propertyInfo = new contact_list().GetType().GetProperty(item.Key);

                if (propertyInfo == null)
                {
                    continue;
                }

                Type targetType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                if (IsNumericOrSpecialType(targetType))
                {
                    fieldsToUpdate[item.Key] = fieldValue != null ? Convert.ChangeType(fieldValue, targetType) : null;
                }
                else
                {
                    fieldsToUpdate[item.Key] = fieldValue ?? string.Empty;
                }
            }

            return fieldsToUpdate;
        }

        private static bool IsNumericOrSpecialType(Type type)
        {
            return type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                   type == typeof(DateTime) || type == typeof(bool);
        }

        private static void UpdateAuditFields(contact_list customer, int agentId)
        {
            if (customer.Is_Valid == "N")
            {
                customer.Created_By = agentId;
                customer.Created_Time = DateTime.Now;
                customer.Is_Valid = "Y";
            }

            customer.Updated_By = agentId;
            customer.Updated_Time = DateTime.Now;
        }


        // Update Case
        [Route("UpdateCase")]
        [HttpPut]
        public IActionResult UpdateCase([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();
            
            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return UpdateCaseResult(data);
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


        private IActionResult UpdateCaseResult(JsonObject data)
        {
            int internalCaseNo = Convert.ToInt32((data["Internal_Case_No"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
            // string replyType = (data["Reply_Type"] ?? "").ToString(); //old
            // string replyDetails = (data["Reply_Details"] ?? "").ToString(); /old

            new_case_no _newCaseNo = new new_case_no();
            var _case = (from _c in _scrme.case_results
                         where _c.Internal_Case_No == internalCaseNo
                         select _c).SingleOrDefault();
            if (_case == null)
            {
                return Ok(new
                {
                    result = AppOutp.OutputResult_FAIL,
                    details = "case does not exist"
                });

            }
            if (_case.Is_Valid == "N")
            {
                // add new case no
                _newCaseNo.Time_Stamp = DateTime.Now;
                _scrme.new_case_nos.Add(_newCaseNo);

                _case.Created_By = agentId;
                _case.Created_Time = DateTime.Now;
                _case.Is_Valid = "Y";

                _case.Attempt = 0;

                _scrme.SaveChanges();
                _case.Case_No = _newCaseNo.Case_No; // assign the newly added Case_No from new_case_no table
            }
            _case.Updated_By = agentId;
            _case.Updated_Time = DateTime.Now;

            _case.Attempt = (_case.Attempt ?? 0) + 1;

         //   Dictionary<string, dynamic> fieldsToBeUpdatedDict = new Dictionary<string, dynamic>(); //old
            foreach (var item in data)
            {
                SetCaseProperty(_case, item);
            }

            if (_case.Case_Flag != "temp")
            {
                CopyTo_CaseLog(_case); // copy case_result data to case_result_log

                // for non-empty reply details, add them to a new table
                if (_case.Reply_Type != string.Empty && _case.Reply_Details != string.Empty)
                {
                    // split the reply details by ,
                    string[]? replyDetailArray = _case.Reply_Details?.Split(','); // e.g. 92292207,91312201 to [92292207] and [91312201]

                    // iterate through each reply detail and add it to the new table
                    for (int i = 0; i < replyDetailArray?.Length; i++)
                    {
                        AddTo_ReplyDetails(_case, replyDetailArray[i]);
                    }
                }
            }


            // update status and save changes in db
            _scrme.Entry(_case).State = EntityState.Modified;
            _scrme.SaveChanges();
            return Ok(new
            {
                result = AppOutp.OutputResult_SUCC,
                details = new { Case_No = _case.Case_No }
            });
        }

        private static void SetCaseProperty(case_result _case, KeyValuePair<string, JsonNode?> item)
        {
            string fieldName = item.Key;
            var fieldValue = item.Value?.ToString() ?? null;

            if (fieldName != AppInp.InputAuth_Agent_Id && fieldName != "Token")
            {
                PropertyInfo? fieldProp = new case_result().GetType().GetProperty(fieldName);
                Type type = Nullable.GetUnderlyingType(fieldProp.PropertyType) ?? fieldProp.PropertyType;
                string ftype = type.Name;

                PropertyInfo? properInfo = _case.GetType().GetProperty(fieldName);


                if (ftype == "Int16" || ftype == "Int32" || ftype == "Int64" || ftype == "DateTime" || ftype == "Boolean")
                {
                    if (fieldValue != null)
                    {
                        properInfo?.SetValue(_case, Convert.ChangeType(fieldValue, type));
                        //fieldsToBeUpdatedDict.Add(fieldName, Convert.ChangeType(fieldValue, type)); // add field items to dictionary
                    }
                    else
                    {
                        properInfo?.SetValue(_case, null);
                        //fieldsToBeUpdatedDict.Add(fieldName, null); // add field items to dictionary
                    }
                }
                else
                {
                    if (fieldValue != null)
                    {
                        properInfo?.SetValue(_case, Convert.ToString(fieldValue));
                        //fieldsToBeUpdatedDict.Add(fieldName, Convert.ToString(fieldValue)); // add field items to dictionary
                    }
                    else
                    {
                        properInfo?.SetValue(_case, string.Empty);
                        //fieldsToBeUpdatedDict.Add(fieldName, string.Empty); // add field items to dictionary
                    }
                }
            }
        }

        void CopyTo_CaseLog(case_result _case_item)
        {
            // declare db table items
            case_result_log _log_item = new case_result_log();

            // iterate each column of the _contact_item
            foreach (PropertyInfo logColumn in _log_item.GetType().GetProperties())
            {
                // insert into all fields except LogID
                if (logColumn.Name != "LogID")
                {
                    // get the column name of case_result table
                    PropertyInfo _case_column = _case_item.GetType().GetProperty(logColumn.Name);

                    // insert each case field value into log field
                    logColumn.SetValue(_log_item, _case_column.GetValue(_case_item));
                }
                // add new case result log record
                _scrme.case_result_logs.Add(_log_item);
            }
        }

        void AddTo_ReplyDetails(case_result _case_item, string replyDetails)
        {
            // declare db table items
            reply_details_history _reply_item = new reply_details_history();

            // make sure reply type and reply details are not empty
            if (_case_item.Reply_Type != string.Empty && replyDetails != string.Empty)
            {
                // assign new reply details record
                _reply_item.Customer_Id = _case_item.Customer_Id;
                _reply_item.Case_No = _case_item.Case_No;
                _reply_item.Reply_Type = _case_item.Reply_Type;
                _reply_item.Reply_Details = replyDetails;
                _reply_item.Updated_By = _case_item.Updated_By;
                _reply_item.Updated_Time = _case_item.Updated_Time;

                // add new reply record
                _scrme.reply_details_histories.Add(_reply_item);
            }
        }



    }
}
