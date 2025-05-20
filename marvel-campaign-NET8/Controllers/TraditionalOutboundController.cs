using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Linq.Dynamic.Core;
using Z.EntityFramework.Plus;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace marvel_campaign_NET8.Controllers
{
    [Route("api")]
    [ApiController]
    public class TraditionalOutboundController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;

        public TraditionalOutboundController(ScrmDbContext context)
        {
            _scrme = context;
        }


        // Get OB Input Form
        [Route("GetOBInputForm")]
        [HttpPost]
        public IActionResult GetOBInputForm([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    List<ob_input_form> _list_form = GetCRM_OBInputForm();

                    // return successful get and display the list of data
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_form });

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

        private List<ob_input_form> GetCRM_OBInputForm()
        {
            // obtain data
            var _frms = (from _f in _scrme.ob_input_forms
                         where _f.Form_Status == "Active"
                         select _f);

            return _frms.ToList();

        }


        // Get OB Campaign
        [Route("GetOBCampaign")]
        [HttpPost]
        public IActionResult GetOBCampaign([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    List<ob_campaign> _list_form = GetCRM_OBCampaign();

                    // return successful get and display the list of data
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_form });

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

        private List<ob_campaign> GetCRM_OBCampaign()
        {
            // obtain data
            var _frms = (from _f in _scrme.ob_campaigns
                         where _f.Campaign_Status == "Active"
                         orderby _f.Created_Time descending
                         select _f).Take(500);

            return _frms.ToList();

        }


        // Get OB Batch
        [Route("GetOBBatch")]
        [HttpPost]
        public IActionResult GetOBBatch([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    List<ob_batch> _list_form = GetCRM_OBBatch();

                    // return successful get and display the list of data
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_form });

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

        private List<ob_batch> GetCRM_OBBatch()
        {
            // obtain data 
            var _frms = (from _f in _scrme.ob_batches
                         where _f.Batch_Status == "Active"
                         select _f);

            return _frms.ToList();

        }


        // Get OB Call Log
        [Route("GetOBCallLog")]
        [HttpPost]
        public IActionResult GetOBCallLog([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int callId = Convert.ToInt32((data["Call_Id"] ?? "-1").ToString());

                    List<ob_result_log> _list_call_log = GetCRM_OBCallLog(callId);

                    // return successful get and display the list of data
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_call_log });

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

        private List<ob_result_log> GetCRM_OBCallLog(int callId)
        {
            var _logs = (from _log in _scrme.ob_result_logs
                         where _log.Call_Id == callId
                         select _log);

            return _logs.ToList();

        }


        // Get OB Sales Order
        [Route("GetOBSalesOrder")]
        [HttpPost]
        public IActionResult GetOBSalesOrder([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int callId = Convert.ToInt32((data["Call_Id"] ?? "-1").ToString());

                    List<ob_sales_order> _list_rt = GetCRM_OBSalesOrder(callId);

                    // return successful get and display the list of data
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_rt });

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

        private List<ob_sales_order> GetCRM_OBSalesOrder(int callId)
        {
            var _rt = (from _r in _scrme.ob_sales_orders
                       where _r.Call_Id == callId && _r.Order_Status == "Active"
                       select _r);

            return _rt.ToList();

        }


        // Add OB Sales Order
        [Route("AddOBSalesOrder")]
        [HttpPost]
        public IActionResult AddOBSalesOrder([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string batchcode = (data["Batch_Code"] ?? "").ToString();
                    string campaigncode = (data["Campaign_Code"] ?? "").ToString();

                    if (batchcode == string.Empty || campaigncode == string.Empty)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        AddCRM_OBSalesOrder(data);
                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = "Order added" });
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

        private void AddCRM_OBSalesOrder(JsonObject data)
        {
            string batchcode = (data["Batch_Code"] ?? "").ToString();
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();

            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
            int callId = Convert.ToInt32((data["Call_Id"] ?? "-1").ToString());

            ob_sales_order _new_cp_item = new ob_sales_order();

            _new_cp_item.Batch_Code = batchcode;
            _new_cp_item.Campaign_Code = campaigncode;
            _new_cp_item.Call_Id = callId;
            _new_cp_item.Product_Code = (data["Product_Code"] ?? "").ToString();
            _new_cp_item.Plan_Code = (data["Plan_Code"] ?? "").ToString();
            _new_cp_item.Price = (data["Price"] ?? "").ToString();

            _new_cp_item.Order_Status = "Active";

            _new_cp_item.Created_By = agentId;
            _new_cp_item.Created_Time = DateTime.Now;
            _new_cp_item.Updated_By = agentId;
            _new_cp_item.Updated_Time = DateTime.Now;

            _scrme.ob_sales_orders.Add(_new_cp_item);

            _scrme.SaveChanges();

        }


        // Update OB Sales Order
        [Route("UpdateOBSalesOrder")]
        [HttpPut]
        public IActionResult UpdateOBSalesOrder([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_OBSalesOrder(data);
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

        private void UpdateCRM_OBSalesOrder(JsonObject data)
        {
            int pID = Convert.ToInt32((data["Sa_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());

            var _pro = (from _c in _scrme.ob_sales_orders
                        where _c.Sa_Id == pID
                        select _c).SingleOrDefault<ob_sales_order>();

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

                    if (fieldName != "Agent_Id" && fieldName != "Token")
                    {
                        PropertyInfo? fieldProp = new ob_sales_order().GetType().GetProperty(fieldName);
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


        // Update OB CallList
        [Route("UpdateOBCallList")]
        [HttpPut]
        public IActionResult UpdateOBCallList([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_OBCallList(data);
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

        private void UpdateCRM_OBCallList(JsonObject data)
        {
            int pID = Convert.ToInt32((data["Call_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());

            var _pro = (from _c in _scrme.ob_results
                        where _c.Call_Id == pID
                        select _c).SingleOrDefault<ob_result>();

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

                    if (fieldName != "Agent_Id" && fieldName != "Token")
                    {
                        PropertyInfo? fieldProp = new ob_result().GetType().GetProperty(fieldName);
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


                CopyTo_OBLog(_pro);


                _scrme.SaveChanges();

            }

        }

        void CopyTo_OBLog(ob_result _ob_item)
        {
            // declare db table items
            ob_result_log _log_item = new ob_result_log();

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
                _scrme.ob_result_logs.Add(_log_item);
            }
        }


        // Search OB CallList
        [Route("SearchOBCallList")]
        [HttpPost]
        public IActionResult SearchOBCallList([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string batchcode = (data["Batch_Code"] ?? "").ToString();
                    string campaigncode = (data["Campaign_Code"] ?? "").ToString();

                    if (batchcode == string.Empty || campaigncode == string.Empty)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        return Content(SearchCRM_OBCallList(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
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


        private JObject SearchCRM_OBCallList(JsonObject data)
        {
            // declare a list of json objects containing the each row of data
            List<JObject> _con_history_list = new List<JObject>();

            // declare a json object to contain all rows of data
            // JObject allJsonResults = new JObject(); //

            string batchcode = (data["Batch_Code"] ?? "").ToString();
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();

            int agent_id = Convert.ToInt32((data["To_Check_Id"] ?? "-1").ToString());

            string phone = (data["Phone"] ?? "").ToString();
            string name = (data["Name"] ?? "").ToString();
            string gender = (data["Gender"] ?? "").ToString();

            int age_from = Convert.ToInt32((data["Age_From"] ?? "-1").ToString());
            int age_to = Convert.ToInt32((data["Age_To"] ?? "-1").ToString());

            string call_status = (data["Call_Status"] ?? "").ToString();
            string callback_time_from = (data["Callback_Time_From"] ?? "").ToString();
            string callback_time_to = (data["Callback_Time_To"] ?? "").ToString();

            string within_batchperiod = (data["Within_BatchPeriod"] ?? "").ToString();


            List<string> conditionList = new List<string>();
            string whereClause = string.Empty;

            string statement = string.Empty;

            //     statement = " c.Is_Valid == \"Y\""; //

            //     conditionList.Add("(" + statement + ")"); //

            statement = " r.Opt_Out == null || !r.Opt_Out.Equals(\"Y\")";

            conditionList.Add("(" + statement + ")");

            // Only search assigned call list
            statement = " r.Agent_Id != null ";

            conditionList.Add("(" + statement + ")");

            if (batchcode != string.Empty)
            {
                statement = " r.Batch_Code.Equals(\"" + batchcode + "\")";
                conditionList.Add("(" + statement + ")");
            }

            if (campaigncode != string.Empty)
            {
                statement = " r.Campaign_Code.Equals(\"" + campaigncode + "\")";
                conditionList.Add("(" + statement + ")");
            }


            if (agent_id != -1)
            {
                statement = " r.Agent_Id == " + agent_id;

                conditionList.Add("(" + statement + ")");

            }

            if (phone != string.Empty)
            {
                statement = " r.Home_No.Contains(\"" + phone + "\")" + " || "
                          + " r.Office_No.Contains(\"" + phone + "\")" + " || "
                          + " r.Mobile_No.Contains(\"" + phone + "\")" + " || "
                          + " r.Other_Phone_No.Contains(\"" + phone + "\")";

                conditionList.Add("(" + statement + ")");
            }

            if (name != string.Empty)
            {
                statement = " r.First_Name.Contains(\"" + name + "\")" + " || "
                          + " r.Last_Name.Contains(\"" + name + "\")";

                conditionList.Add("(" + statement + ")");
            }

            if (gender != string.Empty)
            {
                statement = " r.Gender.Equals(\"" + gender + "\")";

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

                statement = " r.DOB != null" + " && "
                          + " r.DOB > " + "DateTime(" + min_year + "," + min_month + "," + min_day + ",0,0,0)" + " && "
                          + " r.DOB <= " + "DateTime(" + max_year + "," + max_month + "," + max_day + ",0,0,0)";

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


            if (within_batchperiod == "Y")
            {
                statement = " b.Batch_Start_Date != null && b.Batch_End_Date != null "
                            + " && DateTime.Now >= b.Batch_Start_Date "
                            + " && DateTime.Now.Date <= b.Batch_End_Date";

                conditionList.Add("(" + statement + ")");
            }


            whereClause = string.Join(" AND ", conditionList.ToArray());


            var _return_customer = (from r in _scrme.ob_results
                                    join b in _scrme.ob_batches on new { r.Campaign_Code, r.Batch_Code } equals new { b.Campaign_Code, b.Batch_Code }
                                    select new { r, b }
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

                sortColumn_list = "r.";


                final_ordering = sortColumn_list + sortColumn + (isDesc ? " descending" : "");


                // Sorting 2nd
                if (data["order"]?.AsArray()?.Count > 1)
                {
                    int col_index2 = data["order"]?[1]?["column"]?.GetValue<int>() ?? -1;
                    string sortColumn2 = data["columns"]?[col_index2]?["data"]?.GetValue<string>() ?? string.Empty;
                    string sortColumnDir2 = data["order"]?[1]?["dir"]?.GetValue<string>() ?? "asc";

                    string sortColumn_list2 = "r.";

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


                foreach (PropertyInfo property in _result_item.r.GetType().GetProperties())
                {
                    // add only the following column names and values to temp

                    tempJson.Add(new JProperty(property.Name, property.GetValue(_result_item.r)));

                }
                _con_history_list.Add(tempJson); // add the temp result to the list
            }

            // set up _all_results json data
            JObject allJsonResults = new JObject()
            {
                new JProperty("result", "success"),
                new JProperty("draw", draw),
                new JProperty("recordsFiltered", recordsTotal),
                new JProperty("recordsTotal", recordsTotal),
                new JProperty("details", _con_history_list)
            };

            // return all results in json format
            return allJsonResults;

        }


        // Get OB Batch Lead Count
        [Route("GetOBBatchLeadCount")]
        [HttpPost]
        public IActionResult GetOBBatchLeadCount([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string batchcode = (data["Batch_Code"] ?? "").ToString();
                    string campaigncode = (data["Campaign_Code"] ?? "").ToString();
                    int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

                    if (batchcode == string.Empty || campaigncode == string.Empty || assign_from == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        int leadcount = GetCRM_OBBatchLead(data).Count();

                        return Ok(new
                        {
                            result = AppOutp.OutputResult_SUCC,
                            details = new
                            {
                                LeadCount = leadcount
                            }
                        });

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

        private IQueryable<ob_result> GetCRM_OBBatchLead([FromBody] dynamic data)
        {
            string batchcode = (data["Batch_Code"] ?? "").ToString();
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();

            int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

            string call_status = (data["Call_Status"] ?? "").ToString();
            string call_reason = (data["Call_Reason"] ?? "").ToString();

            string gender = (data["Gender"] ?? "").ToString();

            int age_from = Convert.ToInt32((data["Age_From"] ?? "-1").ToString());
            int age_to = Convert.ToInt32((data["Age_To"] ?? "-1").ToString());


            IQueryable<ob_result> _pro = from _r in _scrme.ob_results
                                         where _r.Batch_Code == batchcode && _r.Campaign_Code == campaigncode &&
                                              (_r.Opt_Out == null || !_r.Opt_Out.Equals("Y"))
                                         select _r;

            if (assign_from == 0)
            {
                _pro = _pro.Where(_c => _c.Attempt == 0 && _c.Agent_Id == null);

            }
            else if (assign_from == -999)
            {
                if (call_status == "NewLead")
                {
                    _pro = _pro.Where(_c => _c.Attempt == 0 && _c.Agent_Id != null);
                }
                else
                {
                    _pro = _pro.Where(_c => _c.Call_Status == call_status && _c.Call_Reason == call_reason && _c.Agent_Id != null);
                }
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

            if (gender != "")
            {
                _pro = _pro.Where(_c => _c.Gender == gender);
            }

            if (age_from != -1 && age_to != -1)
            {
                DateTime today = DateTime.Today;
                DateTime min = today.AddYears(-(age_to + 1));
                DateTime max = today.AddYears(-age_from);

                _pro = _pro.Where(_c => _c.DOB != null && _c.DOB > min && _c.DOB <= max);
            }


            return _pro;
        }


        // Assign OB Batch Lead
        [Route("AssignOBBatchLead")]
        [HttpPost]
        public IActionResult AssignOBBatchLead([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string batchcode = (data["Batch_Code"] ?? "").ToString();
                    string campaigncode = (data["Campaign_Code"] ?? "").ToString();
                    int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

                    if (batchcode == string.Empty || campaigncode == string.Empty || assign_from == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        int upd_result = AssignCRM_OBBatchLead(data);

                        if (upd_result == -1)
                        {
                            return Ok(new { result = AppOutp.OutputResult_FAIL, details = "No lead / Not enough lead to assign" });
                        }
                        else
                        {
                            return Ok(new { result = AppOutp.OutputResult_SUCC, details = "Assignment done" });
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

        private int AssignCRM_OBBatchLead(JsonObject data)
        {
            IQueryable<ob_result> _return_lead = GetCRM_OBBatchLead(data);

            int tot_count = _return_lead.Count();

            int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

            int assign_total;

            if (assign_from == -999)
            {
                assign_total = tot_count;
            }
            else
            {
                assign_total = Convert.ToInt32((data["Assign_Total"] ?? "-1").ToString());
            }

            if (tot_count > 0 && assign_total > 0 && tot_count >= assign_total)
            {
                int assign_to = Convert.ToInt32((data["Assign_To"] ?? "-1").ToString());

                if (assign_to == 0)
                {
                    _return_lead.OrderBy(x => Guid.NewGuid()).Take(assign_total)
                        .Update(x => new ob_result { Agent_Id = null });
                }
                else
                {
                    _return_lead.OrderBy(x => Guid.NewGuid()).Take(assign_total)
                        .Update(x => new ob_result { Agent_Id = assign_to });
                }

                // ------------------------------------------------------------------
                ob_assignment_history _new_cp_item = new ob_assignment_history();

                _new_cp_item.Assignment_Details = Convert.ToString(data);

                _new_cp_item.Created_By = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
                _new_cp_item.Created_Time = DateTime.Now;


                _scrme.ob_assignment_histories.Add(_new_cp_item);

                _scrme.SaveChanges();
                // ------------------------------------------------------------------


                return 1;
            }
            else
            {
                return -1;
            }

        }


        // Get OB Campaign Header
        [Route("GetOBCampaignHeader")]
        [HttpPost]
        public IActionResult GetOBCampaignHeader([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string campaigncode = (data["Campaign_Code"] ?? "").ToString();

                    List<ob_header_mapping> _list_form = GetCRM_OBCampaignHeader(campaigncode);

                    // return successful get and display the list of data
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = _list_form });

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

        private List<ob_header_mapping> GetCRM_OBCampaignHeader(string campaigncode)
        {
            // obtain data
            var _frms = (from _f in _scrme.ob_header_mappings
                         where _f.Campaign_Code == campaigncode
                         select _f);

            return _frms.ToList();

        }


        // Add OB Campaign
        [Route("AddOBCampaign")]
        [HttpPost]
        public IActionResult AddOBCampaign([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int campaignId = AddCRM_OBCampaign(data);

                    if (campaignId != -1)
                    {
                        return Ok(new
                        {
                            result = AppOutp.OutputResult_SUCC,
                            details = new
                            {
                                Campaign_Id = campaignId
                            }
                        });
                    }
                    else
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Campaign Code already exists" });
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

        private int AddCRM_OBCampaign(JsonObject data)
        {
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();

            var _cp = (from _c in _scrme.ob_campaigns
                       where _c.Campaign_Status == "Active" && _c.Campaign_Code == campaigncode
                       select _c);

            // exists
            if (_cp.Count() > 0)
            {
                return -1;
            }
            else
            {
                int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
                int formId = Convert.ToInt32((data["Form_Id"] ?? "-1").ToString());

                ob_campaign _new_cp_item = new ob_campaign();

                _new_cp_item.Campaign_Code = campaigncode;
                _new_cp_item.Form_Id = formId;
                _new_cp_item.Form_Name = (data["Form_Name"] ?? "").ToString();
                _new_cp_item.Campaign_Description = (data["Campaign_Description"] ?? "").ToString();
                _new_cp_item.Campaign_Status = "Active";
                _new_cp_item.Created_By = agentId;
                _new_cp_item.Created_Time = DateTime.Now;
                _new_cp_item.Updated_By = agentId;
                _new_cp_item.Updated_Time = DateTime.Now;

                _scrme.ob_campaigns.Add(_new_cp_item);

                _scrme.SaveChanges();

                return _new_cp_item.Campaign_Id;
            }

        }


        // Update OB Campaign
        [Route("UpdateOBCampaign")]
        [HttpPut]
        public IActionResult UpdateOBCampaign([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_OBCampaign(data);
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

        private void UpdateCRM_OBCampaign(JsonObject data)
        {
            int cID = Convert.ToInt32((data["Campaign_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
            string s_action = (data["S_Action"] ?? "").ToString();

            var _cs = (from _c in _scrme.ob_campaigns
                       where _c.Campaign_Id == cID
                       select _c).SingleOrDefault<ob_campaign>();

            // exists in table
            if (_cs != null)
            {
                if (s_action == "Amend")
                {
                    _cs.Campaign_Code = (data["Campaign_Code"] ?? "").ToString();
                    _cs.Form_Id = Convert.ToInt32((data["Form_Id"] ?? "-1").ToString());
                    _cs.Form_Name = (data["Form_Name"] ?? "").ToString();
                    _cs.Campaign_Description = (data["Campaign_Description"] ?? "").ToString();

                    _cs.Updated_By = agentId;
                    _cs.Updated_Time = DateTime.Now;

                    _scrme.SaveChanges();
                }
                else if (s_action == "Delete")
                {
                    _cs.Campaign_Status = "InActive";

                    _cs.Updated_By = agentId;
                    _cs.Updated_Time = DateTime.Now;

                    _scrme.SaveChanges();
                }
            }

        }

        // Add OB Campaign Header
        [Route("AddOBCampaignHeader")]
        [HttpPost]
        public IActionResult AddOBCampaignHeader([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    AddCRM_OBCampaignHeader(data);
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = "Header Added" });
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

        private void AddCRM_OBCampaignHeader(JsonObject data)
        {
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
            int campaignId = Convert.ToInt32((data["Campaign_Id"] ?? "-1").ToString());
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();

            var _ch = (from _c in _scrme.ob_header_mappings
                       where _c.Campaign_Code == campaigncode
                       select _c);

            // exists
            if (_ch.Count() > 0)
            {
                _scrme.ob_header_mappings.RemoveRange(_ch);
            }

            var hcArray = data["HeaderColumnArr"] as JsonArray;

            foreach (JsonObject jsonObj in hcArray.OfType<JsonObject>())
            {
                var _new_h_item = new ob_header_mapping
                {
                    Campaign_Id = campaignId,
                    Campaign_Code = campaigncode,
                    Excel_Field_Order = jsonObj["Excel_Field_Order"]?.GetValue<int>() ?? -1,
                    Excel_Field_Name = jsonObj["Excel_Field_Name"]?.GetValue<string>() ?? "",
                    DB_Field_Name = jsonObj["DB_Field_Name"]?.GetValue<string>() ?? "",
                    Check_Type = jsonObj["Check_Type"]?.GetValue<string>() ?? "",
                    Created_By = agentId,
                    Created_Time = DateTime.Now
                };

                _scrme.ob_header_mappings.Add(_new_h_item);
            }

            _scrme.SaveChanges();

            string filePath = (data["File_Path"] ?? "").ToString();
            if (filePath != "")
            {
                System.IO.File.Delete(filePath);
            }

        }


        // Check OB Excel
        [Route("CheckOBExcel")]
        [HttpPost]
        public async Task<IActionResult> CheckOBExcel([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string upload_status = await CheckCRM_upload_file(data);

                    return Ok(new
                    {
                        result = AppOutp.OutputResult_SUCC,
                        details = new
                        {
                            upload_status
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


        private async Task<string> CheckCRM_upload_file(JsonObject data)
        {
            string filePath = (data["File_Path"] ?? "").ToString();
            string worksheet = (data["WorkSheet"] ?? "").ToString();
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();

            var _frms = (from _f in _scrme.ob_header_mappings
                         where _f.Campaign_Code == campaigncode
                         select _f);

            int count_header = await _frms.CountAsync();

            string strConn = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
            string query = $"SELECT * FROM [{worksheet}]";

            DataTable dtExcelData = new DataTable();

            using var conn = new OleDbConnection(strConn);
            using var oda = new OleDbDataAdapter(query, conn);

            await conn.OpenAsync();
            oda.Fill(dtExcelData);

            var fileColumnNames = dtExcelData.Columns
                .Cast<DataColumn>()
                .Where(col => !string.IsNullOrEmpty(col.ColumnName))
                .Select(col => col.ColumnName)
                .ToList();

            int count_fileheader = fileColumnNames.Count;

            if (count_fileheader != count_header)
            {
                return "Number of columns in Excel does not match with campaign setting.";
            }
            else
            {
                dtExcelData.Columns.Add("Campaign_Code", typeof(string));

                int no_of_uploadedrows = dtExcelData.Rows.Count;

                for (int i = 0; i < no_of_uploadedrows; i++)
                {
                    dtExcelData.Rows[i]["Campaign_Code"] = campaigncode;
                }


                var sql_del = $"delete from ob_temp_upload where Campaign_Code = @CampaignCode ";
                await _scrme.Database.ExecuteSqlRawAsync(sql_del, new SqlParameter("@CampaignCode", campaigncode));


                var mappings = await _scrme.ob_header_mappings
                    .Where(m => m.Campaign_Code == campaigncode)
                    .Select(m => new { m.Excel_Field_Name, m.DB_Field_Name, m.Check_Type })
                    .ToListAsync();

                var dateFields = mappings
                    .Where(m => m.Check_Type == "Date")
                    .Select(m => m.DB_Field_Name)
                    .ToHashSet();

                // Start a transaction
                using var transaction = await _scrme.Database.BeginTransactionAsync();
                var connection = _scrme.Database.GetDbConnection() as SqlConnection
                    ?? throw new InvalidOperationException("The database connection is not a SqlConnection.");


                // Perform SqlBulkCopy
                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction.GetDbTransaction() as SqlTransaction))
                {
                    bulkCopy.DestinationTableName = "ob_temp_upload";
                    foreach (var mapping in mappings)
                    {
                        bulkCopy.ColumnMappings.Add(mapping.Excel_Field_Name, mapping.DB_Field_Name);
                    }
                    bulkCopy.ColumnMappings.Add("Campaign_Code", "Campaign_Code");
                    await bulkCopy.WriteToServerAsync(dtExcelData);
                }


                var safeDateFields = new HashSet<string>();

                if (dateFields.Any())
                {
                    // Retrieve valid column names for ob_temp_upload from the database
                    var columnQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ob_temp_upload'";
                    var allowedColumns = new HashSet<string>();

                    using (var command = _scrme.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = columnQuery;
                        command.Transaction = transaction.GetDbTransaction(); // Associate with the transaction

                        using var reader = await command.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            allowedColumns.Add(reader.GetString(0));
                        }
                    }

                    // Filter dateFields to only include valid column names
                    safeDateFields = dateFields.Intersect(allowedColumns, StringComparer.OrdinalIgnoreCase).ToHashSet();
                }


                // Validate date fields
                var invalidDateFields = new HashSet<string>();
                foreach (var dateFieldOne in safeDateFields)
                {
                    var sql = $"SELECT COUNT(*) FROM ob_temp_upload WHERE Campaign_Code = @CampaignCode AND {dateFieldOne} IS NOT NULL AND ISDATE({dateFieldOne}) = 0";

                    // Create a command using the DbContext's connection
                    using var command = _scrme.Database.GetDbConnection().CreateCommand();
                    command.CommandText = sql; // Set the SQL query
                    command.Transaction = transaction.GetDbTransaction(); // Associate with the transaction
                    command.Parameters.Add(new SqlParameter("@CampaignCode", campaigncode)); // Add parameter

                    // Execute the query and get the scalar result
                    var invalidDateCount = await command.ExecuteScalarAsync();

                    int inv = Convert.ToInt32(invalidDateCount);

                    if (inv > 0)
                    {
                        invalidDateFields.Add(dateFieldOne);
                    }
                }

                if (!invalidDateFields.Any())
                {
                    await transaction.CommitAsync();
                    return "Checked OK. Total No. of records: " + no_of_uploadedrows;
                }
                else
                {
                    await transaction.RollbackAsync();
                    var err_msg = string.Join(", ", invalidDateFields);
                    return "Date type problem for Column(s): " + err_msg;
                }


            }
        }


        // Confirm Upload OB Excel
        [Route("ConfirmUploadOBExcel")]
        [HttpPost]
        public async Task<IActionResult> ConfirmUploadOBExcel([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string upload_status = await ConfirmCRM_upload_file(data);

                    return Ok(new
                    {
                        result = AppOutp.OutputResult_SUCC,
                        details = new
                        {
                            upload_status
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

        private async Task<string> ConfirmCRM_upload_file(JsonObject data)
        {
            string batchcode = (data["Batch_Code"] ?? "").ToString();
            string campaigncode = (data["Campaign_Code"] ?? "").ToString();
            string b_start = (data["Batch_Start_Date"] ?? "").ToString();
            string b_end = (data["Batch_End_Date"] ?? "").ToString();
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());

            var _b = (from _c in _scrme.ob_batches
                      where _c.Batch_Status == "Active" && _c.Campaign_Code == campaigncode && _c.Batch_Code == batchcode
                      select _c);

            // exists
            if (await _b.CountAsync() > 0)
            {
                return "Batch Code of this campaign already exists. Please assign a new Batch Code.";
            }
            else
            {
                using var transaction = await _scrme.Database.BeginTransactionAsync();

                var sql = "uploadOB_CallList"; // Name of the stored procedure
                using var command = _scrme.Database.GetDbConnection().CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure; // Set to StoredProcedure
                command.Transaction = transaction.GetDbTransaction(); // Associate with the transaction
                command.Parameters.Add(new SqlParameter("@Campaign_Code", campaigncode));
                command.Parameters.Add(new SqlParameter("@Batch_Code", batchcode));
                command.Parameters.Add(new SqlParameter("@Batch_Start", b_start));
                command.Parameters.Add(new SqlParameter("@Batch_End", b_end));
                command.Parameters.Add(new SqlParameter("@Agent_Id", agentId));

                var ret_status = await command.ExecuteScalarAsync();
                await transaction.CommitAsync();

                string filePath = (data["File_Path"] ?? "").ToString();

                if (filePath != "")
                {
                    System.IO.File.Delete(filePath);
                }

                return ret_status?.ToString() ?? "";

            }

        }





    }
}
