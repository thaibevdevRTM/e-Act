using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;
using eActForm.Models;
using eActForm.BusinessLayer;
using System.IO;

namespace eActConsoleService
{
    class DialyService
    {
        static void Main(string[] args)
        {
            StreamWriter strLogs = File.AppendText(new DialyService().LogsFileName);
            try
            {
                strLogs.WriteLine("== Start Service " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
                string strBody = "", strName = "";
                ApproveModel.approveWaitingModels model = new ApproveModel.approveWaitingModels();
                model.waitingLists = AppCode.getAllWaitingApproveGroupByEmpId();
                foreach (ApproveModel.approveWaitingModel m in model.waitingLists)
                {
                    try
                    {
                        if (m.empEmail == "")
                        {
                            strLogs.WriteLine(" Error User : " + m.empId + " mess : email this user is empty");
                            m.empEmail = Properties.Settings.Default.strDefaultEmail;
                        }
                        strName = m.empPrefix + " " + m.empFNameTH + " " + m.empLNameTH;
                            strBody = string.Format(Properties.Settings.Default.strBody, strName, m.waitingCount,Properties.Settings.Default.strUrlApprove);
                            new MailOffice365G(Properties.Settings.Default.strMailUser
                                , Properties.Settings.Default.strMailPassword
                                , Properties.Settings.Default.strMailUser
                                , m.empEmail
                                , Properties.Settings.Default.strSubject
                                , strBody
                                , Properties.Settings.Default.strMailCC).Send();
                        
                    }
                    catch (Exception ex)
                    {
                        strLogs.WriteLine(" Error User : " + m.empId +" mess : " + ex.Message);
                        strLogs.WriteLine("== END " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
                    }
                }
            }
            catch(Exception ex)
            {
                strLogs.WriteLine(" Error Main : " + ex.Message);
                strLogs.WriteLine("== END " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
            }
            finally
            {
                strLogs.Flush();
                strLogs.Close();
            }

        }
        private string LogsFileName
        {
            get
            {
                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                string str = string.Format(Properties.Settings.Default.logsFileName, new string[] { Path.GetDirectoryName(location), DateTime.Now.ToString("ddMMyyyy") });
                return str;
            }
        }
    }

}
