using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
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


        // Add CallFilter
        [Route("AddCallFilter")]
        [HttpPost]
        public IActionResult AddCallFilter([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = AddCRM_CallFilter(data) });
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

        private List<call_filter> AddCRM_CallFilter(JsonObject data)
        {
            // declare db table items
            call_filter _filter_item = new call_filter();

            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());
            string filterType = (data["Filter_Type"] ?? "").ToString();
            string firstName = (data["First_Name"] ?? "").ToString();
            string lastName = (data["Last_Name"] ?? "").ToString();
            string title = (data["Title"] ?? "").ToString();
            string mobileNo = (data["Mobile_No"] ?? "").ToString();
            string otherPhoneNo = (data["Other_Phone_No"] ?? "").ToString();
            string email = (data["Email"] ?? "").ToString();
            string addressLine = (data["Address_Line"] ?? "").ToString();
            string remark = (data["Remark"] ?? "").ToString();

            // assign new call filter record
            _filter_item.Filter_Type = filterType;
            _filter_item.First_Name = firstName;
            _filter_item.Last_Name = lastName;
            _filter_item.Title = title;
            _filter_item.Mobile_No = mobileNo;
            _filter_item.Other_Phone_No = otherPhoneNo;
            _filter_item.Email = email;
            _filter_item.Address_Line = addressLine;
            _filter_item.Remark = remark;
            _filter_item.Created_By = agentId;
            _filter_item.Created_Time = DateTime.Now;
            _filter_item.Updated_By = agentId;
            _filter_item.Updated_Time = DateTime.Now;
            _filter_item.Is_Valid = "Y";

            // add new call filter record
            _scrme.call_filters.Add(_filter_item);

            // save db changes
            _scrme.SaveChanges();

            // obtain the new case from table "call filter"
            List<call_filter> _call_filter = (from _c in _scrme.call_filters
                                                        where _c.Filter_Id == _filter_item.Filter_Id
                                                        select _c).ToList();

            return _call_filter;
        }


        // Update CallFilter
        [Route("UpdateCallFilter")]
        [HttpPut]
        public IActionResult UpdateCallFilter([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    UpdateCRM_CallFilter(data);
                    return Ok(new { result = AppOutp.OutputResult_SUCC, details = "updated call filter" });
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

        private void UpdateCRM_CallFilter(JsonObject data)
        {
            int filterId = Convert.ToInt32((data["Filter_Id"] ?? "-1").ToString());
            int agentId = Convert.ToInt32((data["Agent_Id"] ?? "-1").ToString());


            // declare a dictionary object where key = fieldName, value = fieldValue 
            Dictionary<string, string> fieldsToBeAdded = new Dictionary<string, string>();

            // iterate through the form data and assign the field name and field value to dictionary
            foreach (var item in data)
            {
                string fieldName = item.Key;
                string fieldValue = item.Value?.ToString() ?? string.Empty;

                if (fieldName != "Agent_Id" && fieldName != "Filter_Id" && fieldName != "Token")
                {
                    fieldsToBeAdded.Add(fieldName, fieldValue); // add field items to dictionary
                }
            }

            // obtain the call filter record based on the filter id
            var _call = (from _c in _scrme.call_filters
                         where _c.Filter_Id == filterId
                         select _c).SingleOrDefault<call_filter>();

            // call filter exists in table
            if (_call != null)
            {
                _call.Updated_By = agentId;
                _call.Updated_Time = DateTime.Now;

                // iterate through the dictionary and update the fields
                foreach (var fields in fieldsToBeAdded)
                {
                    // find the column name that matches with the field name in dictionary
                    PropertyInfo? properInfo = _call.GetType().GetProperty(fields.Key);
                    properInfo?.SetValue(_call, (fields.Value == null ? string.Empty : fields.Value));
                }

                _scrme.SaveChanges(); // save changes to db
            }
        }






    }
}
