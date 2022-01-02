using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
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
      

    }
}