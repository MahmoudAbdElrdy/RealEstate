using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReportFormCore.Data.StoredProc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;


namespace ReportFormCore.Reports.ReportForm 
{
    public partial class ReportPage : System.Web.UI.Page
    {
      
        private string UrlAPI = WebConfigurationManager.AppSettings["baseUrl"];
 
        protected void Page_Load(object sender, EventArgs e)
       {
        string ReportName = Request["ReportName"];
            if (!IsPostBack && ReportName!= null)
            {
                var method = this.GetType().GetMethod(ReportName);
                method.Invoke(this, null);
              
            }


        }
        private void BindReport(string reportName,  dynamic DataSet)
        {

            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/" + reportName + ".rdlc");// reportPath + ReportName + ".rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
             
           ReportDataSource source = new ReportDataSource(reportName, DataSet);

          reportViewer1.LocalReport.DataSources.Add(source);
          reportViewer1.DataBind();    
        }
        public List<ExtraContrcat> ContractExtraDataSet()  
        {
            List<ExtraContrcat> accountStatements = new List<ExtraContrcat>();
            string projectID = Request["ProjectID"].ToString();

            string contractExtraName = Request["ContractExtraName"].ToString();
          
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                  
                    Model.projectID = projectID;
                  
                    Model.contractExtraName = contractExtraName;
                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportExtraContrcat/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {
                        
                        var DataSet = (JsonConvert.DeserializeObject<ResponseData>(ResultResponse.Result));
                        JObject dynJson = JsonConvert.DeserializeObject(ResultResponse.Result) as JObject;

                        //var o = JsonConvert.DeserializeObject<JObject>(ResultResponse);
                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data")
                            .ToObject<List<ExtraContrcat>>();
                        string message = ((Newtonsoft.Json.Linq.JValue)o2["message"])?.Value.ToString();

                        ///var item = DataSet.Data.data;
                        //dynJson.Dump();
                        BindReport("ExtraContrcat",results);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ExtraName", contractExtraName));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectName", "ProjectName"));

                    }

                }
                return accountStatements;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }

        }

    }
}
public enum EResponse
{
    OK,
    Unauthorized,
    NoPermission,
    NoData,
    ValidationError,
    UnSuccess,
    UnexpectedError
}
public class ResponseData
{
    public EResponse Code { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string[] Errors { get; set; }
    public dynamic Data { get; set; }
    public int? TotalRecordsCount { get; set; }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
    public int? PageCount { get; set; }

}