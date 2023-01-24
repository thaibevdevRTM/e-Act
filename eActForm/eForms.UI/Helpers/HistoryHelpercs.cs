using eForms.Models.MasterData;
using eForms.UI.Controllers;
using RepoDb;
using System.Data.SqlClient;

namespace eForms.UI.Helpers
{
    public class HistoryHelpercs
    {
        private readonly ILogger<HomeController> _logger;
        private static IConfiguration _config = null;
        public HistoryHelpercs(IConfiguration config)
        {
            _config = config;
        }
        public static List<TB_Act_ProductBrand_Model> GetBrandBudgetControl()
        {
            try
            {
                using (var connection = new SqlConnection(_config["AppSettings:DBConnection"].ToString()))
                {

                    var extractor = connection.ExecuteQueryMultiple("EXEC [dbo].[usp_getProductBrandBGControl];");
                     return extractor.Extract<TB_Act_ProductBrand_Model>().ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<TB_Act_ProductBrand_Model>();
            }

        }

        
    }
}
