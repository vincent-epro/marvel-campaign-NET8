using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace marvel_campaign_NET8.Controllers
{
    [Route("api")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ScrmDbContext _scrme;
        public SearchController(ScrmDbContext context)
        {
            _scrme = context;
        }

        // Case Manual Search
        [Route("CaseManualSearch")]
        [HttpPost]
        public IActionResult CaseManualSearch([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();
            //return Content(GetCaseResult(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8); //
            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {

                    return Content(GetCaseResult(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
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

        private JObject GetCaseResult(JsonObject data)
        {
            string anyAll = (data["anyAll"] ?? "").ToString();
            JsonArray? searchArray = (JsonArray?)data["searchArr"];
            string isCurrent = (data["Is_Current"] ?? "").ToString();
            string IsValid = (data["Is_Valid"] ?? "all").ToString();
            string countOnly = (data["Count_Only"] ?? "").ToString();
            var query_r = _scrme.case_results.Select(r => r);
            var query_rl = _scrme.case_result_logs.Select(r => r);
            var query_c = _scrme.contact_lists.Select(r => r);


            if (IsValid != "all")
            {
                query_r = query_r.Where(r => r.Is_Valid == IsValid);
                query_rl = query_rl.Where(r => r.Is_Valid == IsValid);
                query_c = query_c.Where(r => r.Is_Valid == IsValid);
            }
            StringBuilder where_a = new();
            List<Object> params_a = [];


            foreach (var searchObj in searchArray)
            {
                string field_name = (searchObj["field_name"] ?? "").ToString();
                string logic_operator = (searchObj["logic_operator"] ?? "").ToString();
                string field_type = (searchObj["field_type"] ?? "").ToString();
                string list_name = (searchObj["list_name"] ?? "").ToString();

                string field_value = String.Empty;
                int field_value_num = 0;
                switch (field_type)
                {
                    case "number":
                        field_value_num = Convert.ToInt32(searchObj["value"].ToString());
                        break;
                    default:
                        field_value = searchObj["value"].ToString();
                        break;
                }
                string cond_a = string.Empty;
                switch (field_name)
                {
                    case "All_Phone_No":
                        switch (logic_operator)
                        {
                            case "is" or "=":
                                params_a.Add(field_value);
                                cond_a += "(";
                                cond_a += $"c.Home_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Office_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Mobile_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Other_Phone_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Fax_No=@{params_a.Count - 1}";
                                cond_a += $" Or r.Type_Details=@{params_a.Count - 1}";
                                cond_a += ")";
                                break;
                            case "is not" or "!=":
                                params_a.Add(field_value);
                                cond_a += "!(";
                                cond_a += $"c.Home_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Office_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Mobile_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Other_Phone_No=@{params_a.Count - 1}";
                                cond_a += $" Or c.Fax_No=@{params_a.Count - 1}";
                                cond_a += $" Or r.Type_Details=@{params_a.Count - 1}";
                                cond_a += ")";
                                break;
                            case "contains":
                                params_a.Add(field_value);
                                cond_a += "(";
                                cond_a += $"c.Home_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Office_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Mobile_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Other_Phone_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Fax_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or r.Type_Details.Contains(@{params_a.Count - 1})";
                                cond_a += ")";
                                break;
                            case "not contains":
                                params_a.Add(field_value);
                                cond_a += "!(";
                                cond_a += $"c.Home_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Office_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Mobile_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Other_Phone_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or c.Fax_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or r.Type_Details.Contains(@{params_a.Count - 1})";
                                cond_a += ")";
                                break;

                        }
                        break;
                    case "Email":
                        switch (logic_operator)
                        {
                            case "is" or "=":
                                params_a.Add(field_value);
                                cond_a += "(";
                                cond_a += $"c.Email=@{params_a.Count - 1}";
                                cond_a += $" Or r.Type_Details=@{params_a.Count - 1}";
                                cond_a += ")";
                                break;
                            case "is not" or "!=":
                                params_a.Add(field_value);
                                cond_a += "!(";
                                cond_a += $"c.Email=@{params_a.Count - 1}";
                                cond_a += $" Or r.Type_Details=@{params_a.Count - 1}";
                                cond_a += ")";
                                break;
                            case "contains":
                                params_a.Add(field_value);
                                cond_a += "(";
                                cond_a += $"c.Email.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or r.Type_Details.Contains(@{params_a.Count - 1})";
                                cond_a += ")";
                                break;
                            case "not contains":
                                params_a.Add(field_value);
                                cond_a += "!(";
                                cond_a += $"c.Email.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or r.Type_Details.Contains(@{params_a.Count - 1})";
                                cond_a += ")";
                                break;
                        }
                        break;
                    default:
                        string alias = (list_name == "Contact List") ? "c" : "r";
                        switch (field_type)
                        {
                            case "datetime":
                                DateTime _d1 = Convert.ToDateTime(field_value);
                                DateTime _d2 = _d1.AddDays(1);

                                switch (logic_operator)
                                {
                                    case "is" or "=":
                                        params_a.Add(_d1);
                                        cond_a += "(";
                                        cond_a += $"{alias}.{field_name}>=@{params_a.Count - 1}";
                                        params_a.Add(_d2);
                                        cond_a += $" And {alias}.{field_name}<@{params_a.Count - 1}";
                                        cond_a += ")";
                                        break;
                                    case "is not" or "!=":
                                        params_a.Add(_d1);
                                        cond_a += "(";
                                        cond_a += $"{alias}.{field_name}<@{params_a.Count - 1}";
                                        params_a.Add(_d2);
                                        cond_a += $" Or {alias}.{field_name}>=@{params_a.Count - 1}";
                                        cond_a += ")";
                                        break;
                                    case ">" or "<=":
                                        DateTime _d1x = Convert.ToDateTime(field_value + " 23:59:59");
                                        params_a.Add(_d1x);
                                        cond_a += $"{alias}.{field_name}{logic_operator}@{params_a.Count - 1}";
                                        break;
                                    case ">=" or "<":
                                        params_a.Add(_d1);
                                        cond_a += $"{alias}.{field_name}{logic_operator}@{params_a.Count - 1}";
                                        break;

                                }
                                break;
                            case "string" or "null" or "":
                                switch (logic_operator)
                                {
                                    case "is" or "=":
                                        params_a.Add(field_value);
                                        cond_a += $"{alias}.{field_name}=@{params_a.Count - 1}";
                                        break;
                                    case "is not" or "!=":
                                        params_a.Add(field_value);
                                        cond_a += $"{alias}.{field_name}!=@{params_a.Count - 1}";
                                        break;
                                    case "contains":
                                        params_a.Add(field_value);
                                        cond_a += $"{alias}.{field_name}.Contains(@{params_a.Count - 1})";
                                        break;
                                    case "not contains":
                                        params_a.Add(field_value);
                                        cond_a += $"!{alias}.{field_name}.Contains(@{params_a.Count - 1})";
                                        break;
                                }
                                break;
                            default:
                                switch (logic_operator)
                                {
                                    case "is":
                                        logic_operator = "=";
                                        break;
                                    case "is not":
                                        logic_operator = "!=";
                                        break;
                                }
                                if (field_type == "number")
                                    params_a.Add(field_value_num);
                                else
                                    params_a.Add(field_value);
                                cond_a += $"{alias}.{field_name} {logic_operator}@{params_a.Count - 1}";
                                break;
                        }
                        break;
                }
                if (cond_a != string.Empty)
                {
                    if (where_a.Length == 0)
                    {
                        where_a.Append(cond_a);
                    }
                    else if (anyAll == "any")
                    {
                        where_a.Append(" or " + cond_a);
                    }
                    else if (anyAll == "all")
                    {
                        where_a.Append(" and " + cond_a);
                    }
                }

            }
            IQueryable<int> _q;
            if (isCurrent != "Y")
            {
                _q = (from r in query_rl
                      join c in query_c on r.Customer_Id equals c.Customer_Id
                      select new { c, r }
                      ).Where(where_a.ToString(), [.. params_a]).Select(q => q.r.Internal_Case_No ?? 0).Distinct();
            }
            else
            {
                _q = (from r in query_r
                      join c in query_c on r.Customer_Id equals c.Customer_Id
                      select new { c, r }
                      ).Where(where_a.ToString(), [.. params_a]).Select(q => q.r.Internal_Case_No).Distinct();
            }

            List<JObject> jsonResultList = [];

            if (countOnly == "Y")
            {
                return new JObject() {
                    new JProperty("result", AppOutp.OutputResult_SUCC),
                    new JProperty("details", _q.Count())
                };
            }
            else if (_q.Count() > 0)
            {
                List<int> _list = _q.ToList<int>();
                var _search_results = (from _case1 in _scrme.case_results
                                       join _cust in _scrme.contact_lists
                                       on _case1.Customer_Id equals _cust.Customer_Id
                                       where _list.Contains(_case1.Internal_Case_No)
                                       orderby _case1.Updated_Time descending
                                       select new { _cust, _case1 }).Take(100).ToList();

                foreach (var _result_item in _search_results)
                {
                    JObject tempJson = JObject.FromObject(_result_item._cust);
                    tempJson.Remove("Photo");
                    JObject caseJson = JObject.FromObject(_result_item._case1);
                    caseJson.Remove("Customer_Id");
                    caseJson.Property("Opened_By")?.Replace(new JProperty("Case_Opened_By", caseJson.Property("Opened_By")?.Value));
                    caseJson.Property("Opened_Time")?.Replace(new JProperty("Case_Opened_Time", caseJson.Property("Opened_Time")?.Value));
                    caseJson.Property("Created_By")?.Replace(new JProperty("Case_Created_By", caseJson.Property("Created_By")?.Value));
                    caseJson.Property("Created_Time")?.Replace(new JProperty("Case_Created_Time", caseJson.Property("Created_Time")?.Value));
                    caseJson.Property("Updated_By")?.Replace(new JProperty("Case_Updated_By", caseJson.Property("Updated_By")?.Value));
                    caseJson.Property("Updated_Time")?.Replace(new JProperty("Case_Updated_Time", caseJson.Property("Updated_Time")?.Value));
                    caseJson.Property("Is_Valid")?.Replace(new JProperty("Case_Is_Valid", caseJson.Property("Is_Valid")?.Value));
                    tempJson.Merge(caseJson);

                    jsonResultList.Add(tempJson); // add the temp result to the list    
                }


            }
            return new JObject() {
                new JProperty("result", AppOutp.OutputResult_SUCC),
                new JProperty("details", jsonResultList)
            };

        }


        // Manual Search
        [Route("ManualSearch")]
        [HttpPost]
        public IActionResult ManualSearch([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();
            //return Content(GetCustomerResult(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8); //
            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {

                    return Content(GetCustomerResult(data).ToString(), "application/json; charset=utf-8", Encoding.UTF8);
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

        private JObject GetCustomerResult(JsonObject data)
        {
            string anyAll = (data["anyAll"] ?? "").ToString();
            JsonArray searchArray = (JsonArray?)data["searchArr"] ?? new JsonArray();
            string IsValid = (data["Is_Valid"] ?? "all").ToString();
            string takeAll = (data["Take_All"] ?? "").ToString();

            var query_c = _scrme.contact_lists.Select(r => r);

            if (IsValid != "all")
            {
                query_c = query_c.Where(r => r.Is_Valid == IsValid);
            }

            StringBuilder where_a = new();
            List<Object> params_a = new() { };

            foreach (var searchObj in searchArray)
            {
                string field_name = (searchObj["field_name"] ?? "").ToString();
                string logic_operator = (searchObj["logic_operator"] ?? "").ToString();
                string field_type = (searchObj["field_type"] ?? "").ToString();

                string field_value = String.Empty;
                int field_value_num = 0;

                switch (field_type)
                {
                    case "number":
                        field_value_num = Convert.ToInt32(searchObj["value"].ToString());
                        break;
                    default:
                        field_value = searchObj["value"].ToString();
                        break;
                }
                string cond_a = string.Empty;

                switch (field_name)
                {
                    case "All_Phone_No":
                        switch (logic_operator)
                        {
                            case "is" or "=":
                                params_a.Add(field_value);
                                cond_a += "(";
                                cond_a += $"Home_No=@{params_a.Count - 1}";
                                cond_a += $" Or Office_No=@{params_a.Count - 1}";
                                cond_a += $" Or Mobile_No=@{params_a.Count - 1}";
                                cond_a += $" Or Other_Phone_No=@{params_a.Count - 1}";
                                cond_a += $" Or Fax_No=@{params_a.Count - 1}";
                                cond_a += ")";
                                break;
                            case "is not" or "!=":
                                params_a.Add(field_value);
                                cond_a += "!(";
                                cond_a += $"Home_No=@{params_a.Count - 1}";
                                cond_a += $" Or Office_No=@{params_a.Count - 1}";
                                cond_a += $" Or Mobile_No=@{params_a.Count - 1}";
                                cond_a += $" Or Other_Phone_No=@{params_a.Count - 1}";
                                cond_a += $" Or Fax_No=@{params_a.Count - 1}";
                                cond_a += ")";
                                break;
                            case "contains":
                                params_a.Add(field_value);
                                cond_a += "(";
                                cond_a += $"Home_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Office_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Mobile_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Other_Phone_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Fax_No.Contains(@{params_a.Count - 1})";
                                cond_a += ")";
                                break;
                            case "not contains":
                                params_a.Add(field_value);
                                cond_a += "!(";
                                cond_a += $"Home_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Office_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Mobile_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Other_Phone_No.Contains(@{params_a.Count - 1})";
                                cond_a += $" Or Fax_No.Contains(@{params_a.Count - 1})";
                                cond_a += ")";
                                break;

                        }
                        break;
                    default:
                        switch (field_type)
                        {
                            case "datetime":
                                DateTime _d1 = Convert.ToDateTime(field_value);
                                DateTime _d2 = _d1.AddDays(1);

                                switch (logic_operator)
                                {
                                    case "is" or "=":
                                        params_a.Add(_d1);
                                        cond_a += "(";
                                        cond_a += $"{field_name}>=@{params_a.Count - 1}";
                                        params_a.Add(_d2);
                                        cond_a += $" And {field_name}<@{params_a.Count - 1}";
                                        cond_a += ")";
                                        break;
                                    case "is not" or "!=":
                                        params_a.Add(_d1);
                                        cond_a += "(";
                                        cond_a += $"{field_name}<@{params_a.Count - 1}";
                                        params_a.Add(_d2);
                                        cond_a += $" Or {field_name}>=@{params_a.Count - 1}";
                                        cond_a += ")";
                                        break;
                                    case ">" or "<=":
                                        DateTime _d1x = Convert.ToDateTime(field_value + " 23:59:59");
                                        params_a.Add(_d1x);
                                        cond_a += $"{field_name}{logic_operator}@{params_a.Count - 1}";
                                        break;
                                    case ">=" or "<":
                                        params_a.Add(_d1);
                                        cond_a += $"{field_name}{logic_operator}@{params_a.Count - 1}";
                                        break;

                                }
                                break;
                            case "string" or "null" or "":
                                switch (logic_operator)
                                {
                                    case "is" or "=":
                                        params_a.Add(field_value);
                                        cond_a += $"{field_name}=@{params_a.Count - 1}";
                                        break;
                                    case "is not" or "!=":
                                        params_a.Add(field_value);
                                        cond_a += $"{field_name}!=@{params_a.Count - 1}";
                                        break;
                                    case "contains":
                                        params_a.Add(field_value);
                                        cond_a += $"{field_name}.Contains(@{params_a.Count - 1})";
                                        break;
                                    case "not contains":
                                        params_a.Add(field_value);
                                        cond_a += $"!{field_name}.Contains(@{params_a.Count - 1})";
                                        break;
                                }
                                break;
                            default:
                                switch (logic_operator)
                                {
                                    case "is":
                                        logic_operator = "=";
                                        break;
                                    case "is not":
                                        logic_operator = "!=";
                                        break;
                                }
                                if (field_type == "number")
                                    params_a.Add(field_value_num);
                                else
                                    params_a.Add(field_value);
                                cond_a += $"{field_name} {logic_operator}@{params_a.Count - 1}";
                                break;
                        }
                        break;
                }
                if (cond_a != string.Empty)
                {
                    if (where_a.Length == 0)
                    {
                        where_a.Append(cond_a);
                    }
                    else if (anyAll == "any")
                    {
                        where_a.Append(" or " + cond_a);
                    }
                    else if (anyAll == "all")
                    {
                        where_a.Append(" and " + cond_a);
                    }
                }
            }


            List<contact_list> searchResult;
            if (takeAll == "Y")
                searchResult = query_c.Where(where_a.ToString(), [.. params_a]).ToList();
            else
                searchResult = query_c.Where(where_a.ToString(), [.. params_a]).OrderByDescending(_s => _s.Updated_Time).Take(100).ToList();

            JArray jsonResultList = [];


            foreach (contact_list contactList in searchResult)
            {

                JObject tempJson = JObject.FromObject(contactList);
                tempJson.Remove("Photo");

                if (takeAll != "Y")
                { 
                    bool hasCase = _scrme.case_results.Where(r => r.Customer_Id == contactList.Customer_Id && r.Is_Valid == "Y").Any();
                    tempJson.AddFirst(new JProperty("have_case", hasCase));
                }

                jsonResultList.Add(tempJson);
            }

            return new JObject() {
                new JProperty("result", AppOutp.OutputResult_SUCC),
                new JProperty("details", jsonResultList)
            };
        }


    }

}