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
using ExcelDataReader;

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
                         where _f.Form_Status == AppOutp.STATUS_Active
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
                         where _f.Campaign_Status == AppOutp.STATUS_Active
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
                         where _f.Batch_Status == AppOutp.STATUS_Active
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
                       where _r.Call_Id == callId && _r.Order_Status == AppOutp.STATUS_Active
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
                    string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

                    if (batchcode == string.Empty || campaigncode == string.Empty)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });
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
            string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
            string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
            int callId = Convert.ToInt32((data["Call_Id"] ?? "-1").ToString());

            ob_sales_order _new_cp_item = new ob_sales_order();

            _new_cp_item.Batch_Code = batchcode;
            _new_cp_item.Campaign_Code = campaigncode;
            _new_cp_item.Call_Id = callId;
            _new_cp_item.Product_Code = (data["Product_Code"] ?? "").ToString();
            _new_cp_item.Plan_Code = (data["Plan_Code"] ?? "").ToString();
            _new_cp_item.Price = (data["Price"] ?? "").ToString();

            _new_cp_item.Order_Status = AppOutp.STATUS_Active;

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
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

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

                    if (fieldName != AppInp.InputAuth_Agent_Id && fieldName != "Token")
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
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

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

                    if (fieldName != AppInp.InputAuth_Agent_Id && fieldName != "Token")
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
                    string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

                    if (batchcode == string.Empty || campaigncode == string.Empty)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });
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
            var conditionList = GenerateConditions(data);
            var whereClause = string.Join(" AND ", conditionList);
            var filteredCustomers = FilterCustomerRecords(whereClause);
            var sortedCustomers = SortCustomers(data, filteredCustomers);
            var pagedCustomers = PaginateResults(data, sortedCustomers);

            return GenerateFinalResult(data, pagedCustomers, sortedCustomers.Count());
        }

        // Generate search conditions
        private static List<string> GenerateConditions(JsonObject data)
        {
            var conditions = new List<string>
            {
                "(r.Opt_Out == null || !r.Opt_Out.Equals(\"Y\"))",
                "(r.Agent_Id != null)"
            };

            AddCondition(conditions, "r.Batch_Code.Equals", data[AppInp.Input_Batch_Code]);
            AddCondition(conditions, "r.Campaign_Code.Equals", data[AppInp.Input_Campaign_Code]);
            AddCondition(conditions, "r.Agent_Id == ", data["To_Check_Id"], true);
            AddCondition(conditions, "r.Gender.Equals", data["Gender"]);
            AddAgeCondition(conditions, data["Age_From"], data["Age_To"]);
            AddPhoneCondition(conditions, data["Phone"]);
            AddNameCondition(conditions, data["Name"]);
            AddCallStatusCondition(conditions, data["Call_Status"]);
            AddCallbackCondition(conditions, data["Callback_Time_From"], data["Callback_Time_To"]);
            AddBatchPeriodCondition(conditions, data["Within_BatchPeriod"]);

            return conditions;
        }

        // Add simple conditions
        private static void AddCondition(List<string> conditions, string format, object value, bool isInt = false)
        {
            var val = (value ?? "").ToString();
            if (!string.IsNullOrEmpty(val) && (!isInt || Convert.ToInt32(val) != -1))
            {
                conditions.Add($"({format}(\"{val}\"))");
            }
        }

        // Add age-based conditions
        private static void AddAgeCondition(List<string> conditions, object ageFrom, object ageTo)
        {
            int ageFromInt = Convert.ToInt32((ageFrom ?? "-1").ToString());
            int ageToInt = Convert.ToInt32((ageTo ?? "-1").ToString());

            if (ageFromInt != -1 && ageToInt != -1)
            {
                DateTime today = DateTime.Today;
                DateTime min = today.AddYears(-(ageToInt + 1));
                DateTime max = today.AddYears(-ageFromInt);

                conditions.Add($"(r.DOB != null && r.DOB > DateTime({min.Year},{min.Month},{min.Day},0,0,0) && r.DOB <= DateTime({max.Year},{max.Month},{max.Day},0,0,0))");
            }
        }

        // Add phone number conditions
        private static void AddPhoneCondition(List<string> conditions, object phone)
        {
            var phoneValue = (phone ?? "").ToString();
            if (!string.IsNullOrEmpty(phoneValue))
            {
                conditions.Add($"(r.Home_No.Contains(\"{phoneValue}\") || r.Office_No.Contains(\"{phoneValue}\") || r.Mobile_No.Contains(\"{phoneValue}\") || r.Other_Phone_No.Contains(\"{phoneValue}\"))");
            }
        }

        // Add name conditions
        private static void AddNameCondition(List<string> conditions, object name)
        {
            var nameValue = (name ?? "").ToString();
            if (!string.IsNullOrEmpty(nameValue))
            {
                conditions.Add($"(r.First_Name.Contains(\"{nameValue}\") || r.Last_Name.Contains(\"{nameValue}\"))");
            }
        }

        // Add call status conditions
        private static void AddCallStatusCondition(List<string> conditions, object callStatus)
        {
            var statusValue = (callStatus ?? "").ToString();
            if (!string.IsNullOrEmpty(statusValue))
            {
                conditions.Add(statusValue == "NewLead" ? "(r.Attempt == 0)" : $"(r.Call_Status.Equals(\"{statusValue}\"))");
            }
        }

        // Add callback time conditions
        private static void AddCallbackCondition(List<string> conditions, object callbackFrom, object callbackTo)
        {
            var callbackStart = (callbackFrom ?? "").ToString();
            var callbackEnd = (callbackTo ?? "").ToString();
            if (!string.IsNullOrEmpty(callbackStart) && !string.IsNullOrEmpty(callbackEnd))
            {
                double s_min = TimeSpan.ParseExact(callbackStart, @"hh\:mm", CultureInfo.InvariantCulture).TotalMinutes;
                double e_min = TimeSpan.ParseExact(callbackEnd, @"hh\:mm", CultureInfo.InvariantCulture).TotalMinutes;
                conditions.Add($"(r.Callback_Time.HasValue && ((r.Callback_Time.Value.Hour * 60) + r.Callback_Time.Value.Minute) >= {s_min} && ((r.Callback_Time.Value.Hour * 60) + r.Callback_Time.Value.Minute) <= {e_min})");
            }
        }

        // Add batch period conditions
        private static void AddBatchPeriodCondition(List<string> conditions, object batchPeriod)
        {
            if ((batchPeriod ?? "").ToString() == "Y")
            {
                conditions.Add("(b.Batch_Start_Date != null && b.Batch_End_Date != null && DateTime.Now >= b.Batch_Start_Date && DateTime.Now.Date <= b.Batch_End_Date)");
            }
        }

        // Filter customer records
        private IQueryable<dynamic> FilterCustomerRecords(string whereClause)
        {
            return _scrme.ob_results.Join(_scrme.ob_batches, r => new { r.Campaign_Code, r.Batch_Code },
                                          b => new { b.Campaign_Code, b.Batch_Code },
                                          (r, b) => new { r, b }).Where(whereClause);
        }

        // Sort customer records
        private static IQueryable<dynamic> SortCustomers(JsonObject data, IQueryable<dynamic> customers)
        {
            int colIndex = data["order"]?[0]?["column"]?.GetValue<int>() ?? -1;
            string sortColumn = data["columns"]?[colIndex]?["data"]?.GetValue<string>() ?? string.Empty;
            string sortColumnDir = data["order"]?[0]?["dir"]?.GetValue<string>() ?? "asc";

            if (!string.IsNullOrEmpty(sortColumn))
            {
                string ordering = $"r.{sortColumn} {(sortColumnDir == "desc" ? "descending" : "ascending")}";

                if (data["order"]?.AsArray()?.Count > 1)
                {
                    int colIndex2 = data["order"]?[1]?["column"]?.GetValue<int>() ?? -1;
                    string sortColumn2 = data["columns"]?[colIndex2]?["data"]?.GetValue<string>() ?? string.Empty;
                    string sortColumnDir2 = data["order"]?[1]?["dir"]?.GetValue<string>() ?? "asc";

                    if (!string.IsNullOrEmpty(sortColumn2))
                    {
                        ordering += $", r.{sortColumn2} {(sortColumnDir2 == "desc" ? "descending" : "ascending")}";
                    }
                }

                customers = customers.OrderBy(ordering);
            }

            return customers;
        }

        // Paginate results
        private static List<dynamic> PaginateResults(JsonObject data, IQueryable<dynamic> customers)
        {
            int pageSize = Convert.ToInt32((data["length"] ?? "0").ToString());
            int skip = Convert.ToInt32((data["start"] ?? "0").ToString());
            return customers.Skip(skip).Take(pageSize).ToList();
        }

        // Generate final JSON result
        private static JObject GenerateFinalResult(JsonObject data, List<dynamic> customers, int totalRecords)
        {
            var conHistoryList = customers.Select(c =>
            {
                JObject obj = new JObject();
                foreach (var property in c.r.GetType().GetProperties())
                {
                    obj.Add(new JProperty(property.Name, property.GetValue(c.r)));
                }
                return obj;
            }).ToList();

            return new JObject
            {
                new JProperty(AppOutp.OutputResult_Field, "success"),
                new JProperty("draw", Convert.ToInt32((data["draw"] ?? "-1").ToString())),
                new JProperty("recordsFiltered", totalRecords),
                new JProperty("recordsTotal", totalRecords),
                new JProperty(AppOutp.OutputDetails_Field, conHistoryList)
            };
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
                    string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();
                    int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

                    if (batchcode == string.Empty || campaigncode == string.Empty || assign_from == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });
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
            string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
            string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

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


            _pro = ApplyAssignmentFilter(_pro, assign_from, call_status, call_reason);


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

        private static IQueryable<ob_result> ApplyAssignmentFilter(IQueryable<ob_result> query, int assign_from, string call_status, string call_reason)
        {
            if (assign_from == 0)
                return query.Where(_c => _c.Attempt == 0 && _c.Agent_Id == null);

            if (assign_from == -999)
                return call_status == "NewLead"
                    ? query.Where(_c => _c.Attempt == 0 && _c.Agent_Id != null)
                    : query.Where(_c => _c.Call_Status == call_status && _c.Call_Reason == call_reason && _c.Agent_Id != null);

            return call_status == "NewLead"
                ? query.Where(_c => _c.Attempt == 0 && _c.Agent_Id == assign_from)
                : query.Where(_c => _c.Call_Status == call_status && _c.Call_Reason == call_reason && _c.Agent_Id == assign_from);
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
                    string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();
                    int assign_from = Convert.ToInt32((data["Assign_From"] ?? "-1").ToString());

                    if (batchcode == string.Empty || campaigncode == string.Empty || assign_from == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });
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

                _new_cp_item.Created_By = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
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
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

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
            string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

            var _cp = (from _c in _scrme.ob_campaigns
                       where _c.Campaign_Status == AppOutp.STATUS_Active && _c.Campaign_Code == campaigncode
                       select _c);

            // exists
            if (_cp.Count() > 0)
            {
                return -1;
            }
            else
            {
                int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
                int formId = Convert.ToInt32((data["Form_Id"] ?? "-1").ToString());

                ob_campaign _new_cp_item = new ob_campaign();

                _new_cp_item.Campaign_Code = campaigncode;
                _new_cp_item.Form_Id = formId;
                _new_cp_item.Form_Name = (data["Form_Name"] ?? "").ToString();
                _new_cp_item.Campaign_Description = (data["Campaign_Description"] ?? "").ToString();
                _new_cp_item.Campaign_Status = AppOutp.STATUS_Active;
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
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
            string s_action = (data["S_Action"] ?? "").ToString();

            var _cs = (from _c in _scrme.ob_campaigns
                       where _c.Campaign_Id == cID
                       select _c).SingleOrDefault<ob_campaign>();

            // exists in table
            if (_cs != null)
            {
                if (s_action == "Amend")
                {
                    _cs.Campaign_Code = (data[AppInp.Input_Campaign_Code] ?? "").ToString();
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
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());
            int campaignId = Convert.ToInt32((data["Campaign_Id"] ?? "-1").ToString());
            string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

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
            
            // Ensure worksheet name doesn't end with "$"
            string worksheet = (data["WorkSheet"] ?? "").ToString().TrimEnd('$');

            string campaignCode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();

            // Validate header count
            int expectedHeaderCount = await _scrme.ob_header_mappings
                .CountAsync(f => f.Campaign_Code == campaignCode);

            // Read Excel data
            DataTable excelData = ReadExcelFile(filePath, worksheet);

            if (excelData.Columns.Count != expectedHeaderCount)
            {
                return "Number of columns in Excel does not match with campaign setting.";
            }

            // Add Campaign_Code column
            excelData.Columns.Add(AppInp.Input_Campaign_Code, typeof(string));
            foreach (DataRow row in excelData.Rows)
            {
                row[AppInp.Input_Campaign_Code] = campaignCode;
            }

            // Delete existing temp data
            await _scrme.Database.ExecuteSqlRawAsync(
                "DELETE FROM ob_temp_upload WHERE Campaign_Code = @CampaignCode",
                new SqlParameter("@CampaignCode", campaignCode));

            // Get column mappings
            var mappings = await _scrme.ob_header_mappings
                .Where(m => m.Campaign_Code == campaignCode)
                .Select(m => new { m.Excel_Field_Name, m.DB_Field_Name, m.Check_Type })
                .ToListAsync();

            // Start transaction
            using var transaction = await _scrme.Database.BeginTransactionAsync();
            var connection = _scrme.Database.GetDbConnection() as SqlConnection
                ?? throw new InvalidOperationException("The database connection is not a SqlConnection.");

            // Perform bulk copy
            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction.GetDbTransaction() as SqlTransaction);
            bulkCopy.DestinationTableName = "ob_temp_upload";
            foreach (var mapping in mappings)
            {
                bulkCopy.ColumnMappings.Add(mapping.Excel_Field_Name, mapping.DB_Field_Name);
            }
            bulkCopy.ColumnMappings.Add(AppInp.Input_Campaign_Code, AppInp.Input_Campaign_Code);
            await bulkCopy.WriteToServerAsync(excelData);
          
            await transaction.CommitAsync();

            // Validate date fields
            var dateFields = mappings
                .Where(m => m.Check_Type == "Date")
                .Select(m => m.DB_Field_Name)
                .ToHashSet();
            var invalidDateFields = await ValidateDateFields(dateFields, campaignCode);
            if (invalidDateFields.Any())
            {
             //   await transaction.RollbackAsync();//
                return $"Date type problem for Column(s): {string.Join(", ", invalidDateFields)}";
            }

           // await transaction.CommitAsync();//
            return $"Checked OK. Total No. of records: {excelData.Rows.Count}";
        }

        private static DataTable ReadExcelFile(string filePath, string worksheetName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    if (result.Tables.Contains(worksheetName))
                    {
                        return result.Tables[worksheetName];
                    }
                    else
                    {
                        throw new ArgumentException("Worksheet name not found.");
                    }
                }
            }
        }

        private async Task<HashSet<string>> ValidateDateFields(HashSet<string> dateFields, string campaignCode)
        {
            var invalidDateFields = new HashSet<string>();
            foreach (var dateField in dateFields)
            {
                var entries = await _scrme.ob_temp_uploads
                    .Where(x => x.Campaign_Code == campaignCode && EF.Property<string>(x, dateField) != null)
                    .Select(x => EF.Property<string>(x, dateField))
                    .ToListAsync();

                if (entries.Any(dateValue => !string.IsNullOrWhiteSpace(dateValue) && !IsValidDateTimeFormat(dateValue.Trim())))
                {
                    invalidDateFields.Add(dateField);
                }
            }
            return invalidDateFields;
        }

        private static bool IsValidDateTimeFormat(string dateValue)
        {
            if (string.IsNullOrWhiteSpace(dateValue))
                return true;

            // Define common date formats, including those with AM/PM in different languages
            string[] formats =
            {
                "yyyy-MM-dd",
                "MM/dd/yyyy",
                "dd/MM/yyyy",
                "yyyy/MM/dd",
                "yyyy/M/d tt h:mm:ss", // For formats like "2000/8/19 上午 12:00:00"
                "yyyy/M/d tth:mm:ss",  // Alternative for some locales
                "yyyy/MM/dd tt h:mm:ss",
                "yyyy-MM-dd HH:mm:ss",
                "dd-MMM-yyyy HH:mm:ss"
            };

            // Try parsing with invariant culture first
            if (DateTime.TryParseExact(dateValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return true;

            // Try parsing with specific cultures (e.g., Chinese, Japanese, etc.)
            var cultures = new[]
            {
                new CultureInfo("zh-TW"), // Traditional Chinese (Taiwan)
                new CultureInfo("zh-CN"), // Simplified Chinese (China)
                new CultureInfo("ja-JP"), // Japanese
                CultureInfo.CurrentCulture // System's current culture
            };

            foreach (var culture in cultures)
            {
                if (DateTime.TryParseExact(dateValue, formats, culture, DateTimeStyles.None, out _) ||
                    DateTime.TryParse(dateValue, culture, DateTimeStyles.None, out _))
                    return true;
            }

            return false;
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
            string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
            string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();
            string b_start = (data["Batch_Start_Date"] ?? "").ToString();
            string b_end = (data["Batch_End_Date"] ?? "").ToString();
            int agentId = Convert.ToInt32((data[AppInp.InputAuth_Agent_Id] ?? "-1").ToString());

            var _b = (from _c in _scrme.ob_batches
                      where _c.Batch_Status == AppOutp.STATUS_Active && _c.Campaign_Code == campaigncode && _c.Batch_Code == batchcode
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
