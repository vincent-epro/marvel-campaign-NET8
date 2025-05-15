using marvel_campaign_NET8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Data;
using System.Text.Json.Nodes;

namespace marvel_campaign_NET8.Controllers
{
    [Route("api")]
    [ApiController]
    public class TraditionalOutboundFileController : ControllerBase
    {
      //  private readonly ScrmDbContext _scrme; //

        private readonly string rootPath;
        public TraditionalOutboundFileController(IConfiguration iConfig, ScrmDbContext context)
        {
            rootPath = iConfig.GetValue<string>("CallListUpload_path") ?? "";

         //   _scrme = context; //
        }


        // Upload Excel Get Worksheet
        [Route("UploadExcelGetWorksheet")]
        [HttpPost]
        public IActionResult UploadExcelGetWorksheet()
        {
            try
            {
                string token = string.Empty;
                string tk_agentId = string.Empty;

                //   int agentId = 0;//

                if (Request.Form.Files.Count == 0)
                {
                    return Ok(new { result = AppOutp.OutputResult_FAIL, details = "No file was uploaded." });
                }
                var file = Request.Form.Files[0];

                foreach (var key in Request.Form.Keys)
                {
                    if (key == "Agent_Id")
                    {
                        //    agentId = Convert.ToInt32(Request.Form[key]);//

                        tk_agentId = Convert.ToString(Request.Form[key]);
                    }
                    else if (key == "Token")
                    {
                        token = Convert.ToString(Request.Form[key]) ?? string.Empty;
                    }
                }


                if (ValidateClass.Authenticated(token, tk_agentId))
                {

                    //  string mediaType = file.ContentType; //

                    // extract only the filename
                    string fileName = Path.GetFileName(file.FileName);

                    // decide the whole file path
                    string filePath = Path.Combine(rootPath, fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // string mediaLink = ".\\fb_media\\" + fileName; //


                    var builder = new OleDbConnectionStringBuilder
                    {
                        Provider = "Microsoft.ACE.OLEDB.12.0",
                        DataSource = filePath
                    };

                    builder["Extended Properties"] = "Excel 12.0 Xml;HDR=YES;";

                    string strconn = builder.ConnectionString;

                    List<string> File_SheetName = new List<string>();

                    using (OleDbConnection objConn = new OleDbConnection(strconn))
                    {
                        objConn.Open();
                        DataTable dataTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                       File_SheetName = new List<string>();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            File_SheetName.Add(row["TABLE_NAME"].ToString());
                        }

                        objConn.Close();
                    }


                    return Ok(new
                    {
                        result = AppOutp.OutputResult_SUCC,
                        filepath = filePath,
                        worksheet = File_SheetName
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


        // Get Excel Worksheet Header
        [Route("GetExcelWorksheetHeader")]
        [HttpPost]
        public IActionResult GetExcelWorksheetHeader([FromBody] JsonObject data)
        {
            string token = (data[AppInp.InputAuth_Token] ?? "").ToString();
            string tk_agentId = (data[AppInp.InputAuth_Agent_Id] ?? "").ToString();

            try
            {
                if (ValidateClass.Authenticated(token, tk_agentId))
                {
                    string filePath = (data["File_Path"] ?? "").ToString();
                    string worksheet = (data["WorkSheet"] ?? "").ToString();

                    if (!System.IO.File.Exists(filePath))
                    {
                        return Ok(new { result = AppOutp.OutputResult_FAIL, details = "File not found." });
                    }

                    string strconn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" +
                       filePath + "';Extended Properties='Excel 12.0 Xml;HDR=YES;';";

                    string strworksheet = "Select * FROM [" + worksheet + "]";

                    OleDbConnection conn = new OleDbConnection(strconn);
                    OleDbDataAdapter oda = new OleDbDataAdapter(strworksheet, conn);

                    DataTable dtExcelData = new DataTable();
                    oda.Fill(dtExcelData);

                    oda.Dispose();
                    conn.Close();


                    List<string> Call_list_ColumnName = new List<string>();

                    foreach (DataColumn col in dtExcelData.Columns)
                        Call_list_ColumnName.Add(col.ColumnName);


                    return Ok(new
                    {
                        result = AppOutp.OutputResult_SUCC,
                        details = new
                        {
                            // column names
                            Call_list_ColumnName
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






    }
}
