using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using WebLibrary;

namespace eActForm.Models
{
    public class AppCode
    {
        public static string StrCon = System.Configuration.ConfigurationManager.ConnectionStrings["ActDB_ConnectionString"].ConnectionString;
       


       

        public static List<Productcostdetail> getProductcostdetail(string p_productcateid, string p_size, string p_customerid)
        {
            DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductcost"
                 , new SqlParameter("@p_productcateid", p_productcateid)
                 , new SqlParameter("@p_size", p_size)
                 , new SqlParameter("@p_customerid", p_customerid));
            var lists = (from DataRow d in ds.Tables[0].Rows
                         select new Productcostdetail()
                         {
                             productId = d["productId"].ToString(),
                             productName = d["productName"].ToString(),
                             wholeSalesPrice = decimal.Parse(d["price"].ToString()),
                             normalCost = decimal.Parse(d["price"].ToString()),
                         });
            return lists.ToList();
        }



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public static void CheckFolder_CreateNotHave_Direct(string path)
        {

            DirectoryInfo ObjDirItemNo = new DirectoryInfo(path);

            if (ObjDirItemNo.Exists == false)
            {
                ObjDirItemNo.Create();
            }

        }

    }
}