using eActForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace eActForm.BusinessLayer
{
    public class UtilsAppCode
    {
        public class Session
        {
            private static HttpSessionState CurrentSession
            {
                get
                {
                    return HttpContext.Current.Session;
                }
            }

            private static T GetSessionValue<T>(string name)
            {
                return (T)CurrentSession[name];
            }

            private static List<T> GetSessionList<T>(string name)
            {
                return (List<T>)CurrentSession[name];
            }

            private static string GetSessionValueString(string name)
            {
                return GetSessionValue<string>(name);
            }

            private static void SetSessionValue(string name, object value)
            {
                CurrentSession[name] = value;
            }

            public static void ClearSession()
            {
                CurrentSession.Clear();
            }


            public static ActUserModel.User User
            {
                get
                {
                    var obj = GetSessionValue<ActUserModel.User>("USER");

                    return obj;
                }
                set
                {
                    SetSessionValue("USER", value);
                }
            }


            public static string writeFileHistory(HttpServerUtility server, byte[] imgByte, string filePath)
            {
                try
                {
                    if (imgByte.Length > 0)
                    {
                        File.WriteAllBytes(server.MapPath(filePath), imgByte);
                    }
                    return filePath;
                }
                catch (Exception ex)
                {
                    return server.MapPath(filePath) + ex.Message;
                }
            }


        }
    }
}