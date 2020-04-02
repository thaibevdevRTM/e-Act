using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eActConsoleServiceWeekly
{
    public class appCodeWeeklySummaryOrder
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "bySaleTeam")
                {
                    sentReportBySaleTeam();
                }
                else if (args[0] == "byRegion")
                {

                }
            }
            catch(Exception ex)
            {

            }
        }

        public static string sentReportBySaleTeam()
        {
            try
            {
                return "";
            }catch(Exception ex)
            {
                throw new Exception("sentReportBySaleTeam >> " + ex.Message);
            }
        }
    }
}
