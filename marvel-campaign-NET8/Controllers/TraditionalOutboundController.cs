using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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





    }
}
