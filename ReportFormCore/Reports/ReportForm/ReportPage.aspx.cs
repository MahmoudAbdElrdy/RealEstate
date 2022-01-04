using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReportFormCore.Data.Models;
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
        private void BindReport2(string reportName, dynamic DataSet, dynamic DataSet2)
        {

            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/" + reportName + ".rdlc");// reportPath + ReportName + ".rdlc";
            reportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource source = new ReportDataSource(reportName, DataSet);
            ReportDataSource source2 = new ReportDataSource("ViewDailyReports", DataSet2);

            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.LocalReport.DataSources.Add(source2);
            reportViewer1.DataBind();
        }
        public List<ExtraContrcat> ExtraContrcat()  
        {
            List<ExtraContrcat> dataSets = new List<ExtraContrcat>();
            string projectID =null;
            if (Request.QueryString["ProjectID"] != null)
            {
                projectID = Request.QueryString["ProjectID"].ToString();
            }
            string contractExtraName = null;
            if (Request.QueryString["ContractExtraName"] != null)
            {
                contractExtraName = Request.QueryString["ContractExtraName"].ToString();
            }
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
                        string ProjectName = ((Newtonsoft.Json.Linq.JValue)o2["message"])?.Value.ToString(); 

                        ///var item = DataSet.Data.data;
                        //dynJson.Dump();
                        BindReport("ExtraContrcat", results);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ExtraName", contractExtraName));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectName", ProjectName));

                    }

                }
                return dataSets;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }

        }
        public List<Alert> Alert() 
        {
            List<Alert> dataSets = new List<Alert>();
            string id = null;
            if (Request.QueryString["projectID"] != null)
            {
                id = Request.QueryString["projectID"].ToString();
            }
            string from = null;
            if (Request.QueryString["fromDate"] != null)
            {
                from = Request.QueryString["fromDate"].ToString();
            }
            string to = null;
            if (Request.QueryString["toDate"] != null)
            {
                to = Request.QueryString["toDate"].ToString();
            }
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (id == "null")
                        Model.id = 0;
                    else
                    Model.id = id;

                    Model.from = from;
                    Model.to = to;
                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportAlert/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {

                       
                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data")
                            .ToObject<List<Alert>>();
                        string ProjectName = ((Newtonsoft.Json.Linq.JValue)o2["message"])?.Value.ToString();
                        BindReport("Alert", results);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectName", ProjectName));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("FromDate", from));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ToDate", to));

                    }

                }
                return dataSets;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }

        }
        public List<Alert> Overdue()
        {
            List<Alert> dataSets = new List<Alert>();
            string id = null;
            if (Request.QueryString["projectID"] != null)
            {
                id = Request.QueryString["projectID"].ToString();
            }
            
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (id == "null")
                        Model.id = 0;
                    else
                        Model.id = id;

                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportOverdue/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data")
                            .ToObject<List<Alert>>();
                        string ProjectName = ((Newtonsoft.Json.Linq.JValue)o2["message"])?.Value.ToString();
                        BindReport("Overdue", results);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectName", ProjectName));
                       
                    }

                }
                return dataSets;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }

        }
        public List<SupervisorDetail> Supervisor()
        {
            List<SupervisorDetail> dataSets = new List<SupervisorDetail>();
            string SupervisorId = null;
            if (Request.QueryString["supervisorId"] != null)
            {
                SupervisorId = Request.QueryString["supervisorId"].ToString();
            }
            string FromDate = null;
            if (Request.QueryString["fromDate"] != null)
            {
                FromDate = Request.QueryString["fromDate"].ToString();
            }
            string ToDate = null;
            if (Request.QueryString["toDate"] != null)
            {
                ToDate = Request.QueryString["toDate"].ToString();
            }
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (SupervisorId == "null")
                        Model.SupervisorId = 0;
                    else
                        Model.SupervisorId = SupervisorId;

                    Model.FromDate = FromDate;
                    Model.ToDate = ToDate;
                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportSupervisor/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data")
                            .ToObject<List<SupervisorDetail>>();
                        var results2 = o2.Value<JArray>("data2")
                         .ToObject<List<ViewDailyReport>>();
                        string supervisor = ((Newtonsoft.Json.Linq.JValue)o2["message"])?.Value.ToString();
                        BindReport2("Supervisor", results,results2);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Supervisor", supervisor ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("FromDate", FromDate));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ToDate", ToDate));

                    }

                }
                return dataSets;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }

        }
        public List<ViewDailyReport> ViewDailyReports()
        {
            return new List<ViewDailyReport>();
        }
        public List<Customer> CustomerWaiting() 
        {
            List<Customer> dataSets = new List<Customer>();
            string region = null;
            if (Request.QueryString["region"] != null)
            {
                region = Request.QueryString["region"].ToString();
            }
            string FromDate = null;
            if (Request.QueryString["fromDate"] != null)
            {
                FromDate = Request.QueryString["fromDate"].ToString();
            }
            string ToDate = null;
            if (Request.QueryString["toDate"] != null)
            {
                ToDate = Request.QueryString["toDate"].ToString();
            }
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (region == "null")
                        Model.Region = null;
                    else
                        Model.Region = region;
                    if (FromDate == "null")
                        Model.FromDate = null;
                    else
                    Model.FromDate = FromDate;
                    if (ToDate == "null")
                        Model.ToDate = null;
                    else
                        Model.ToDate = ToDate;

                
                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportCustomerWaiting/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data")
                            .ToObject<List<Customer>>();
                       
                        BindReport("CustomerWaiting", results);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Region", region ));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("FromDate", FromDate));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ToDate", ToDate));

                    }

                }
                return dataSets;
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