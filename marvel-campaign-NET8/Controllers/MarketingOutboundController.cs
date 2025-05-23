using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Linq.Dynamic.Core;
using Z.EntityFramework.Plus;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace marvel_campaign_NET8.Controllers
{
    [Route("api")]
    [ApiController]
    public class MarketingOutboundController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;

        public MarketingOutboundController(ScrmDbContext context)
        {
            _scrme = context;
        }


        // Get Outbound Input Form
        [Route("GetOutboundInputForm")]
        [HttpPost]
        public IActionResult GetOutboundInputForm([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    List<outbound_input_form> _list_form = GetCRM_OutboundInputForm();

                    if ( _list_form.Any() )
                    {
                        // return successful get and display the list of data
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_form });
                    }
                    else
                    {
                        // return unsuccessful get
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "form does not exist" });
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

        private List<outbound_input_form> GetCRM_OutboundInputForm()
        {
            // obtain data
            var _frms = (from _f in _scrme.outbound_input_forms
                         where _f.Form_Status == "Active"
                         select _f);

                return _frms.ToList();

        }


        // Get Outbound Batch
        [Route("GetOutboundBatch")]
        [HttpPost]
        public IActionResult GetOutboundBatch([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    List<outbound_batch> _list_form = GetCRM_OutboundBatch();

                    if (_list_form.Any())
                    {
                        // return successful get and display the list of data
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_form });
                    }
                    else
                    {
                        // return unsuccessful get
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "batch does not exist" });
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

        private List<outbound_batch> GetCRM_OutboundBatch()
        {
            // obtain data 
            var _frms = (from _f in _scrme.outbound_batches
                         where _f.Batch_Status == "Active"
                         select _f);

            return _frms.ToList();

        }


        // Get Outbound Call Log
        [Route("GetOutboundCallLog")]
        [HttpPost]
        public IActionResult GetOutboundCallLog([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int callLeadId = Convert.ToInt32((data["Call_Lead_Id"] ?? "-1").ToString());

                    List<outbound_call_result_log> _list_call_log = GetCRM_OutboundCallLog(callLeadId);

                    if (_list_call_log.Any())
                    {
                        // return successful get and display the list of data
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_call_log });
                    }
                    else
                    {
                        // return unsuccessful get
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "call log does not exist" });
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

        private List<outbound_call_result_log> GetCRM_OutboundCallLog(int callLeadId)
        {
            var _logs = (from _log in _scrme.outbound_call_result_logs
                         where _log.Call_Lead_Id == callLeadId
                         select _log);

            return _logs.ToList();

        }


        // Get Outbound Batch Lead Count
        [Route("GetOutboundBatchLeadCount")]
        [HttpPost]
        public IActionResult GetOutboundBatchLeadCount([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {                
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int leadcount = GetCRM_BatchLead(data).Count();

                    return Ok(new
                    {
                        result = AppOutp.OutputResult_SUCC,
                        details = new
                        {
                            LeadCount = leadcount
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

        private IQueryable<outbound_call_result> GetCRM_BatchLead(JsonObject data)
        {
            int batch_id = Convert.ToInt32((data["Batch_Id"] ?? "-1").ToString());
            int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

            string call_status = (data["Call_Status"] ?? "").ToString();
            string call_reason = (data["Call_Reason"] ?? "").ToString();


            IQueryable<outbound_call_result> _pro = from _r in _scrme.outbound_call_results
                                                    where _r.Batch_Id == batch_id &&
                                                         (_r.Opt_Out == null || !_r.Opt_Out.Equals("Y"))
                                                    select _r;

            if (assign_from == 0)
            {
                _pro = _pro.Where(_c => _c.Attempt == 0 && _c.Agent_Id == null);

            }
            else
            {
                if (call_status == "NewLead")
                {
                    _pro = _pro.Where(_c => _c.Attempt == 0 && _c.Agent_Id == assign_from);
                }
                else
                {
                    _pro = _pro.Where(_c => _c.Call_Status == call_status && _c.Call_Reason == call_reason && _c.Agent_Id == assign_from);
                }
            }

            return _pro;
        }


        // Assign Outbound Batch Lead
        [Route("AssignOutboundBatchLead")]
        [HttpPost]
        public IActionResult AssignOutboundBatchLead([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int upd_result = AssignCRM_OutboundBatchLead(data);

                    if (upd_result == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "No lead / Not enough lead to assign" });

                    }
                    else
                    {
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = "Assignment done" });
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

        private int AssignCRM_OutboundBatchLead(JsonObject data)
        {
            IQueryable<outbound_call_result> _return_lead = GetCRM_BatchLead(data);

            int tot_count = _return_lead.Count();

            int assign_total = Convert.ToInt32((data["Assign_Total"] ?? "-1").ToString());


            if (tot_count > 0 && assign_total > 0 && tot_count >= assign_total)
            {
                int assign_to = Convert.ToInt32((data["Assign_To"] ?? "-1").ToString());

                if (assign_to == 0)
                {
                    _return_lead.OrderBy(x => Guid.NewGuid()).Take(assign_total)
                        .Update(x => new outbound_call_result { Agent_Id = null });
                }
                else
                {
                    _return_lead.OrderBy(x => Guid.NewGuid()).Take(assign_total)
                        .Update(x => new outbound_call_result { Agent_Id = assign_to });
                }

                return 1;
            }
            else
            {
                return -1;
            }

        }


        // Create Outbound Batch
        [Route("CreateOutboundBatch")]
        [HttpPost]
        public IActionResult CreateOutboundBatch([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int batchNo = CreateCRM_OutboundBatch(data);

                    if (batchNo != -1)
                    {
                        return Ok(new
                        {
                            result = AppOutp.OutputResult_SUCC,
                            details = new
                            {
                                Batch_Id = batchNo
                            }
                        });
                    }
                    else
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "no customer record to create batch" });
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

        private int CreateCRM_OutboundBatch(JsonObject data)
        {
            IQueryable<contact_list> _return_customer = GetCRM_CustomerForOutbound(data);

            if (data["excludeArr"] != null)
            {
                //  int[]? excludeArr_cust = data["excludeArr"]?.Deserialize<int[]>(); //
                List<int>? excludeArr_cust = data["excludeArr"]?.Deserialize<List<int>>();

                if (excludeArr_cust?.Count > 0)
                    _return_customer = _return_customer.Where(c => !excludeArr_cust.Contains(c.Customer_Id));

            }

            int tot_count = _return_customer.Count();

            if (tot_count > 0)
            {
                string batch_name = (data["Batch_Name"] ?? "").ToString();
                int form_id = Convert.ToInt32((data["Form_Id"] ?? "-1").ToString());
                int agent_id = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
                string batch_details = (data["Batch_Details"] ?? "").ToString();

                string channel_call = (data["Channel_Call"] ?? "").ToString();
                string channel_email = (data["Channel_Email"] ?? "").ToString();
                string channel_sms = (data["Channel_SMS"] ?? "").ToString();
                string channel_whatsapp = (data["Channel_Whatsapp"] ?? "").ToString();

                string gender = (data["Gender"] ?? "").ToString();

                string age_from = (data["Age_From"] ?? "").ToString();
                string age_to = (data["Age_To"] ?? "").ToString();

                string recordrange_from = (data["Recordrange_From"] ?? "").ToString();
                string recordrange_to = (data["Recordrange_To"] ?? "").ToString();


                // declare db table items
                outbound_batch _cpn_item = new outbound_batch();

                _cpn_item.Batch_Name = batch_name;
                _cpn_item.Batch_Status = "Active";
                _cpn_item.Form_Id = form_id;
                _cpn_item.Channel_Call = channel_call;
                _cpn_item.Channel_Email = channel_email;
                _cpn_item.Channel_SMS = channel_sms;
                _cpn_item.Channel_Whatsapp = channel_whatsapp;
                _cpn_item.Total_Leads = tot_count;
                _cpn_item.Created_By = agent_id;
                _cpn_item.Created_Time = DateTime.Now;
                _cpn_item.Batch_Details = batch_details;
                _cpn_item.Criteria = "Gender: " + gender + " | " + "Age_From: " + age_from + " | " + "Age_To: " + age_to + " | "
                                    + "Channel_Call: " + channel_call + " | " + "Channel_Email: " + channel_email + " | "
                                    + "Channel_SMS: " + channel_sms + " | " + "Channel_Whatsapp: " + channel_whatsapp + " | "
                                    + "Recordrange_From: " + recordrange_from + " | "
                                    + "Recordrange_To: " + recordrange_to;


                // add new record
                _scrme.outbound_batches.Add(_cpn_item);

                // save db changes
                _scrme.SaveChanges();

                int new_batch_id = _cpn_item.Batch_Id;

                _return_customer
                  .InsertFromQuery("outbound_call_result", x => new {
                      Customer_Id = x.Customer_Id,
                      Batch_Id = new_batch_id,
                      Form_Id = form_id,
                      Attempt = 0
                  });


                return new_batch_id;
            }
            else
            {
                return -1;
            }


        }

        private IQueryable<contact_list> GetCRM_CustomerForOutbound(JsonObject data)
        {
            string gender = (data["Gender"] ?? "").ToString();

            int age_from = Convert.ToInt32((data["Age_From"] ?? "-1").ToString());
            int age_to = Convert.ToInt32((data["Age_To"] ?? "-1").ToString());

            int recordrange_from = Convert.ToInt32((data["Recordrange_From"] ?? "-1").ToString());
            int recordrange_to = Convert.ToInt32((data["Recordrange_To"] ?? "-1").ToString());

            IQueryable<contact_list> _pro = from _r in _scrme.contact_lists
                                                 where _r.Is_Valid == "Y"
                                                 select _r;

            if (gender != "")
            {
                _pro = _pro.Where(_c => _c.Gender == gender);
            }

            if (age_from != -1 && age_to != -1)
            {
                DateTime today = DateTime.Today;
                DateTime min = today.AddYears(-(age_to + 1));
                DateTime max = today.AddYears(-age_from);

                _pro = _pro.Where(e => e.DOB != null && e.DOB > min && e.DOB <= max);
            }

            _pro = FilterByCommunicationChannels(_pro, data);

            if (recordrange_from != -1 && recordrange_to != -1)
            {
                _pro = _pro.OrderBy(_c => _c.Customer_Id).Skip(recordrange_from - 1).Take(recordrange_to - recordrange_from + 1);
            }

            return _pro;

        }

        private static IQueryable<contact_list> FilterByCommunicationChannels(IQueryable<contact_list> query, JsonObject data)
        {
            string channel_call = (data["Channel_Call"] ?? "").ToString();
            string channel_email = (data["Channel_Email"] ?? "").ToString();
            string channel_sms = (data["Channel_SMS"] ?? "").ToString();
            string channel_whatsapp = (data["Channel_Whatsapp"] ?? "").ToString();

            if (channel_call == "Y")
            {
                query = query.Where(_c => !(_c.Home_No == null || _c.Home_No.Equals("")) ||
                                        !(_c.Office_No == null || _c.Office_No.Equals("")) ||
                                        !(_c.Mobile_No == null || _c.Mobile_No.Equals("")) ||
                                        !(_c.Fax_No == null || _c.Fax_No.Equals("")) ||
                                        !(_c.Other_Phone_No == null || _c.Other_Phone_No.Equals(""))
                                        );

            }

            if (channel_email == "Y")
            {
                query = query.Where(_c => !(_c.Email == null || _c.Email.Equals("")));
            }

            if (channel_sms == "Y" || channel_whatsapp == "Y")
            {
                query = query.Where(_c => !(_c.Mobile_No == null || _c.Mobile_No.Equals("")));
            }

            return query;

        }


        // Search Customer For Outbound
        [Route("SearchCustomerForOutbound")]
        [HttpPost]
        public IActionResult SearchCustomerForOutbound([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Content(SearchCRM_CustomerForOutbound(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
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

        private JObject SearchCRM_CustomerForOutbound(JsonObject data)
        {
            IQueryable<contact_list> _return_customer = GetCRM_CustomerForOutbound(data);

            // declare a list of json objects containing the each row of data
            List<JObject> _con_history_list = new List<JObject>();

            // declare a json object to contain all rows of data
            // JObject allJsonResults = new JObject(); //
            
            int draw = Convert.ToInt32((data["draw"] ?? "-1").ToString());

            var start = (data["start"] ?? "").ToString();
            var length = (data["length"] ?? "").ToString();


            //  int col_index = (int)data.order[0].column.Value; //
            int col_index = data["order"]?[0]?["column"]?.GetValue<int>() ?? -1;

       //     string sortColumn = data.columns[col_index].data.Value;//
              string sortColumn = data["columns"]?[col_index]?["data"]?.GetValue<string>() ?? string.Empty;

           // var sortColumnDir = data.order[0].dir.Value; //
            var sortColumnDir = data["order"]?[0]?["dir"]?.GetValue<string>() ?? "asc";


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;


            //Sorting    
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                bool isDesc;

                if (sortColumnDir == "desc")
                    isDesc = true;
                else
                    isDesc = false;

                _return_customer = _return_customer.OrderBy(sortColumn + (isDesc ? " descending" : ""));
            }


            //total number of rows count     
            recordsTotal = _return_customer.Count();
            //Paging     
            var data2 = _return_customer.Skip(skip).Take(pageSize).ToList();

            // iterate through each row of data
            foreach (var _c_item in data2)
            {
                // declare a temp json object to store each column of data
                JObject tempJson = new JObject();

             //   tempJson.RemoveAll(); // clear the temp object

                // iterate through each column
                foreach (PropertyInfo property in _c_item.GetType().GetProperties())
                {

                    if (property.Name != "Photo")
                    {
                        tempJson.Add(new JProperty(property.Name, property.GetValue(_c_item)));
                    }
                }

                _con_history_list.Add(tempJson);
            }

            // set up _all_results json data
            JObject allJsonResults = new JObject()
            {
                new JProperty(AppOutp.OutputResult_Field, "success"),
                new JProperty("draw", draw),
                new JProperty("recordsFiltered", recordsTotal),
                new JProperty("recordsTotal", recordsTotal),
                new JProperty(AppOutp.OutputDetails_Field, _con_history_list)
            };

            // return all results in json format
            return allJsonResults;
        }


        // Update Outbound Batch
        [Route("UpdateOutboundBatch")]
        [HttpPut]
        public IActionResult UpdateOutboundBatch([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_OutboundBatch(data);
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = "record updated" });
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


        private void UpdateCRM_OutboundBatch(JsonObject data)
        {
            int pID = Convert.ToInt32((data["Batch_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

            var _pro = (from _c in _scrme.outbound_batches
                        where _c.Batch_Id == pID // && _c.Channel_SMS == "Y"
                        select _c).SingleOrDefault<outbound_batch>();

            // if there is at least 1 
            if (_pro != null)
            {
                Dictionary<string, dynamic> fieldsToBeUpdatedDict = new Dictionary<string, dynamic>();

                // iterate through the form data and assign the field name and field value to dictionary
                foreach (var item in data)
                {

                    string fieldName = item.Key;
                    var fieldValue = item.Value?.ToString() ?? null;

                    //    var fieldType = item.Value?.GetValueKind(); //old

                    if (fieldName != AppInp.InputAuth_Agent_Id && fieldName != "Token")
                    {
                        PropertyInfo? fieldProp = new outbound_batch().GetType().GetProperty(fieldName);
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

                foreach (var fields in fieldsToBeUpdatedDict)
                {
                    // find the column name that matches with the field name in dictionary
                    PropertyInfo? properInfo = _pro.GetType().GetProperty(fields.Key);
                    properInfo?.SetValue(_pro, fields.Value);
                }


                _pro.Updated_By = agentId;
                _pro.Updated_Time = DateTime.Now;

                _scrme.SaveChanges();
                
            }

        }


        // Update Outbound CallList
        [Route("UpdateOutboundCallList")]
        [HttpPut]
        public IActionResult UpdateOutboundCallList([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_OutboundCallList(data);
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = "record updated" });
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

        private void UpdateCRM_OutboundCallList(JsonObject data)
        {
            int pID = Convert.ToInt32((data["Call_Lead_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

            var _pro = (from _c in _scrme.outbound_call_results
                        where _c.Call_Lead_Id == pID
                        select _c).SingleOrDefault<outbound_call_result>();

            // if there is at least 1 
            if (_pro != null)
            {
                Dictionary<string, dynamic> fieldsToBeUpdatedDict = new Dictionary<string, dynamic>();

                // iterate through the form data and assign the field name and field value to dictionary
                foreach (var item in data)
                {

                    string fieldName = item.Key;
                    var fieldValue = item.Value?.ToString() ?? null;

                    //    var fieldType = item.Value?.GetValueKind(); //old

                    if (fieldName != AppInp.InputAuth_Agent_Id && fieldName != "Token")
                    {
                        PropertyInfo? fieldProp = new outbound_call_result().GetType().GetProperty(fieldName);
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


                foreach (var fields in fieldsToBeUpdatedDict)
                {
                    // find the column name that matches with the field name in dictionary
                    PropertyInfo? properInfo = _pro.GetType().GetProperty(fields.Key);
                    properInfo?.SetValue(_pro, fields.Value);
                }

                _pro.Transaction_Time = DateTime.Now;

                _pro.Updated_By = agentId;
                _pro.Updated_Time = DateTime.Now;
                _pro.Attempt = _pro.Attempt + 1;


                CopyTo_OutboundLog(_pro);


                _scrme.SaveChanges();

            }

        }

        void CopyTo_OutboundLog(outbound_call_result _ob_item)
        {
            // declare db table items
            outbound_call_result_log _log_item = new outbound_call_result_log();

            // iterate each column of the _contact_item
            foreach (PropertyInfo logColumn in _log_item.GetType().GetProperties())
            {
                // insert into all fields except LogID
                if (logColumn.Name != "LogID")
                {
                    // get the column name of table
                    PropertyInfo? _ob_column = _ob_item.GetType().GetProperty(logColumn.Name);

                    // insert each field value into log field
                    logColumn.SetValue(_log_item, _ob_column.GetValue(_ob_item));
                }

                // add new log record
                _scrme.outbound_call_result_logs.Add(_log_item);
            }
        }


        // Search Outbound CallList
        [Route("SearchOutboundCallList")]
        [HttpPost]
        public IActionResult SearchOutboundCallList([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int batchNo = Convert.ToInt32((data["Batch_Id"] ?? "-1").ToString());

                    if (batchNo == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        return Content(SearchCRM_OutboundCallList(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
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


        private JObject SearchCRM_OutboundCallList(JsonObject data)
        {
            // declare a list of json objects containing the each row of data
            List<JObject> _con_history_list = new List<JObject>();

            // declare a json object to contain all rows of data
            // JObject allJsonResults = new JObject(); //

            int batch_id = Convert.ToInt32((data["Batch_Id"] ?? "-1").ToString());
            int agent_id = Convert.ToInt32((data["To_Check_Id"] ?? "-1").ToString());

            string phone = (data["Phone"] ?? "").ToString();
            string name = (data["Name"] ?? "").ToString();
            string gender = (data["Gender"] ?? "").ToString();

            int age_from = Convert.ToInt32((data["Age_From"] ?? "-1").ToString());
            int age_to = Convert.ToInt32((data["Age_To"] ?? "-1").ToString());

            string call_status = (data["Call_Status"] ?? "").ToString();
            string callback_time_from = (data["Callback_Time_From"] ?? "").ToString();
            string callback_time_to = (data["Callback_Time_To"] ?? "").ToString();


            List<string> conditionList = new List<string>();
            string whereClause = string.Empty;

            string statement = string.Empty;

            statement = " c.Is_Valid == \"Y\"";

            conditionList.Add("(" + statement + ")");

            statement = " r.Opt_Out == null || !r.Opt_Out.Equals(\"Y\")";

            conditionList.Add("(" + statement + ")");


            if (batch_id != -1)
            {
                statement = " r.Batch_Id == " + batch_id;

                conditionList.Add("(" + statement + ")");

            }

            if (agent_id != -1)
            {
                statement = " r.Agent_Id == " + agent_id;

                conditionList.Add("(" + statement + ")");

            }

            if (phone != string.Empty)
            {
                statement = " c.Home_No.Contains(\"" + phone + "\")" + " || "
                          + " c.Office_No.Contains(\"" + phone + "\")" + " || "
                          + " c.Mobile_No.Contains(\"" + phone + "\")" + " || "
                          + " c.Fax_No.Contains(\"" + phone + "\")" + " || "
                          + " c.Other_Phone_No.Contains(\"" + phone + "\")";

                conditionList.Add("(" + statement + ")");
            }

            if (name != string.Empty)
            {
                statement = " c.Name_Eng.Contains(\"" + name + "\")";

                conditionList.Add("(" + statement + ")");
            }

            if (gender != string.Empty)
            {
                statement = " c.Gender.Equals(\"" + gender + "\")";

                conditionList.Add("(" + statement + ")");
            }

            if (age_from != -1 && age_to != -1)
            {
                DateTime today = DateTime.Today;
                DateTime min = today.AddYears(-(age_to + 1));
                DateTime max = today.AddYears(-age_from);

                int min_year = min.Year;
                int min_month = min.Month;
                int min_day = min.Day;

                int max_year = max.Year;
                int max_month = max.Month;
                int max_day = max.Day;

                statement = " c.DOB != null" + " && "
                          + " c.DOB > " + "DateTime(" + min_year + "," + min_month + "," + min_day + ",0,0,0)" + " && "
                          + " c.DOB <= " + "DateTime(" + max_year + "," + max_month + "," + max_day + ",0,0,0)";

                conditionList.Add("(" + statement + ")");

            }

            if (call_status != string.Empty)
            {
                if (call_status == "NewLead")
                    statement = " r.Attempt == 0";
                else
                    statement = " r.Call_Status.Equals(\"" + call_status + "\")";

                conditionList.Add("(" + statement + ")");
            }




            if (callback_time_from != string.Empty && callback_time_to != string.Empty)
            {
                double s_min = TimeSpan.ParseExact(callback_time_from, @"hh\:mm", CultureInfo.InvariantCulture).TotalMinutes;
                double e_min = TimeSpan.ParseExact(callback_time_to, @"hh\:mm", CultureInfo.InvariantCulture).TotalMinutes;

                statement = "r.Callback_Time.HasValue "
                          + " && ((r.Callback_Time.Value.Hour * 60) + r.Callback_Time.Value.Minute) >= " + s_min
                          + " && ((r.Callback_Time.Value.Hour * 60) + r.Callback_Time.Value.Minute) <= " + e_min;


                conditionList.Add("(" + statement + ")");

            }

            whereClause = string.Join(" AND ", conditionList.ToArray());


            var _return_customer = (from r in _scrme.outbound_call_results
                                    join c in _scrme.contact_lists on r.Customer_Id equals c.Customer_Id
            select new { c, r }
                   ).Where(whereClause);


            int draw = Convert.ToInt32((data["draw"] ?? "-1").ToString());

            var start = (data["start"] ?? "").ToString();
            var length = (data["length"] ?? "").ToString();


            int col_index = data["order"]?[0]?["column"]?.GetValue<int>() ?? -1;

            string sortColumn = data["columns"]?[col_index]?["data"]?.GetValue<string>() ?? string.Empty;

            var sortColumnDir = data["order"]?[0]?["dir"]?.GetValue<string>() ?? "asc";

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;


            //Sorting    
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                string final_ordering;

                bool isDesc;

                if (sortColumnDir == "desc")
                    isDesc = true;
                else
                    isDesc = false;

                string sortColumn_list;

                if (sortColumn == "Customer_Id" || sortColumn == "Name_Eng" || sortColumn == "Gender"
                     || sortColumn == "Mobile_No" || sortColumn == "Email")
                    sortColumn_list = "c.";
                else
                    sortColumn_list = "r.";


                final_ordering = sortColumn_list + sortColumn + (isDesc ? " descending" : "");


                // Sorting 2nd
                if (data["order"]?.AsArray()?.Count > 1)
                {
                    int col_index2 = data["order"]?[1]?["column"]?.GetValue<int>() ?? -1;
                    string sortColumn2 = data["columns"]?[col_index2]?["data"]?.GetValue<string>() ?? string.Empty;
                    string sortColumnDir2 = data["order"]?[1]?["dir"]?.GetValue<string>() ?? "asc";

                    string sortColumn_list2 = (sortColumn2 == "Customer_Id" || sortColumn2 == "Name_Eng" || sortColumn2 == "Gender"
                                                || sortColumn2 == "Mobile_No" || sortColumn2 == "Email") ? "c." : "r.";

                    final_ordering += $", {sortColumn_list2}{sortColumn2} {(sortColumnDir2 == "desc" ? "descending" : "")}";
                }

                _return_customer = _return_customer.OrderBy(final_ordering);
            }


            //total number of rows count     
            recordsTotal = _return_customer.Count();
            //Paging     
            var data2 = _return_customer.Skip(skip).Take(pageSize).ToList();


            // assign results' row values to the jsonResultList JSON object
            foreach (var _result_item in data2)
            {
                // declare a temp json object to store each _result_item
                JObject tempJson = new JObject();

              //  tempJson.RemoveAll(); // clear the temp object

                // iterate each column of the _result
                foreach (PropertyInfo property in _result_item.c.GetType().GetProperties())
                {
                    switch (property.Name)
                    {
                        case "Photo": // skip adding Photo content to temp
                            break;
                        default:
                            // For others, add the column name and value to temp
                            tempJson.Add(new JProperty(property.Name, property.GetValue(_result_item.c)));
                            break;
                    }
                }

                foreach (PropertyInfo property in _result_item.r.GetType().GetProperties())
                {
                    // add only the following column names and values to temp
                    switch (property.Name)
                    {
                        case "Customer_Id":
                            break;

                        case "Updated_By":
                            // rename the column name and add value to temp
                            tempJson.Add(new JProperty("Call_Updated_By", property.GetValue(_result_item.r)));
                            break;

                        case "Updated_Time":
                            // rename the column name and add value to temp
                            tempJson.Add(new JProperty("Call_Updated_Time", property.GetValue(_result_item.r)));
                            break;

                        default:
                            // add the column name and value to temp
                            tempJson.Add(new JProperty(property.Name, property.GetValue(_result_item.r)));
                            break;

                    }

                }
                _con_history_list.Add(tempJson); // add the temp result to the list
            }

            // set up _all_results json data
            JObject allJsonResults = new JObject()
            {
                new JProperty(AppOutp.OutputResult_Field, "success"),
                new JProperty("draw", draw),
                new JProperty("recordsFiltered", recordsTotal),
                new JProperty("recordsTotal", recordsTotal),
                new JProperty(AppOutp.OutputDetails_Field, _con_history_list)
            };

            // return all results in json format
            return allJsonResults;

        }


    }
}
