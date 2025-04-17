using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace marvel_campaign_NET8.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;

        public FilterController(ScrmDbContext context)
        {
            _scrme = context;
        }



        // Get CallFilter
        [Route("GetCallFilter")]
        [HttpPost]
        public IActionResult GetCallFilter([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = GetCRM_CallFilter(data) });
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

        private List<call_filter> GetCRM_CallFilter(JsonObject data)
        {
            int filterId = Convert.ToInt32((data["Filter_Id"] ?? "-1").ToString());
            string allPhoneNo = (data["All_Phone_No"] ?? "").ToString();
            string filterType = (data["Filter_Type"] ?? "").ToString();

            // obtain data from table "call_filter"
            var _filter = (from _c in _scrme.call_filters
                           select _c);

            if (filterId != -1)
            {
                _filter = _filter.Where(_c => _c.Filter_Id == filterId && _c.Is_Valid == "Y");
            }

            if (allPhoneNo != string.Empty)
            {
                _filter = _filter.Where(_c => _c.Mobile_No == allPhoneNo || _c.Other_Phone_No == allPhoneNo);
                _filter = _filter.Where(_c => _c.Is_Valid == "Y");
            }

            if (filterType != string.Empty)
            {
                _filter = _filter.Where(_c => _c.Filter_Type == filterType && _c.Is_Valid == "Y");
            }

            return _filter.ToList();

        }






    }
}
