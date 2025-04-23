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
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });

            }
        }

        private JObject SaveCRM_CallHistory(JsonObject data)
        {
            // obtain form body values
            int connId = Convert.ToInt32((data["Conn_Id"] ?? "-1").ToString());
            string callType = (data["Call_Type"] ?? "").ToString();
            int updatedBy = Convert.ToInt32((data["Updated_By"] ?? "-1").ToString());
            int internalCaseNo = Convert.ToInt32((data["Internal_Case_No"] ?? "-1").ToString());
            string isSaved = (data["Is_Saved"] ?? "").ToString();
            int conferenceConnId = Convert.ToInt32((data["Conference_Conn_Id"] ?? "-1").ToString());
            string replyType = (data["Reply_Type"] ?? "").ToString();
            string replyDetails = (data["Reply_Details"] ?? "").ToString();
            string typeDetails = (data["Type_Details"] ?? "").ToString();
            int replyConnId = Convert.ToInt32((data["Reply_Conn_Id"] ?? "-1").ToString());
            string ivrInfo = (data["IVR_Info"] ?? "").ToString();

            //JObject allJsonResults = new JObject(); // declare json object

            string connectionType = string.Empty; // declare connection type

            // obtain the row of data with the given conn_id
            call_history? _call_history_item = (from _h in _scrme.call_histories
                                                         where _h.Conn_Id == connId
                                                         select _h).SingleOrDefault();

            if (_call_history_item == null)
            {
                call_history _inserted_call_history = new call_history();
                // insert new record with the connId
                _inserted_call_history.Conn_Id = connId;
                _inserted_call_history.Call_Type = callType;
                _inserted_call_history.Updated_By = updatedBy;
                _inserted_call_history.Updated_Time = DateTime.Now;
                _inserted_call_history.Reply_Type = replyType;
                _inserted_call_history.Reply_Details = replyDetails;
                _inserted_call_history.Type_Details = typeDetails;
                _inserted_call_history.IVR_Info = ivrInfo;

                if (internalCaseNo != -1)
                {
                    _inserted_call_history.Internal_Case_No = internalCaseNo;
                }

                if (isSaved != string.Empty)
                {
                    _inserted_call_history.Is_Saved = isSaved;
                }
                else
                {
                    _inserted_call_history.Is_Saved = "N";
                }

                if (conferenceConnId != -1)
                {
                    _inserted_call_history.Conference_Conn_Id = conferenceConnId;
                }

                if (replyConnId != -1)
                {
                    _inserted_call_history.Reply_Conn_Id = replyConnId;
                }

                // add new call history record
                _scrme.call_histories.Add(_inserted_call_history);

                // save db changes
                _scrme.SaveChanges();

                connectionType = "catching a connection";
            }
            else
            {
                // update record when Is_Saved != "Y" in the table
                if (_call_history_item.Is_Saved != "Y")
                {
                    if (internalCaseNo != -1)
                    {
                        _call_history_item.Internal_Case_No = internalCaseNo;
                        connectionType = "opening an input form or creating a new reply connection";
                    }

                    if (isSaved != string.Empty)
                    {
                        _call_history_item.Is_Saved = isSaved;
                        connectionType = "the save button is clicked. the call may be made twice if the reply connection is created in earlier time";
                    }
                    else
                    {
                        _call_history_item.Is_Saved = "N";
                    }

                    if (conferenceConnId != -1)
                    {
                        _call_history_item.Conference_Conn_Id = conferenceConnId;
                    }

                    if (replyConnId != -1)
                    {
                        _call_history_item.Reply_Conn_Id = replyConnId;
                    }


                    _call_history_item.Reply_Type = replyType;
                    _call_history_item.Reply_Details = replyDetails;

                    _call_history_item.Updated_By = updatedBy;
                    _call_history_item.Updated_Time = DateTime.Now;


                    _scrme.SaveChanges();
                }
                else
                {
                    connectionType = "Is_Saved is already Y, so there is no need to update this record (by Conn_Id) in this table anymore";
                }
            }

            JObject allJsonResults = new JObject()
            {
                new JProperty("result", AppOutp.OutputResult_SUCC),
                new JProperty("details", connectionType)
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

                    if (updateStatus == "success")
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
            int customerId = Convert.ToInt32((data["Customer_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());

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

                return "success";
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
                    if (key == "Customer_Id")
                    {
                        customerId = Convert.ToInt32(Request.Form[key]);
                    }
                    else if (key == "Agent_Id")
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

                        if (saveStatus == "success")
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

                return "success";
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
                            Customer_Id = _new_case.First().Customer_Id,
                            Internal_Case_No = _new_case.First().Internal_Case_No
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
            int customerId = Convert.ToInt32((data["Customer_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
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
            int customerId = Convert.ToInt32((data["Customer_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());

            // assign form body values to table item
            var customerData = data["Customer_Data"];

            // obtain single customer record based on the customer id
            var _customer = (from _c in _scrme.contact_lists
                             where _c.Customer_Id == customerId
                             select _c).SingleOrDefault<contact_list>();

            // if there is at least 1 customer
            if (_customer != null)
            {

                Dictionary<string, dynamic> fieldsToBeUpdatedDict = new Dictionary<string, dynamic>();

                var customerDataList = customerData as JsonObject;

                if (customerDataList != null)
                {

                    // iterate through the form data and assign the field name and field value to dictionary
                    foreach (var item in customerDataList)
                    {

                        string fieldName = item.Key;
                        var fieldValue = item.Value?.ToString() ?? null;

                    //    var fieldType = item.Value?.GetValueKind(); //old

                        if (fieldName != "Agent_Id" && fieldName != "Token")
                        {
                            PropertyInfo? fieldProp = new contact_list().GetType().GetProperty(fieldName);
                            Type type = Nullable.GetUnderlyingType(fieldProp.PropertyType) ?? fieldProp.PropertyType;
                            string ftype = type.Name;

                            if (ftype == "Int16" || ftype == "Int32" || ftype == "Int64" || ftype == "DateTime" || ftype == "Boolean")
                            {
                                if (fieldValue != null)
                                {
                                    fieldsToBeUpdatedDict.Add(fieldName, Convert.ChangeType(fieldValue, type)); // add field items to dictionary
                                }
                                else
                                {
                                    fieldsToBeUpdatedDict.Add(fieldName, null); // add field items to dictionary
                                }
                            }
                            else
                            {
                                if (fieldValue != null)
                                {
                                    fieldsToBeUpdatedDict.Add(fieldName, Convert.ToString(fieldValue)); // add field items to dictionary
                                }
                                else
                                {
                                    fieldsToBeUpdatedDict.Add(fieldName, string.Empty); // add field items to dictionary
                                }
                            }
                        }
                    }

                }


                foreach (var fields in fieldsToBeUpdatedDict)
                {
                    // find the column name that matches with the field name in dictionary
                    PropertyInfo? properInfo = _customer.GetType().GetProperty(fields.Key);
                    properInfo?.SetValue(_customer, fields.Value);
                }




                // updated other columns
                if (_customer.Is_Valid == "N") // insert customer info
                {
                    _customer.Created_By = agentId;
                    _customer.Created_Time = DateTime.Now;
                    _customer.Is_Valid = "Y";
                }
                _customer.Updated_By = agentId;
                _customer.Updated_Time = DateTime.Now;

                if (_customer.Is_Valid == "Y")
                {
                    CopyTo_ContactLog(_customer); // copy contact_list data to contact_list_log
                }

                _scrme.SaveChanges();

            }

        }





    }
}
