using eForms.Models.MasterData;
using RepoDb;
using System.Data;
using System.Data.SqlClient;

namespace eForms.Data
{
    public class DdlData
    {
        public static List<TB_Act_ProductBrand_Model> GetBrandBudgetControl(string srtCon)
        {
            try
            {
                using (var connection = new SqlConnection(srtCon))
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

        public static List<TB_Act_Chanel_Model> GetChannelBudgetControl(string strConn)
        {
            try
            {
                using (var connection = new SqlConnection(strConn))
                {
                    var extractor = connection.ExecuteQueryMultiple("EXEC [dbo].[usp_getChanelBGControl];");
                    return extractor.Extract<TB_Act_Chanel_Model>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetChannelBudgetControl >>" + ex.Message);
            }
        }

        public static List<TB_Act_ActivityGroup_Model> getActivityGroupBudgetControl(string strConn, string condition)
        {
            try
            {
                using (var connection = new SqlConnection(strConn))
                {
                    var extractor = connection.ExecuteQueryMultiple("EXEC [dbo].[usp_getActivityGroupBGControl] @condition;", new { condition = condition });
                    return extractor.Extract<TB_Act_ActivityGroup_Model>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetChannelBudgetControl >>" + ex.Message);
            }
        }

        public static List<TB_Act_Other_Model> getOhterMaster(string strConn, string type,string subType)
        {
            try
            {
                using (var connection = new SqlConnection(strConn))
                {
                    var extractor = connection.ExecuteQueryMultiple("EXEC [dbo].[usp_getAllTOtherMaster] @type,@subType;", new { type = type, subType = subType });
                    return extractor.Extract<TB_Act_Other_Model>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getActivityGroupBudgetControl >>" + ex.Message);
            }
        }


    }
}