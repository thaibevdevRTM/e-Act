using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLibrary
{
    public class MitTool
    {
        #region Export Excel
        private void ClearControl(Control ctrl)
        {
            for (int index = ctrl.Controls.Count - 1; index > 0; index--)
            {
                ClearControl(ctrl.Controls[index]);
            }
            if (!(ctrl is TableCell))
            {
                if (ctrl.GetType().GetProperty("SelectedItem") != null || ctrl.GetType().GetProperty("Text") != null)
                {
                    LiteralControl lControl = new LiteralControl();
                    ctrl.Parent.Controls.Add(lControl);
                    lControl.Text = (string)((ctrl.GetType().GetProperty("SelectedItem") != null)
                        ? ctrl.GetType().GetProperty("SelectedItem").GetValue(ctrl, null)
                        : ctrl.GetType().GetProperty("Text").GetValue(ctrl, null));
                    ctrl.Parent.Controls.Remove(ctrl);
                }
                else ctrl.Parent.Controls.Remove(ctrl);
            }
            return;
        }
        /// <summary>
        /// Export DataGrid To Excel
        /// </summary>
        /// <param name="response">ส่ง Response</param>
        /// <param name="thisPage">ส่ง this มาเลย</param>
        /// <param name="objDataGrid">ส่ง DataGrid ที่ต้องการ Export</param>
        public void ExportToExcel(System.Web.HttpResponse response, System.Web.UI.Page thisPage, GridView objDataGrid)
        {
            response.Clear();
            response.Buffer = false;
            response.ContentType = "application/vnd.ms-excel";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Charset = "";
            thisPage.EnableViewState = false;
            System.IO.StringWriter strWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hWrite = new HtmlTextWriter(strWrite);
            ClearControl(objDataGrid);
            objDataGrid.RenderControl(hWrite);
            response.Write(strWrite.ToString());
            response.End();
        }
        /// <summary>
        /// Export DataGrid To Excel
        /// </summary>
        /// <param name="response">ส่ง Response</param>
        /// <param name="thisPage">ส่ง this</param>
        /// <param name="objDataGrid">ส่ง DataGrid ที่ต้องการ Export</param>
        /// <param name="header">ชื่อ Header</param>
        /// <param name="footer">ชื่อ Footer</param>
        public void ExportToExcel(System.Web.HttpResponse response, System.Web.UI.Page thisPage, GridView objDataGrid, string header, string footer)
        {
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/vnd.ms-excel";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Charset = "";
            //Response.AddHeader("Content-Disposition", "attachment;filename=Clientes.xls");

            string open = @"<html xmlns:x=""urn:schemas-microsoft-com:office:excel""><head><style>
			<!--table
			br {mso-data-placement:same-cell;}
			tr {vertical-align:middle;}
			td {mso-number-format:\@;font-family:Tahoma, sans-serif;font-size:8.0pt;text-align:center;}			
			--></style></head> <body>";

            response.Write(open);

            thisPage.EnableViewState = false;
            if (header != "")
            {
                response.Write("<font face='Tahoma'><b>" + header + "</b></font>");
            }
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            this.ClearControl(objDataGrid);

            for (int rowPos = 0; rowPos < objDataGrid.Rows.Count; ++rowPos)
            {
                for (int colPos = 0; colPos < objDataGrid.Rows[rowPos].Cells.Count; ++colPos)
                {
                    objDataGrid.Rows[rowPos].Cells[colPos].Attributes.Add("class", "NumberString");
                }
            }
            objDataGrid.RenderControl(oHtmlTextWriter);

            string styleInfo = @"<style>.NumberString    {mso-number-format:\@;}</style>";
            response.Write(styleInfo + oStringWriter.ToString());
            response.Write(footer);
            response.Write("</body></html>");
            response.End();

        }
        #endregion

        #region Cookie Service

        public static void CreateCookie(string values, System.Web.HttpResponse response, string cookieName, double addMinutes)
        {
            try
            {
                response.Cookies[cookieName].Value = values;
                response.Cookies[cookieName].Expires = DateTime.Now.AddMinutes(addMinutes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetCookie(System.Web.HttpRequest request, string cookieName)
        {
            try
            {
                string strValues = "";
                if (request.Cookies[cookieName] != null && request.Cookies[cookieName].Value != null)
                {
                    strValues = request.Cookies[cookieName].Value;
                }
                return strValues;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteCookie(System.Web.HttpResponse response, string cookieName)
        {
            try
            {
                System.Web.HttpCookie userCookie = new System.Web.HttpCookie(cookieName);
                userCookie.Expires = DateTime.Now.AddDays(-1);
                response.Cookies.Add(userCookie);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetSeite
        public static string GetSiteRootAndApplicationPath()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

            if (port == null || port == "80" || port == "443")
            {
                port = "";
            }
            else
            {
                port = ":" + port;
            }

            string protocall = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocall == null || protocall == "0")
            {
                protocall = "http://";
            }
            else
            {
                protocall = "https://";
            }

            return protocall + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] +
                port + System.Web.HttpContext.Current.Request.ApplicationPath;
        }
        public static string GetRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

            if (port == null || port == "80" || port == "443")
            {
                port = "";
            }
            else
            {
                port = ":" + port;
            }

            string protocall = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocall == null || protocall == "0")
            {
                protocall = "http://";
            }
            else
            {
                protocall = "https://";
            }

            return protocall + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port;
        }
        #endregion

    }
}
