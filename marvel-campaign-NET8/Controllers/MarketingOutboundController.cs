using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

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





    }
}
