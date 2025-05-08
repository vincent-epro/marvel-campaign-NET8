using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json.Nodes;

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







    }
}
