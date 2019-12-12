using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WebLibrary
{
    public class OkHTTP
    {
        public static string SendGet(String IP_PORT)
        {
            try
            {
                string responseHtml = "";
                string url =
                    string.Format(IP_PORT);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows; U; "
                    + "Windows NT 5.1; en-US; rv:1.7) "
                    + "Gecko/20040707 Firefox/0.9.2";
                request.Method = "GET";
                request.Accept =
                    "text/xml,application/xml,application/xhtml+xml,"
                    + "text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.KeepAlive = true;
                request.ContentType = @"application/x-www-form-urlencoded";

                // Get the response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding enc = System.Text.Encoding.UTF8;
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), enc, true);
                responseHtml = responseStream.ReadToEnd();
                response.Close();
                responseStream.Close();

                return responseHtml;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string SendPost(String IP_PORT, String COMMAND)
        {
            try
            {
                string responseHtml = "";
                string url =
                    string.Format(IP_PORT);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                request.KeepAlive = true;
                request.ContentType = @"application/json;charset=utf-8";

                string postData = COMMAND;
                byte[] postBuffer = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = postBuffer.Length;
                Stream postDataStream = request.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding enc = System.Text.Encoding.UTF8;
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), enc, true);
                responseHtml = responseStream.ReadToEnd();
                response.Close();
                responseStream.Close();

                return responseHtml;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
