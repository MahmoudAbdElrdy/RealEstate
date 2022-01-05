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
        private void BindReport3(string reportName, dynamic DataSet, dynamic DataSet2)
        {

            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/" + reportName + ".rdlc");// reportPath + ReportName + ".rdlc";
            reportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource source = new ReportDataSource(reportName, DataSet);
            ReportDataSource source2 = new ReportDataSource("CustomerCardStock2", DataSet2);

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
                        Model.id = null;
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
                    if (FromDate == "null")
                        Model.FromDate = null;
                    else
                        Model.FromDate = FromDate;

                    if (ToDate == "null")
                        Model.ToDate = null;
                    else
                        Model.ToDate = FromDate;
                    
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

        public List<CustomerCard> CustomerCardStock() 
        {
            List<CustomerCard> dataSets = new List<CustomerCard>();
            string customerName = null;
            if (Request.QueryString["customerName"] != null)
            {
                customerName = Request.QueryString["customerName"].ToString();
            }
           
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (customerName == "null")
                        Model.customerName = null;
                    else
                        Model.customerName = customerName;
                   


                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/CustomerCard/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data").ToObject<List<CustomerCard>>();
                        var results2 = o2.Value<JArray>("data2").ToObject<List<CustomerCard>>();

                        var parmarter = o2.Value<JObject>("data3").ToObject<ContractReportDto>();
                       // var list = results.Where(x => x.IsExtra);
                        BindReport3("CustomerCardStock", results,results2);
                        //this.reportViewer1.LocalReport.SetParameters(new ReportParameter("CustomerName", customerName));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectName", parmarter.ProjectName ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Name", parmarter.Name ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Number", parmarter?.Number.ToString() ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Area", parmarter?.Area.ToString() ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("FloorNumber", parmarter?.FloorNumber.ToString() ?? "")); 
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("NationalNumber", parmarter.NationalNumber ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Address", parmarter.Address ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Phone", parmarter?.Phone ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectAddress", parmarter.ProjectAddress ?? "")); ;
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Notes", parmarter.Notes ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Date", parmarter?.Date.ToString("dd-MM-yyyy") ?? ""));
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
        public List<CustomerCard> CustomerCardStock2() 
        {
            return new List<CustomerCard>();
        }
        public List<ViewCustomerDatum> SalesYear() 
        {
            List<ViewCustomerDatum> dataSets = new List<ViewCustomerDatum>();
            string year = null;
            if (Request.QueryString["year"] != null)
            {
                year = Request.QueryString["year"].ToString();
            }

            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (year == "null")
                        Model.year = null;
                    else
                        Model.year = year;



                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportSalesYear/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data").ToObject<List<ViewCustomerDatum>>();
                        var results2 = o2.Value<JArray>("data2").ToObject<List<ViewCancelledContract>>();

                       
                        reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/SalesYear.rdlc");// reportPath + ReportName + ".rdlc";
                        reportViewer1.LocalReport.DataSources.Clear();

                        ReportDataSource source = new ReportDataSource("ViewCustomerData", results);
                        ReportDataSource source2 = new ReportDataSource("CancelledContract", results2);

                        reportViewer1.LocalReport.DataSources.Add(source);
                        reportViewer1.LocalReport.DataSources.Add(source2);
                        reportViewer1.DataBind();
                        //this.reportViewer1.LocalReport.SetParameters(new ReportParameter("CustomerName", customerName));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Year", year ?? ""));
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
        public List<ViewCustomerDatum> CustomerData()
        {
            List<ViewCustomerDatum> dataSets = new List<ViewCustomerDatum>();
            string ProjectId = null;
            if (Request.QueryString["ProjectId"] != null)
            {
                ProjectId = Request.QueryString["ProjectId"].ToString();
            }

            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(UrlAPI);

                    dynamic Model = new ExpandoObject();
                    if (ProjectId == "ProjectId")
                        Model.ProjectId = 0;
                    else
                        Model.ProjectId = ProjectId;



                    var jsonString = JsonConvert.SerializeObject(Model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();

                    var response = client.PostAsync("api/Reports/ReportCustomerData/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var results = o2.Value<JArray>("data").ToObject<List<ViewCustomerDatum>>();

                        int option = 0;
                        if (Request.QueryString["option"] != null)
                        {
                            option =Convert.ToInt32(Request.QueryString["option"].ToString());
                        }
                        if (option == 1)
                        {
                            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/CustomerData.rdlc");
                        }
                        else if (option == 2)
                        {
                            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/CustomerData2.rdlc");
                        }
                        else if (option == 3)
                        {
                            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/CustomerData3.rdlc");
                        }
                        else if (option == 4)
                        {
                            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/CustomerData4.rdlc");
                        }
                        else if (option == 5)
                        {
                            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/CustomerData5.rdlc");
                        }
                        else
                        {
                            reportViewer1.LocalReport.ReportPath = Server.MapPath("../ReportDesigne/CustomerData.rdlc");
                        }
      
                        reportViewer1.LocalReport.DataSources.Clear();

                        ReportDataSource source = new ReportDataSource("ViewCustomerData", results);
                      
                        reportViewer1.LocalReport.DataSources.Add(source);
                        reportViewer1.DataBind();
                        //this.reportViewer1.LocalReport.SetParameters(new ReportParameter("CustomerName", customerName));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Notes", ""));
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

        public List<PrintBill> PrintBill()  
        {
            List<PrintBill> dataSets = new List<PrintBill>();
            string id = null;
            if (Request.QueryString["id"] != null)
            {
                id = Request.QueryString["id"].ToString();
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

                    var response = client.PostAsync("api/Reports/ReportBill/", content);
                    response.Wait();

                    dynamic ResultResponse = response.Result.Content.ReadAsStringAsync();

                    {


                        var o2 = JsonConvert.DeserializeObject<JObject>(ResultResponse.Result);
                        var data = o2.Value<JArray>("data") 
                            .ToObject<List<PrintBill>>();
                        string Paid = ((Newtonsoft.Json.Linq.JValue)o2["message"])?.Value.ToString();
                      

                        BindReport("PrintBill", data);
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("ProjectName", data[0].ProjectName ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("CustomerPhone", data[0].CustomerPhone ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("NationalNumber", data[0].NationalNumber ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Stock", data[0].Stock ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("CustomerName", data[0].CustomerName ?? ""));
                        this.reportViewer1.LocalReport.SetParameters(new ReportParameter("Paid", Paid));
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
public class ContractReportDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string NationalNumber { get; set; }
    public string Phone { get; set; }
    public DateTime? Date { get; set; }
    public string Program { get; set; }
    public string Address { get; set; }
    public bool? IsStock { get; set; }
    public double? TotalCost { get; set; }
    public double? MeterCost { get; set; }
    public int? ProjectUnitId { get; set; }
    public string ProjectName { get; set; }
    public double? StockCount { get; set; }
    public double? MetersCount { get; set; }
    public string Notes { get; set; }
    public int ProjectId { get; set; }

    public int? Number { get; set; }
    public int? FloorNumber { get; set; }
    public double? Area { get; set; }
    public string ProjectAddress { get; set; }
    public int? Floors { get; set; }
  
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