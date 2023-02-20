using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebLibrary;

namespace eForm.Approve
{
    class Program
    {
        static async Task Main(string[] args)
        {
            StreamWriter strLogs = File.AppendText(new Program().LogsFileName);
            try
            {
                strLogs.WriteLine("== Start Service " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
                var result = AppCode.getWaitApprove();
                if (result.Count > 0)
                {
                    int countSuccess = 0, countUnSuccess = 0;
                    foreach(var item in result)
                    {
                        try
                        {

                            item.producerDetail.companyName = item.companyName;
                            item.producerDetail.attachedFileName = item.attachedFileName;
                            item.producerDetail.attachedUrl = item.attachedUrl;

                            using (var httpClient = new HttpClient())
                            {
                                using (var request1 = new HttpRequestMessage(new HttpMethod("POST"), Properties.Resources.apiBevApprove))
                                {
                                    var contentList = new List<string>();
                                    contentList.Add($"data={Uri.EscapeDataString("P1;P2")}");
                                    contentList.Add($"format={Uri.EscapeDataString("json")}");
                                    request1.Content = new StringContent(string.Join("&", contentList));
                                    request1.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");


                                    var response1 = await httpClient.SendAsync(request1);
                                }
                            }

                          

                            ConsumerApproverBevAPI response = null;
                            ProducerApproverBevAPI request = new ProducerApproverBevAPI(item.docNo, MainAppCode.ApproveStatus.CREATE.ToString(), "1.0.0", DateTime.Now.ToString(), item);

                            string conjson = JsonConvert.SerializeObject(request).ToString();
        
                                string rtn1 = OkHTTP.SendGet(Properties.Resources.apiBevApprove);ces.apiBevApprove, JsonConvert.SerializeObject(request).ToString());
                            JObject json = JObject.Parse(rtn);
                            if (json.Count > 0)
                            {
                                response = JsonConvert.DeserializeObject<ConsumerApproverBevAPI>(rtn);
                            }


                        }
                        catch(Exception ex)
                        {

                        }






                    
                    }

                    Console.WriteLine("Update Report Success");


                    strLogs.WriteLine("== Update Report Success " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss "));
                }
            }
            catch (Exception ex)
            {
                strLogs.WriteLine(" Error User :" + ex.Message);
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
                string str = string.Format(Properties.Resources.logsFileName, new string[] { Path.GetDirectoryName(location), DateTime.Now.ToString("ddMMyyyy") });
                return str;
            }
        }

    }
}
