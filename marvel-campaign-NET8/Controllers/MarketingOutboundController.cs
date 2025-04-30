using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Z.EntityFramework.Plus;

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
                int leadcount = GetCRM_BatchLead(data).Count();

                if (ValidateClass.Authenticated(token, tk_agentId))
                {
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




    }
}
