using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
            return ProcessSearch(data, SearchType.Case);
        }

        // Manual Search
        [Route("ManualSearch")]
        [HttpPost]
        public IActionResult ManualSearch([FromBody] JsonObject data)
        {
            return ProcessSearch(data, SearchType.Customer);
        }

        private enum SearchType { Case, Customer }

        private IActionResult ProcessSearch(JsonObject data, SearchType searchType)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (!ValidateClass.Authenticated(token, tk_agentId))
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = AppOutp.Not_Auth_Desc });
                }

                JObject result = searchType == SearchType.Case
                    ? GetCaseResult(data)
                    : GetCustomerResult(data);

                return Content(result.ToString(), "application/json; charset=utf-8", Encoding.UTF8);
            }
            catch (Exception err)
            {
                return Ok(new { result = AppOutp.OutputResult_FAIL, details = err.Message });
            }
        }

        private JObject GetCaseResult(JsonObject data)
        {
            string anyAll = (data["anyAll"] ?? "").ToString();
            JsonArray searchArray = (JsonArray?)data["searchArr"] ?? new JsonArray();
            string isCurrent = (data["Is_Current"] ?? "").ToString();
            string isValid = (data[AppInp.Input_Is_Valid] ?? "all").ToString();
            string countOnly = (data["Count_Only"] ?? "").ToString();

            var query_r = _scrme.case_results.AsQueryable();
            var query_rl = _scrme.case_result_logs.AsQueryable();
            var query_c = _scrme.contact_lists.AsQueryable();

            if (isValid != "all")
            {
                query_r = query_r.Where(r => r.Is_Valid == isValid);
                query_rl = query_rl.Where(r => r.Is_Valid == isValid);
                query_c = query_c.Where(r => r.Is_Valid == isValid);
            }

            var (whereClause, parameters) = BuildSearchConditions(searchArray, anyAll, isCaseSearch: true);

            IQueryable<int> caseQuery = isCurrent != "Y"
                ? (from r in query_rl
                   join c in query_c on r.Customer_Id equals c.Customer_Id
                   select new { c, r })
                  .Where(whereClause, parameters.ToArray())
                  .Select(q => q.r.Internal_Case_No ?? 0)
                  .Distinct()
                : (from r in query_r
                   join c in query_c on r.Customer_Id equals c.Customer_Id
                   select new { c, r })
                  .Where(whereClause, parameters.ToArray())
                  .Select(q => q.r.Internal_Case_No)
                  .Distinct();

            if (countOnly == "Y")
            {
                return new JObject
                {
                    { AppOutp.OutputResult_Field, AppOutp.OutputResult_SUCC },
                    { AppOutp.OutputDetails_Field, caseQuery.Count() }
                };
            }

            List<JObject> jsonResultList = new();
            if (caseQuery.Any())
            {
                var caseNumbers = caseQuery.ToList();
                var searchResults = (from case1 in _scrme.case_results
                                     join cust in _scrme.contact_lists
                                     on case1.Customer_Id equals cust.Customer_Id
                                     where caseNumbers.Contains(case1.Internal_Case_No)
                                     orderby case1.Updated_Time descending
                                     select new { cust, case1 })
                                    .Take(100)
                                    .ToList();

                foreach (var result in searchResults)
                {
                    JObject tempJson = JObject.FromObject(result.cust);
                    tempJson.Remove("Photo");
                    JObject caseJson = JObject.FromObject(result.case1);
                    caseJson.Remove("Customer_Id");
                    RenameCaseProperties(caseJson);
                    tempJson.Merge(caseJson);
                    jsonResultList.Add(tempJson);
                }
            }

            return new JObject
            {
                { AppOutp.OutputResult_Field, AppOutp.OutputResult_SUCC },
                { AppOutp.OutputDetails_Field, JArray.FromObject(jsonResultList) }
            };
        }

        private JObject GetCustomerResult(JsonObject data)
        {
            string anyAll = (data["anyAll"] ?? "all").ToString();
            JsonArray searchArray = (JsonArray?)data["searchArr"] ?? new JsonArray();
            string isValid = (data[AppInp.Input_Is_Valid] ?? "all").ToString();
            string takeAll = (data["Take_All"] ?? "").ToString();

            var query_c = _scrme.contact_lists.AsQueryable();
            if (isValid != "all")
            {
                query_c = query_c.Where(r => r.Is_Valid == isValid);
            }

            var (whereClause, parameters) = BuildSearchConditions(searchArray, anyAll, isCaseSearch: false);

            var searchResult = takeAll == "Y"
                ? query_c.Where(whereClause, parameters.ToArray()).ToList()
                : query_c.Where(whereClause, parameters.ToArray()).OrderByDescending(s => s.Updated_Time).Take(100).ToList();

            JArray jsonResultList = new();
            foreach (var contact in searchResult)
            {
                JObject tempJson = JObject.FromObject(contact);
                tempJson.Remove("Photo");
                if (takeAll != "Y")
                {
                    bool hasCase = _scrme.case_results.Any(r => r.Customer_Id == contact.Customer_Id && r.Is_Valid == "Y");
                    tempJson.AddFirst(new JProperty("have_case", hasCase));
                }
                jsonResultList.Add(tempJson);
            }

            return new JObject
            {
                { AppOutp.OutputResult_Field, AppOutp.OutputResult_SUCC },
                { AppOutp.OutputDetails_Field, jsonResultList }
            };
        }

        private static (string WhereClause, List<object> Parameters) BuildSearchConditions(JsonArray searchArray, string anyAll, bool isCaseSearch)
        {
            List<string> conditions = new();
            List<object> parameters = new();

            foreach (var searchObj in searchArray)
            {
                string condition = string.Empty;
                SetCondition(searchObj, ref parameters, ref condition, isCaseSearch);
                if (!string.IsNullOrEmpty(condition))
                {
                    conditions.Add(condition);
                }
            }

            string whereClause = anyAll == "any"
                ? string.Join(" Or ", conditions)
                : string.Join(" And ", conditions);

            return (whereClause, parameters);
        }

        private static void SetCondition(JsonNode? searchObj, ref List<object> parameters, ref string condition, bool isCaseSearch)
        {
            string fieldName = (searchObj[AppInp.Input_SearchArr_field_name] ?? "").ToString();
            string logicOperator = (searchObj[AppInp.Input_SearchArr_logic_operator] ?? "").ToString();
            string fieldType = (searchObj["field_type"] ?? "").ToString();
            string fieldValue = searchObj[AppInp.Input_SearchArr_value]?.ToString() ?? "";
            string listName = (searchObj["list_name"] ?? "").ToString();

            // Normalize operator
            logicOperator = logicOperator switch
            {
                "is" => "=",
                AppInp.Input_Search_Operator_is_not => "!=",
                _ => logicOperator
            };

            // Handle multi-field searches (All_Phone_No, Email)
            if (fieldName == "All_Phone_No")
            {
                var fields = isCaseSearch
                    ? new[] { "c.Home_No", "c.Office_No", "c.Mobile_No", "c.Other_Phone_No", "c.Fax_No", "r.Type_Details" }
                    : new[] { "Home_No", "Office_No", "Mobile_No", "Other_Phone_No", "Fax_No" };
                condition = BuildMultiFieldCondition(fields, logicOperator, fieldValue, ref parameters);
                return;
            }
            else if (fieldName == "Email" && isCaseSearch)
            {
                condition = BuildMultiFieldCondition(new[] { "c.Email", "r.Type_Details" }, logicOperator, fieldValue, ref parameters);
                return;
            }

            // Determine alias based on search type and list name
            string alias = isCaseSearch && listName != "Contact List" ? "r" : "c";
            if (!isCaseSearch) alias = ""; // No alias for customer search

            // Handle single-field conditions
            switch (fieldType)
            {
                case "datetime":
                    condition = BuildDateTimeCondition(fieldName, logicOperator, fieldValue, alias, ref parameters);
                    break;
                case "string" or "null" or "":
                    condition = BuildStringCondition(fieldName, logicOperator, fieldValue, alias, ref parameters);
                    break;
                default:
                    condition = BuildDefaultCondition(fieldName, logicOperator, fieldType, fieldValue, alias, ref parameters);
                    break;
            }
        }

        private static string BuildMultiFieldCondition(string[] fields, string logicOperator, string fieldValue, ref List<object> parameters)
        {
            bool isNegative = logicOperator is "!=" or AppInp.Input_Search_Operator_not_contains;
            string operatorSymbol = logicOperator switch
            {
                "=" or "is" => "=",
                "!=" or AppInp.Input_Search_Operator_is_not => "!=",
                AppInp.Input_Search_Operator_contains => "Contains",
                AppInp.Input_Search_Operator_not_contains => "!Contains",
                _ => "="
            };

            parameters.Add(fieldValue);
            int paramIndex = parameters.Count - 1;
            var conditions = fields.Select(field =>
            {
                if (operatorSymbol == "Contains")
                {
                    return $"({field} != null && {field}.Contains(@{paramIndex}))";
                }
                if (operatorSymbol == "!Contains")
                {
                    return $"({field} == null || !{field}.Contains(@{paramIndex}))";
                }
                if (operatorSymbol == "!=")
                {
                    // For !=, ensure the field is either null or not equal to the value
                    return $"({field} == null || {field} != @{paramIndex})";
                }
                return $"{field}{operatorSymbol}@{paramIndex}";
            }).ToList();

            // Use AND for != and !Contains to ensure all fields satisfy the condition
            string joinOperator = operatorSymbol == "!=" || operatorSymbol == "!Contains" ? " And " : " Or ";
            string condition = $"({string.Join(joinOperator, conditions)})";
            return condition;
        }

        private static string BuildDateTimeCondition(string fieldName, string logicOperator, string fieldValue, string alias, ref List<object> parameters)
        {
            alias = string.IsNullOrEmpty(alias) ? "" : $"{alias}.";
            try
            {
                DateTime d1 = Convert.ToDateTime(fieldValue);
                DateTime d2 = d1.AddDays(1);

                switch (logicOperator)
                {
                    case "=":
                        parameters.Add(d1);
                        parameters.Add(d2);
                        return $"({alias}{fieldName}>=@{parameters.Count - 2} And {alias}{fieldName}<@{parameters.Count - 1})";
                    case "!=":
                        parameters.Add(d1);
                        parameters.Add(d2);
                        return $"({alias}{fieldName}<@{parameters.Count - 2} Or {alias}{fieldName}>=@{parameters.Count - 1})";
                    case ">" or "<=":
                        DateTime d1x = Convert.ToDateTime(fieldValue + " 23:59:59");
                        parameters.Add(d1x);
                        return $"{alias}{fieldName}{logicOperator}@{parameters.Count - 1}";
                    case ">=" or "<":
                        parameters.Add(d1);
                        return $"{alias}{fieldName}{logicOperator}@{parameters.Count - 1}";
                    default:
                        return "";
                }
            }
            catch (FormatException)
            {
                return "";
            }
        }

        private static string BuildStringCondition(string fieldName, string logicOperator, string fieldValue, string alias, ref List<object> parameters)
        {
            alias = string.IsNullOrEmpty(alias) ? "" : $"{alias}.";
            parameters.Add(fieldValue);
            return logicOperator switch
            {
                "=" => $"{alias}{fieldName}=@{parameters.Count - 1}",
                "!=" => $"{alias}{fieldName}!=@{parameters.Count - 1}",
                AppInp.Input_Search_Operator_contains => $"{alias}{fieldName}.Contains(@{parameters.Count - 1})",
                AppInp.Input_Search_Operator_not_contains => $"!{alias}{fieldName}.Contains(@{parameters.Count - 1})",
                _ => ""
            };
        }

        private static string BuildDefaultCondition(string fieldName, string logicOperator, string fieldType, string fieldValue, string alias, ref List<object> parameters)
        {
            alias = string.IsNullOrEmpty(alias) ? "" : $"{alias}.";
            if (fieldType == "number")
                parameters.Add(Convert.ToInt32(fieldValue));
            else
                parameters.Add(fieldValue);
            return $"{alias}{fieldName} {logicOperator}@{parameters.Count - 1}";
        }

        private static void RenameCaseProperties(JObject caseJson)
        {
            var propertyMap = new Dictionary<string, string>
            {
                { "Opened_By", "Case_Opened_By" },
                { "Opened_Time", "Case_Opened_Time" },
                { "Created_By", "Case_Created_By" },
                { "Created_Time", "Case_Created_Time" },
                { "Updated_By", "Case_Updated_By" },
                { "Updated_Time", "Case_Updated_Time" },
                { AppInp.Input_Is_Valid, "Case_Is_Valid" }
            };

            foreach (var prop in propertyMap)
            {
                if (caseJson.Property(prop.Key) != null)
                {
                    caseJson[prop.Value] = caseJson[prop.Key];
                    caseJson.Remove(prop.Key);
                }
            }
        }
    }
}