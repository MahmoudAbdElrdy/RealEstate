<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPage.aspx.cs" Inherits="ReportFormCore.Reports.ReportForm.ReportPage" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <style>
        body:nth-of-type(1) img[src*="Blank.gif"] {
            display: none;
        }
		
        /*#reportViewer1_ReportViewer #reportViewer1 #reportViewer1_HttpHandlerMissingErrorMessage {
	        display:block!important;
        }*/

        html, body {
            width: 80%;
            height: 100%;
        }

        #reportViewer {
            width: 100% !important;
            height: 100%;
        }

        

      

    </style>
</head>
<body dir="rtl" style="width: 94%">
<form id="form1" runat="server" style="float:left!important">
    <%-- <asp:DropDownList runat="server" ID="lstPrinters"></asp:DropDownList>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <rsweb:ReportViewer ID="reportViewer1" Height="100%" Width="100%" SizeToReportContent="true" ShowPrintButton="true" AsyncRendering="false"  runat="server"></rsweb:ReportViewer>
    <%-- style="float:left!important"--%>
</form>
</body>
</html>
