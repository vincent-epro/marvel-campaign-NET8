using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Generic method to handle authentication, validation, and database query
        private async Task<IActionResult> ExecuteAuthenticatedQuery<T>(
            JsonObject data,
            Func<JsonObject, bool> validateInput,
            Func<Task<List<T>>> executeQuery,
            Func<List<T>, object> transformResult = null)
        {
            try
            {
                string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
                string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

                if (!ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.Not_Auth_Desc });
                }

                if (!validateInput(data))
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.OutputDetails_Inv_Para });
                }

                var result = await executeQuery();
                var response = transformResult != null ? transformResult(result) : result;
                return Ok(new { result = AppOutp.OutputResult_SUCC, details = response });
            }
            catch (Exception err)
            {
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = err.Message });
            }
        }

        // Get Customer Journey
        [Route("GetCustomerJourney")]
        [HttpPost]
        public async Task<IActionResult> GetCustomerJourney([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => Convert.ToInt32((d["Customer_Id"] ?? "-1").ToString()) != -1,
                async () =>
                {
                    int customerId = Convert.ToInt32((data["Customer_Id"] ?? "-1").ToString());
                    return await _scrme_sp.CustomerJourney_sp
                        .FromSqlRaw("EXEC getCustomerJourney @customerId", new SqlParameter("@customerId", customerId))
                        .ToListAsync();
                });
        }

        // Get Dashboard CallNature
        [Route("GetDashboard_CallNature")]
        [HttpPost]
        public async Task<IActionResult> GetDashboard_CallNature([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => !string.IsNullOrEmpty((d["Call_Nature"] ?? "").ToString()),
                async () =>
                {
                    string callNature = (data["Call_Nature"] ?? "").ToString();
                    return await _scrme_sp.Dashboard_CallNature_sp
                        .FromSqlRaw("EXEC getDashboard_CallNature @callNature", new SqlParameter("@callNature", callNature))
                        .ToListAsync();
                },
                result => result.Select(x => x.Description).ToList());
        }

        // Get Dashboard Agent CallNature
        [Route("GetDashboard_Agent_CallNature")]
        [HttpPost]
        public async Task<IActionResult> GetDashboard_Agent_CallNature([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => {
                    string dateRange = (d["Date_Range"] ?? "").ToString();
                    return dateRange == "Today" || dateRange == "Last7" || dateRange == "Last14" || dateRange == "Last30";
                },
                async () =>
                {
                    string dateRange = (data["Date_Range"] ?? "").ToString();
                    return await _scrme_sp.Dashboard_Agent_CallNature_sp
                        .FromSqlRaw("EXEC getDashboard_Agent_CallNature @dateRange", new SqlParameter("@dateRange", dateRange))
                        .ToListAsync();
                });
        }

        // Get Outbound Batch Assignment
        [Route("GetOutboundBatchAssignment")]
        [HttpPost]
        public async Task<IActionResult> GetOutboundBatchAssignment([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => Convert.ToInt32((d[AppInp.Input_Batch_Id] ?? "-1").ToString()) != -1,
                async () =>
                {
                    int batchNo = Convert.ToInt32((data[AppInp.Input_Batch_Id] ?? "-1").ToString());
                    return await _scrme_sp.OutboundBatchAssignment_sp
                        .FromSqlRaw("EXEC getOutboundBatchAssignment @batchNo", new SqlParameter("@batchNo", batchNo))
                        .ToListAsync();
                });
        }

        // Get Outbound Batch Assignment Agent
        [Route("GetOutboundBatchAssignment_Agent")]
        [HttpPost]
        public async Task<IActionResult> GetOutboundBatchAssignment_Agent([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => Convert.ToInt32((d[AppInp.Input_Batch_Id] ?? "-1").ToString()) != -1,
                async () =>
                {
                    int batchNo = Convert.ToInt32((data[AppInp.Input_Batch_Id] ?? "-1").ToString());
                    return await _scrme_sp.OutboundBatchAssignment_Agent_sp
                        .FromSqlRaw("EXEC getOutboundBatchAssignment_Agent @batchNo", new SqlParameter("@batchNo", batchNo))
                        .ToListAsync();
                });
        }

        // Get OB Batch Assignment
        [Route("GetOBBatchAssignment")]
        [HttpPost]
        public async Task<IActionResult> GetOBBatchAssignment([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => !string.IsNullOrEmpty((d[AppInp.Input_Batch_Code] ?? "").ToString()) &&
                     !string.IsNullOrEmpty((d[AppInp.Input_Campaign_Code] ?? "").ToString()),
                async () =>
                {
                    string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();
                    return await _scrme_sp.OBBatchAssignment_sp
                        .FromSqlRaw("EXEC getOBBatchAssignment @batchcode, @campaigncode",
                            new SqlParameter("@batchcode", batchcode),
                            new SqlParameter("@campaigncode", campaigncode))
                        .ToListAsync();
                });
        }

        // Get OB Batch Assignment Agent
        [Route("GetOBBatchAssignment_Agent")]
        [HttpPost]
        public async Task<IActionResult> GetOBBatchAssignment_Agent([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => !string.IsNullOrEmpty((d[AppInp.Input_Batch_Code] ?? "").ToString()) &&
                     !string.IsNullOrEmpty((d[AppInp.Input_Campaign_Code] ?? "").ToString()),
                async () =>
                {
                    string batchcode = (data[AppInp.Input_Batch_Code] ?? "").ToString();
                    string campaigncode = (data[AppInp.Input_Campaign_Code] ?? "").ToString();
                    return await _scrme_sp.OBBatchAssignment_Agent_sp
                        .FromSqlRaw("EXEC getOBBatchAssignment_Agent @batchcode, @campaigncode",
                            new SqlParameter("@batchcode", batchcode),
                            new SqlParameter("@campaigncode", campaigncode))
                        .ToListAsync();
                });
        }

        // Get OB Product Price
        [Route("GetOBProductPrice")]
        [HttpPost]
        public async Task<IActionResult> GetOBProductPrice([FromBody] JsonObject data)
        {
            return await ExecuteAuthenticatedQuery(
                data,
                d => true, // No specific input validation needed
                async () =>
                {
                    return await _scrme_sp.OBProductPrice_sp
                        .FromSqlRaw("EXEC getOBProductPrice")
                        .ToListAsync();
                },
                result => result.Select(x => x.Product_Price).ToList());
        }
    }
}