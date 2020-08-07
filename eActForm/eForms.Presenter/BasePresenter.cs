using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace eForms.Presenter
{
    public class BasePresenter
    {
        public static List<T> ToGenericList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                   .Select(c => c.ColumnName)
                   .ToList();

            var properties = typeof(T).GetProperties();
            DataRow[] rows = dt.Select();
            return rows.Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    try
                    {

                        if (columnNames.Contains(pro.Name))
                            pro.SetValue(objT, row[pro.Name]);
                    }
                    catch
                    {
                        //try
                        //{
                        //    pro.SetValue(objT, 0);
                        //}
                        //catch {
                        //    try
                        //    {
                        //        pro.SetValue(objT, "");
                        //    }
                        //    catch
                        //    {
                        //        pro.SetValue(objT, 0.0);
                        //    }
                        //}
                    }
                }

                return objT;
            }).ToList();
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
    }
}
