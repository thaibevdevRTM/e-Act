using eActForm.Presenter.MasterData;
using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eForms.Presenter.AppCode
{
    public class ImportBudgetControlAppCode
    {
        public static string channel = "channel";
        public static string brand = "brand";


        public static string getLE_No(string strCon)
        {
            string result = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getLE_No");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new
                             {
                                 LE = d["LE"].ToString(),
                             });
                if (lists.Any())
                {
                    result = lists.FirstOrDefault().LE;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getLE_No");
            }

            return result;
        }
        public static int getBudget_No(string strCon)
        {
            int result = 0;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getBudget_No");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new
                             {
                                 bgNo = d["budgetNo"].ToString(),
                             });
                if (lists.Any())
                {
                    result = int.Parse(lists.FirstOrDefault().bgNo);
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getLE_No");
            }

            return result;
        }
        public static int InsertBudgetControl(string strCon, ImportBudgetControlModel.BudgetControlModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetControl"
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetNo",model.budgetNo)
                         ,new SqlParameter("@EO",model.EO)
                         ,new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@budgetGroupType",model.budgetGroupType)
                         ,new SqlParameter("@customerId",model.customerId)
                         ,new SqlParameter("@chanelId",model.chanelId)
                         ,new SqlParameter("@brandId",model.brandId)
                         ,new SqlParameter("@startDate",model.startDate)
                         ,new SqlParameter("@endDate",model.endDate)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@LE",model.LE)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetControl => " + ex.Message);
            }

            return result;
        }

        public static int InsertBudgetLE(string strCon, ImportBudgetControlModel.BudgetControl_LEModel model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLE"
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetId",model.budgetId)
                         ,new SqlParameter("@startDate",model.startDate)
                         ,new SqlParameter("@endDate",model.endDate)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@description",model.descripion)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetLE => " + ex.Message);
            }

            return result;
        }

        public static int InsertBudgetLE_History(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLE_History");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> InsertBudgetLE_History");
            }

            return result;
        }

        public static int InsertBudgetActType(string strCon, ImportBudgetControlModel.BudgetControl_ActType model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActType"
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetId",model.budgetId)
                         ,new SqlParameter("@budgetLEId",model.budgetLEId)
                         ,new SqlParameter("@actTypeId",model.actTypeId)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@description",model.description)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetActType => " + ex.Message);
            }

            return result;
        }
        public static int InsertBudgetActType_History(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActType_History");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> InsertBudgetActType_History");
            }

            return result;
        }

        public static string genEO(string strCon, BudgetControlModels modelBudget)
        {
            string result = "";
            try
            {
                var genGroup = "{0}";
                if (!string.IsNullOrEmpty(modelBudget.chanelName))
                {
                    if (modelBudget.chanelName.Equals("ONT"))
                    {
                        genGroup = "B2";
                    }
                    else if (modelBudget.chanelName.Equals("TT") || modelBudget.chanelName.Equals("CVM") || modelBudget.chanelName.Equals("SSC"))
                    {
                        genGroup = "B3";
                    }
                    else if (modelBudget.chanelName.Equals("MT"))
                    {
                        genGroup = "B4";
                    }
                }

                result = QueryGetAllBrand.GetAllBrand(strCon).Where(x => x.id.Equals(modelBudget.brandId)).FirstOrDefault().digit_EO;
                result += getDigitDepartment(modelBudget.budgetGroupType);
                result += genGroup; //group
                result += modelBudget.startDate.Value.Year.ToString().Substring(2, 2);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("genEO Presenter => " + ex.Message);
            }
            return result;
        }


        public static string getDigitDepartment(string p)
        {
            var result = "";
            if(p == channel)
            {
                result = "11";
            }
            else
            {
                result = "10";
            }
            return result;
        }

    }
}
