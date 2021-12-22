﻿using System;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Threading.Tasks;


namespace updateDataReportTBM
{
    public class AppCode
    {
        public static bool updateDataToTable()
        {
            bool result = false;
            try
            {
               
                int rtn = 0;
                Console.Write("waiting for update data");
                waiting();
                 rtn = SqlHelper.ExecuteNonQuery(Properties.Settings.Default.strConn, CommandType.StoredProcedure, "usp_updateReportTBMToTB");
                if (rtn > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception("updateDataToTable >>" + ex.Message);
            }

        }

        public static async Task waiting()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 3; i++)
                {

                    Console.Write('.');
                    System.Threading.Thread.Sleep(1000);
                    if (i == 2)
                    {
                        Console.Write("\r   \r");
                        i = -1;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            });
        }

    }
}