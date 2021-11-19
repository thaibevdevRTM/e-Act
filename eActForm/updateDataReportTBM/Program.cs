
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace updateDataReportTBM
{
    public class Program
    {
        static void Main(string[] args)
        {

            var result = AppCode.updateDataToTable();
            if (result)
            {
                Console.WriteLine("Update Report Success");
            }
                
            
        }
    }
}
