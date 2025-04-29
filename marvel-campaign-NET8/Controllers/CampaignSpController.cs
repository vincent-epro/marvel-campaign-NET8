using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace marvel_campaign_NET8.Controllers
{
    [Route("api")]
    [ApiController]
    public class CampaignSpController : ControllerBase
    {
        private readonly Scrm_SP_DbContext _scrme_sp;

        public CampaignSpController(Scrm_SP_DbContext context_sp)
        {
            _scrme_sp = context_sp;

        }


        // Get Customer Journey
        [Route("GetCustomerJourney")]
        [HttpPost]
        public async Task<IActionResult> GetCustomerJourney([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    int customerId = Convert.ToInt32((data["Customer_Id"] ?? "-1").ToString());

                    if (customerId == -1)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        var cj = await _scrme_sp.CustomerJourney_sp
                            .FromSqlRaw("EXEC getCustomerJourney @customerId", new SqlParameter("@customerId", customerId))
                            .ToListAsync();

                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = cj });
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


        // Get Dashboard CallNature
        [Route("GetDashboard_CallNature")]
        [HttpPost]
        public async Task<IActionResult> GetDashboard_CallNature([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string callNature = (data["Call_Nature"] ?? "").ToString();

                    if (callNature == string.Empty)
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
                    }
                    else
                    {
                        var dc = await _scrme_sp.Dashboard_CallNature_sp
                            .FromSqlRaw("EXEC getDashboard_CallNature @callNature", new SqlParameter("@callNature", callNature))
                            .ToListAsync(); // Ensuring async execution

                        var descriptions = dc.Select(x => x.Description).ToList();

                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = descriptions });
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


        // Get Dashboard Agent CallNature
        [Route("GetDashboard_Agent_CallNature")]
        [HttpPost]
        public async Task<IActionResult> GetDashboard_Agent_CallNature([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string dateRange = (data["Date_Range"] ?? "").ToString();

                    if (dateRange == "Today" || dateRange == "Last7" || dateRange == "Last14" || dateRange == "Last30")
                    {
                        var da = await _scrme_sp.Dashboard_Agent_CallNature_sp
                            .FromSqlRaw("EXEC getDashboard_Agent_CallNature @dateRange", new SqlParameter("@dateRange", dateRange))
                            .ToListAsync();

                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = da });
                    }
                    else
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "Invalid Parameters." });
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


        // Get Outbound Batch Assignment
        [Route("GetOutboundBatchAssignment")]
        [HttpPost]
        public async Task<IActionResult> GetOutboundBatchAssignment([FromBody] JsonObject data)
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
                        var res = await _scrme_sp.OutboundBatchAssignment_sp
                            .FromSqlRaw("EXEC getOutboundBatchAssignment @batchNo", new SqlParameter("@batchNo", batchNo))
                            .ToListAsync();

                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = res });
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


        // Get Outbound Batch Assignment Agent
        [Route("GetOutboundBatchAssignment_Agent")]
        [HttpPost]
        public async Task<IActionResult> GetOutboundBatchAssignment_Agent([FromBody] JsonObject data)
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
                        var res = await _scrme_sp.OutboundBatchAssignment_Agent_sp
                            .FromSqlRaw("EXEC getOutboundBatchAssignment_Agent @batchNo", new SqlParameter("@batchNo", batchNo))
                            .ToListAsync();

                        return Ok(new { result = AppOutp.OutputResult_SUCC, details = res });
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





    }
}
